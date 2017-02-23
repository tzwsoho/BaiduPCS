using System;
using System.IO;
using System.Net;
using System.Web;
using System.Drawing;
using System.Threading;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web.Script.Serialization;
using System.Text;
using System.Text.RegularExpressions;

namespace BaiduPCS
{
    class BaiduPCSUtil
    {
        const int READ_BUF_SIZE = 1024;

        const string BAIDU_HOME = "http://www.baidu.com";
        const string BAIDU_DISK_HOME = "http://pan.baidu.com/disk/home";
        const string BAIDU_PASSPORT_API = "https://passport.baidu.com/v2/api/?";
        const string BAIDU_GET_PUBLIC_KEY = "https://passport.baidu.com/v2/getpublickey?";
        const string BAIDU_PASSPORT_LOGOUT = "https://passport.baidu.com/?logout&u=http://pan.baidu.com";
        const string BAIDU_CAPTCHA = "https://passport.baidu.com/cgi-bin/genimage?";
        const string BAIDU_PAN_API = "http://pan.baidu.com/api/";
        const string BAIDU_PCS_REST = "http://c.pcs.baidu.com/rest/2.0/pcs/file";
        const string USER_AGENT = "Mozilla/5.0 (Windows NT 6.3; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/35.0.1916.153 Safari/537.36";

        #region 公共

        public delegate void OnNewLogDelegate(string new_log);
        private event OnNewLogDelegate m_on_new_log = null;
        public OnNewLogDelegate OnNewLog
        {
            get { return m_on_new_log; }
            set { m_on_new_log = value; }
        }

        private CookieContainer m_req_cc = null;
        private CookieCollection m_res_cc = null;

        private string m_str_error = "";
        public string LastErrorStr
        {
            get { return m_str_error; }
        }

        public BaiduPCSUtil()
        {
            m_req_cc = new CookieContainer();
            m_res_cc = new CookieCollection();
        }

        private void Log(string new_log)
        {
            if (null == m_on_new_log) return;

            m_on_new_log(new_log);
        }

        private string GetTimeStamp()
        {
            return (DateTime.Now - new DateTime(1970, 1, 1)).TotalSeconds.ToString();
        }

        public object GetJsonValue(string json, string path)
        {
            JavaScriptSerializer jss = new JavaScriptSerializer();
            Dictionary<string, object> d = jss.DeserializeObject(json) as Dictionary<string, object>;
            if (null == d) return null;

            string[] arr_path = path.Split(".".ToCharArray());
            for (int i = 0; i < arr_path.Length; i++)
            {
                if (d.ContainsKey(arr_path[i]))
                {
                    if (i == arr_path.Length - 1)
                    {
                        return d[arr_path[i]];
                    }
                    else if (d[arr_path[i]] is Dictionary<string, object>)
                    {
                        d = d[arr_path[i]] as Dictionary<string, object>;
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }

            return null;
        }

        private string GetStringIn(string str_org, string str_start, string str_end)
        {
            Regex reg = new Regex("(?<=" + str_start + ").+" + "(?=" + str_end + ")", RegexOptions.Multiline);
            if (!reg.IsMatch(str_org))
            {
                return "";
            }

            return reg.Match(str_org).Value;
        }

        private string GetNumberByKey(string str_org, string key)
        {
            Regex reg = new Regex("(?<=" + key + "\\=)\\d*", RegexOptions.Multiline);
            if (!reg.IsMatch(str_org))
            {
                return "";
            }

            return reg.Match(str_org).Value;
        }

        private string GetValueByKey(string str_org, string key)
        {
            Regex reg = new Regex("(?<=" + key + "\\=)[^&=\"]*");
            if (!reg.IsMatch(str_org))
            {
                return "";
            }

            return reg.Match(str_org).Value;
        }

        private byte[] BuildKeyValueParams(NameValueCollection kv, bool need_url_encode = true)
        {
            string str_ret = "";
            foreach (string key in kv.AllKeys)
            {
                str_ret += key + "=" + (need_url_encode ? HttpUtility.UrlEncode(kv[key]) : kv[key]);
                str_ret += "&";
            }

            str_ret = str_ret.TrimEnd("&".ToCharArray());
            Log("params = " + str_ret);

            return Encoding.UTF8.GetBytes(str_ret);
        }

        private bool HttpGet(string url, ref byte[] html, ref HttpStatusCode http_code)
        {
            try
            {
                HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(url);
                req.Proxy = null;
                req.Timeout = 20000;
                req.UserAgent = USER_AGENT;
                req.Method = WebRequestMethods.Http.Get;
                req.CookieContainer = m_req_cc;

                Log("GET " + url);
                HttpWebResponse res = (HttpWebResponse)req.GetResponse();
                http_code = res.StatusCode;
                m_res_cc.Add(res.Cookies);

                BinaryReader br = new BinaryReader(res.GetResponseStream());
                html = new byte[READ_BUF_SIZE];
                int offset = 0, read_len = 0;
                do
                {
                    offset += read_len;
                    if (offset + READ_BUF_SIZE > html.Length)
                    {
                        Array.Resize(ref html, html.Length * 2);
                    }

                    read_len = br.Read(html, offset, READ_BUF_SIZE);
                } while (0 != read_len);

                offset += read_len;
                Array.Resize(ref html, offset);

                Log("返回状态码：" + http_code.ToString() + "，返回内容大小：" + offset + " 字节！");

                br.Close(); res.Close();
                return true;
            }
            catch (Exception ex)
            {
                m_str_error = ex.ToString();
                return false;
            }
        }

        private bool HttpPost(string url, ref byte[] html, ref byte[] post_data, ref HttpStatusCode http_code)
        {
            try
            {
                HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(url);
                req.Proxy = null;
                req.Timeout = 20000;
                req.UserAgent = USER_AGENT;
                req.ContentType = "application/x-www-form-urlencoded";
                req.ContentLength = post_data.Length;
                req.Method = WebRequestMethods.Http.Post;
                req.CookieContainer = m_req_cc;

                req.GetRequestStream().Write(post_data, 0, post_data.Length);

                Log("POST " + post_data.Length + " 字节数据至 " + url);
                HttpWebResponse res = (HttpWebResponse)req.GetResponse();
                http_code = res.StatusCode;
                m_res_cc.Add(res.Cookies);

                BinaryReader br = new BinaryReader(res.GetResponseStream());
                html = new byte[READ_BUF_SIZE];
                int offset = 0, read_len = 0;
                do
                {
                    offset += read_len;
                    if (offset + READ_BUF_SIZE > html.Length)
                    {
                        Array.Resize(ref html, html.Length * 2);
                    }

                    read_len = br.Read(html, offset, READ_BUF_SIZE);
                } while (0 != read_len);

                offset += read_len;
                Array.Resize(ref html, offset);

                Log("返回状态码：" + http_code.ToString() + "，返回内容大小：" + offset + " 字节！");

                br.Close(); res.Close();
                return true;
            }
            catch (Exception ex)
            {
                m_str_error = ex.ToString();
                return false;
            }
        }

        #endregion

        #region 登录

        string m_code_string = "";
        string m_token = "";
        string m_bdstoken = "";
        string m_sysuid = "";
        string m_bduss = "";

        private int PreLogin(string username)
        {
            byte[] html = null;
            HttpStatusCode http_code = HttpStatusCode.OK;

            Log("访问百度网盘主页，获取 Cookies");
            bool ret = HttpGet(BAIDU_DISK_HOME, ref html, ref http_code);
            if (!ret)
            {
                return 1;
            }

            Log("登录第一步，获取 token");
            const string LOGIN_STEP1_URL = "https://passport.baidu.com/v2/api/?getapi&tpl=netdisk&apiver=v3&tt={0}&class=login&logintype=basicLogin&callback=bd__cbs__pwxtn7";
            ret = HttpGet(string.Format(LOGIN_STEP1_URL, GetTimeStamp()), ref html, ref http_code);
            if (!ret || null == html)
            {
                return 2;
            }

            string str_html = Encoding.UTF8.GetString(html);
            string str_step1 = str_html.EndsWith(")") ? GetStringIn(str_html, "\\(", "\\)") : str_html;
            if ("" == str_step1)
            {
                return 3;
            }

            object code_string = GetJsonValue(str_step1, "data.codeString");
            object token = GetJsonValue(str_step1, "data.token");
            if (null == code_string ||
                null == token ||
                !(code_string is string) ||
                !(token is string))
            {
                return 4;
            }

            Log("token = " + token.ToString());
            foreach (char c in token.ToString().ToCharArray())
            {
                if (!char.IsLetterOrDigit(c))
                {
                    return 5;
                }
            }

            m_token = token.ToString();

            Log("codeString = " + code_string.ToString());
            if ("" != code_string.ToString()) // 需要验证码
            {
                m_code_string = code_string.ToString();
                return 10;
            }

            Log("登录第二步，获取 codeString");
            const string LOGIN_STEP2_URL = "https://passport.baidu.com/v2/api/?logincheck&token={0}&tpl=netdisk&apiver=v3&tt={1}&username={2}&isphone=false&callback=bd__cbs__q4ztud";
            ret = HttpGet(string.Format(LOGIN_STEP2_URL, m_token, GetTimeStamp(), username), ref html, ref http_code);
            if (!ret || null == html)
            {
                return 6;
            }

            str_html = Encoding.UTF8.GetString(html);
            string str_step2 = GetStringIn(str_html, "\\(", "\\)");
            if ("" == str_step2)
            {
                return 7;
            }

            code_string = GetJsonValue(str_step2, "data.codeString");
            if (null == code_string ||
                !(code_string is string))
            {
                return 8;
            }

            // 需要验证码
            Log("codeString = " + code_string.ToString());
            if ("" != code_string.ToString())
            {
                m_code_string = code_string.ToString();
                return 10;
            }

            return 0;
        }

        private int DoLogin(string username, string password, string captcha)
        {
            NameValueCollection kv = new NameValueCollection();
            kv.Add("staticpage", "http://pan.baidu.com/res/static/thirdparty/pass_v3_jump.html");
            kv.Add("charset", "utf-8");
            kv.Add("token", m_token);
            kv.Add("tpl", "netdisk");
            kv.Add("subpro", "");
            kv.Add("apiver", "v3");
            kv.Add("tt", GetTimeStamp());
            kv.Add("codestring", m_code_string);
            kv.Add("safeflg", "0");
            kv.Add("u", "http://pan.baidu.com/");
            kv.Add("isPhone", "");
            kv.Add("quick_user", "0");
            kv.Add("logintype", "basicLogin");
            kv.Add("logLoginType", "pc_loginBasic");
            kv.Add("idc", "");
            kv.Add("loginmerge", "true");
            kv.Add("username", username);
            kv.Add("password", password);
            kv.Add("verifycode", captcha);
            kv.Add("mem_pass", "on");
            kv.Add("rsakey", "");
            kv.Add("crypttype", "");
            kv.Add("ppui_logintime", "2602");
            kv.Add("callback", "parent.bd__pcbs__msdlhs");

            byte[] post_data = BuildKeyValueParams(kv);

            byte[] html = null;
            HttpStatusCode http_code = HttpStatusCode.OK;

            bool ret = HttpPost(BAIDU_PASSPORT_API + "login", ref html, ref post_data, ref http_code);
            if (!ret)
            {
                return 20;
            }

            int err_no = 0;
            string str_html = Encoding.UTF8.GetString(html);
            string str_err_no = GetNumberByKey(str_html, "err_no");
            if ("" == str_err_no ||
                !int.TryParse(str_err_no, out err_no))
            {
                str_err_no = GetNumberByKey(str_html, "error");
                if ("" == str_err_no ||
                    !int.TryParse(str_err_no, out err_no))
                {
                    return 21;
                }
            }

            Log("err_no = " + err_no);
            switch (err_no)
            {
                // 登录成功
                case 0:
                case 18:
                case 400032:
                case 400034:
                case 400037:
                case 400401:
                    {
                        string str_jump_url = GetStringIn(str_html, "decodeURIComponent\\(\"", "\"\\)\\+\"\\?\"");
                        string str_account = GetStringIn(str_html, "var\\s+accounts\\s+\\= '", "'\n\n");
                        string str_href = GetStringIn(str_html, "href\\s+\\+\\=\\s+\"", "\"\\+accounts;");
                        str_jump_url = str_jump_url.Replace("\\", "") + "?" + str_href + str_account;
                        Log("jump_url = " + str_jump_url);

                        ret = HttpGet(str_jump_url, ref html, ref http_code);
                        if (!ret)
                        {
                            return 22;
                        }

                        if (HttpStatusCode.OK != http_code)
                        {
                            return 23;
                        }

                        if (IsLogin())
                        {
                            return 0;
                        }
                    }
                    break;

                // 需要验证码
                case 3:
                case 6:
                case 257:
                case 200010:
                    {
                        string str_href = GetStringIn(str_html, "href\\s+\\+\\=\\s+\"", "\"\\+accounts;");
                        Log("href = " + str_href);

                        NameValueCollection nvc = HttpUtility.ParseQueryString(str_href);
                        foreach (string key in nvc.AllKeys)
                        {
                            if (0 == string.Compare("codeString", key))
                            {
                                m_code_string = nvc[key];
                                Log("codeString = " + nvc[key]);
                                break;
                            }
                        }
                    }
                    return 10;

                // 需要跳转
                case 120019:
                case 120021:
                    {
                        string authtoken = GetStringIn(str_html, "&authtoken\\s*\\=\\s*", "[&\"]");
                        string gotourl = GetStringIn(str_html, "&gotourl\\s*\\=\\s*", "[&\"]");
                        Log("authtoken = " + authtoken);
                        Log("gotourl = " + gotourl);

                        // TODO
                        return err_no;
                    }
                    //break;

                default:
                    return err_no;
            }

            return 0;
        }

        public int Login(string username, string password, string captcha = "")
        {
            int ret = 0;
            if ("" == captcha &&
                "" == m_code_string)
            {
                ret = PreLogin(username);
                if (0 != ret)
                {
                    return ret;
                }
            }

            ret = DoLogin(username, password, captcha);
            if (0 != ret)
            {
                return ret;
            }

            return 0;
        }

        public bool IsLogin()
        {
            byte[] html = null;
            HttpStatusCode http_code = HttpStatusCode.OK;

            bool ret = HttpGet(BAIDU_DISK_HOME, ref html, ref http_code);
            if (!ret || HttpStatusCode.OK != http_code)
            {
                return false;
            }

            foreach (Cookie cookie in m_res_cc)
            {
                if (0 == string.Compare(cookie.Name, "BDUSS", true))
                {
                    m_bduss = cookie.Value;
                    break;
                }
            }

            Log("BDUSS = " + m_bduss);
            if ("" == m_bduss)
            {
                return false;
            }

            string str_html = Encoding.UTF8.GetString(html);
            m_bdstoken = GetStringIn(str_html, "yunData.MYBDSTOKEN\\s*\\=\\s*\"", "\"");
            if ("" == m_bdstoken)
            {
                m_bdstoken = GetStringIn(str_html, "FileUtils.bdstoken\\s*\\=\\s*\"", "\"");
            }

            if ("" == m_bdstoken)
            {
                string str_context = GetStringIn(str_html, "var\\s+context\\s*\\=\\s*", ";[\r]?\n");
                object bdstoken = GetJsonValue(str_context, "bdstoken");
                object sysuid = GetJsonValue(str_context, "username");
                if (null == bdstoken ||
                    null == sysuid ||
                    !(bdstoken is string) ||
                    !(sysuid is string))
                {
                    return false;
                }

                m_bdstoken = bdstoken.ToString();
                m_sysuid = sysuid.ToString();
            }
            else
            {
                m_sysuid = GetStringIn(str_html, "yunData.MYNAME\\s*\\=\\s*\"", "\"");
                if ("" == m_sysuid)
                {
                    m_sysuid = GetStringIn(str_html, "FileUtils.sysUID\\s*\\=\\s*\"", "\"");
                }
            }

            Log("bdstoken = " + m_bdstoken);
            Log("sysUID = " + m_sysuid);
            return true;
        }

        public bool Logout()
        {
            if (!IsLogin())
            {
                return false;
            }

            byte[] html = null;
            HttpStatusCode http_code = HttpStatusCode.OK;

            return HttpGet(BAIDU_PASSPORT_LOGOUT, ref html, ref http_code);
        }

        public Bitmap GetCaptcha()
        {
            byte[] html = null;
            HttpStatusCode http_code = HttpStatusCode.OK;

            bool ret = HttpGet(BAIDU_CAPTCHA + m_code_string, ref html, ref http_code);
            if (!ret)
            {
                return null;
            }

            MemoryStream ms = new MemoryStream(html);
            Bitmap bmp_captcha = new Bitmap(Image.FromStream(ms));
            ms.Close();

            return bmp_captcha;
        }

        #endregion

        #region 文件操作

        public bool List()
        {
            return true;
        }

        #endregion
    }
}
