using System;
using System.IO;
using System.Net;
using System.Web;
using System.Drawing;
using System.Threading;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web.Script.Serialization;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.RegularExpressions;
using System.Security.Cryptography;

namespace BaiduPCS
{
    class BaiduPCSUtil
    {
        // https://github.com/GangZhuo/BaiduPCS.git

        const int READ_BUF_SIZE = 1024;
        const int DOWNLOAD_BUF_SIZE = 100 * 1024;

        const long RAPIDUPLOAD_THRESHOLD = 256 * 1024;

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

        [DataContract()]
        public class BaiduThumbs
        {
            [DataMember(Name = "icon")]
            internal string m_icon;
            [DataMember(Name = "url1")]
            internal string m_url1;
            [DataMember(Name = "url2")]
            internal string m_url2;
            [DataMember(Name = "url3")]
            internal string m_url3;
        }

        [DataContract()]
        public class BaiduFileInfo
        {
            [DataMember(Name = "fs_id")]
            internal long m_fs_id;
            [DataMember(Name = "isdir")]
            internal int m_is_dir;
            [DataMember(Name = "path")]
            internal string m_path;
            [DataMember(Name = "server_filename")]
            internal string m_server_filename;
            [DataMember(Name = "server_ctime")]
            internal long m_server_ctime;
            [DataMember(Name = "size")]
            internal long m_size;
            [DataMember(Name = "md5", IsRequired = false)]
            internal string m_md5;
            [DataMember(Name = "thumbs", IsRequired = false)]
            internal BaiduThumbs m_thumbs;
        }

        [DataContract()]
        public class BaiduFileList
        {
            [DataMember(Name = "errno")]
            internal int m_errno;
            [DataMember(Name = "list")]
            internal BaiduFileInfo[] m_list;
        }

        [DataContract()]
        public class BaiduFileMgrInfo
        {
            [DataMember(Name = "errno")]
            internal int m_errno;
            [DataMember(Name = "path")]
            internal string m_path;
        }

        [DataContract()]
        public class BaiduFileMgrReturn
        {
            [DataMember(Name = "errno")]
            internal int m_errno;
            [DataMember(Name = "request_id")]
            internal long m_request_id;
            [DataMember(Name = "info")]
            internal BaiduFileMgrInfo[] m_info;
        }

        [DataContract()]
        public class BaiduRenameFileInfo
        {
            [DataMember(Name = "path")]
            internal string m_path;
            [DataMember(Name = "newname")]
            internal string m_newname;
        }

        [DataContract()]
        public class BaiduMkDirInfo
        {
            [DataMember(Name = "errno")]
            internal int m_errno;
            [DataMember(Name = "fs_id")]
            internal long m_fs_id;
            [DataMember(Name = "isdir")]
            internal int m_is_dir;
            [DataMember(Name = "path")]
            internal string m_path;
            [DataMember(Name = "ctime")]
            internal long m_ctime;
        }

        private CookieContainer m_req_cc = null;
        private CookieCollection m_res_cc = null;

        public delegate void OnNewLogDelegate(string new_log);
        private event OnNewLogDelegate m_on_new_log = null;
        public OnNewLogDelegate OnNewLog
        {
            get { return m_on_new_log; }
            set { m_on_new_log = value; }
        }

        public delegate void OnReportProgressDelegate(
            bool is_download,
            long current_size,
            long total_size,
            long current_bytes,
            long total_bytes,
            int current_files,
            int total_files,
            string local_path,
            string remote_path);
        private event OnReportProgressDelegate m_on_report_prog = null;
        public OnReportProgressDelegate OnReportProgress
        {
            get { return m_on_report_prog; }
            set { m_on_report_prog = value; }
        }

        private HttpStatusCode m_http_code = HttpStatusCode.OK;
        public HttpStatusCode HttpCode
        {
            get { return m_http_code; }
        }

        private string m_last_error = "";
        public string LastErrorStr
        {
            get { return m_last_error; }
        }

        private int m_status = 0;
        public int Status
        {
            get { return m_status; }
            set { m_status = value; }
        }

        private static DateTime BASE_TIME = new DateTime(1970, 1, 1);

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

        private void ReportProgress(
            bool is_download,
            long current_size,
            long total_size,
            long current_bytes,
            long total_bytes,
            int current_files,
            int total_files,
            string local_path,
            string remote_path)
        {
            if (null == m_on_report_prog) return;

            m_on_report_prog(is_download,
                current_size, total_size,
                current_bytes, total_bytes,
                current_files, total_files,
                local_path, remote_path);
        }

        /// <summary>
        /// 获取当前 UNIX_TIMESTAMP * 1000
        /// </summary>
        /// <returns></returns>
        static public string GetTimeStamp()
        {
            return ((long)((DateTime.Now - BASE_TIME).TotalSeconds - 8 * 3600) * 1000 & 0x7FFFFFFF).ToString();
        }

        /// <summary>
        /// 获取 UNIX_TIMESTAMP 对应的可读日期时间
        /// </summary>
        /// <param name="unix_time"></param>
        /// <returns></returns>
        static public string FromUnixtime(long unix_time)
        {
            return new DateTime((unix_time + 8 * 3600) *
                TimeSpan.TicksPerSecond + BASE_TIME.Ticks)
                .ToString("yyyy-MM-dd HH:mm:ss");
        }

        /// <summary>
        /// 获取 JSON 指定路径的值
        /// </summary>
        /// <param name="json">JSON 字符串</param>
        /// <param name="path">指定路径，如 "data.errno"</param>
        /// <returns></returns>
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

        private string EncodeJson<T>(T obj) where T : class
        {
            try
            {
                MemoryStream ms = new MemoryStream();
                DataContractJsonSerializer s = new DataContractJsonSerializer(typeof(T));
                s.WriteObject(ms, obj);

                byte[] bytes_json = new byte[ms.Length];
                ms.Position = 0;
                ms.Read(bytes_json, 0, bytes_json.Length);
                ms.Close();

                return Encoding.UTF8.GetString(bytes_json);
            }
            catch (Exception)
            {
                return "";
            }
        }

        private T DecodeJson<T>(string json) where T : class
        {
            try
            {
                byte[] bytes_object = Encoding.UTF8.GetBytes(json);
                MemoryStream ms = new MemoryStream(bytes_object);
                DataContractJsonSerializer s = new DataContractJsonSerializer(typeof(T));
                object obj_ret = s.ReadObject(ms);
                ms.Close();

                if (null == obj_ret ||
                    !(obj_ret is T))
                {
                    return null;
                }

                return obj_ret as T;
            }
            catch (Exception)
            {
                return null;
            }
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

        private string GetDirFileName(string dir_path)
        {
            if ("/" == dir_path) return "/";

            string[] arr_path = dir_path.Split("/".ToCharArray());
            return arr_path[arr_path.Length - 1];
        }

        private string GetDirPath(string str_path)
        {
            if ("/" == str_path) return "/";

            string[] arr_path = str_path.Split("/".ToCharArray());
            return string.Join("/", arr_path, 0, arr_path.Length - 1);
        }

        private string MD5File(FileStream fs, long offset = 0, long length = 0)
        {
            const int SLICE_SIZE = 16 * 1024;

            try
            {
                fs.Seek(offset, SeekOrigin.Begin);
                if (0 == length)
                {
                    length = fs.Length;
                }

                long read_size = 0;
                MD5 md5 = MD5CryptoServiceProvider.Create();
                byte[] bytes_slice = null, bytes_out = null;
                do
                {
                    int slice_size = (int)(length - read_size);
                    if (slice_size > SLICE_SIZE)
                    {
                        slice_size = SLICE_SIZE;
                    }

                    bytes_slice = new byte[slice_size];
                    read_size += fs.Read(bytes_slice, 0, slice_size);

                    if (read_size < length)
                    {
                        bytes_out = new byte[slice_size];
                        md5.TransformBlock(bytes_slice, 0, slice_size, bytes_out, 0);
                    }
                    else
                    {
                        bytes_out = md5.TransformFinalBlock(bytes_slice, 0, slice_size);
                    }
                } while (read_size < length);

                string str_ret = BitConverter.ToString(md5.Hash).Replace("-", "");
                return str_ret;
            }
            catch (Exception ex)
            {
                m_last_error = ex.ToString();
                return "";
            }
        }

        private byte[] BuildKeyValueParams(NameValueCollection nvc, bool need_url_encode = true)
        {
            string str_ret = "";
            foreach (string key in nvc.AllKeys)
            {
                str_ret += key + "=" + (need_url_encode ? HttpUtility.UrlEncode(nvc[key]) : nvc[key]);
                str_ret += "&";
            }

            str_ret = str_ret.TrimEnd("&".ToCharArray());
            Log("params = " + str_ret);

            return Encoding.UTF8.GetBytes(str_ret);
        }

        private bool HttpGet(string url, ref byte[] html)
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
                m_http_code = res.StatusCode;
                m_res_cc.Add(res.Cookies);

                BinaryReader br = new BinaryReader(res.GetResponseStream());
                html = new byte[READ_BUF_SIZE * 2];
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

                Log("返回状态码：" + m_http_code.ToString() + "，返回内容大小：" + offset + " 字节！");

                br.Close(); res.Close();
                return true;
            }
            catch (Exception ex)
            {
                m_last_error = ex.ToString();
                return false;
            }
        }

        private bool HttpPost(string url, ref byte[] html, ref byte[] post_data)
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
                m_http_code = res.StatusCode;
                m_res_cc.Add(res.Cookies);

                BinaryReader br = new BinaryReader(res.GetResponseStream());
                html = new byte[READ_BUF_SIZE * 2];
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

                Log("返回状态码：" + m_http_code.ToString() + "，返回内容大小：" + offset + " 字节！");

                br.Close(); res.Close();
                return true;
            }
            catch (Exception ex)
            {
                m_last_error = ex.ToString();
                return false;
            }
        }

        #endregion

        #region 登录

        private string m_code_string = "";
        private string m_token = "";
        private string m_bdstoken = "";
        private string m_bduss = "";

        private string m_sysuid = "";
        public string SysUID
        {
            get { return m_sysuid; }
        }

        private int PreLogin(string username)
        {
            byte[] html = null;

            Log("访问百度网盘主页，获取 Cookies");
            bool ret = HttpGet(BAIDU_DISK_HOME, ref html);
            if (!ret)
            {
                return 1;
            }

            Log("登录第一步，获取 token");
            const string LOGIN_STEP1_URL = "https://passport.baidu.com/v2/api/?getapi&tpl=netdisk&apiver=v3&tt={0}&class=login&logintype=basicLogin&callback=bd__cbs__pwxtn7";
            ret = HttpGet(string.Format(LOGIN_STEP1_URL, GetTimeStamp()), ref html);
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
            ret = HttpGet(string.Format(LOGIN_STEP2_URL, m_token, GetTimeStamp(), username), ref html);
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
            NameValueCollection nvc = new NameValueCollection();
            nvc.Add("staticpage", "http://pan.baidu.com/res/static/thirdparty/pass_v3_jump.html");
            nvc.Add("charset", "utf-8");
            nvc.Add("token", m_token);
            nvc.Add("tpl", "netdisk");
            nvc.Add("subpro", "");
            nvc.Add("apiver", "v3");
            nvc.Add("tt", GetTimeStamp());
            nvc.Add("codestring", m_code_string);
            nvc.Add("safeflg", "0");
            nvc.Add("u", "http://pan.baidu.com/");
            nvc.Add("isPhone", "");
            nvc.Add("quick_user", "0");
            nvc.Add("logintype", "basicLogin");
            nvc.Add("logLoginType", "pc_loginBasic");
            nvc.Add("idc", "");
            nvc.Add("loginmerge", "true");
            nvc.Add("username", username);
            nvc.Add("password", password);
            nvc.Add("verifycode", captcha);
            nvc.Add("mem_pass", "on");
            nvc.Add("rsakey", "");
            nvc.Add("crypttype", "");
            nvc.Add("ppui_logintime", "2602");
            nvc.Add("callback", "parent.bd__pcbs__msdlhs");

            byte[] html = null;
            byte[] post_data = BuildKeyValueParams(nvc);
            bool ret = HttpPost(BAIDU_PASSPORT_API + "login", ref html, ref post_data);
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

                        ret = HttpGet(str_jump_url, ref html);
                        if (!ret)
                        {
                            return 22;
                        }

                        if (HttpStatusCode.OK != m_http_code)
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

                        NameValueCollection nvc_href = HttpUtility.ParseQueryString(str_href);
                        foreach (string key in nvc_href.AllKeys)
                        {
                            if (0 == string.Compare("codeString", key))
                            {
                                m_code_string = nvc_href[key];
                                Log("codeString = " + nvc_href[key]);
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

        /// <summary>
        /// 登录网盘
        /// </summary>
        /// <param name="username">账号</param>
        /// <param name="password">密码</param>
        /// <param name="captcha">验证码</param>
        /// <returns></returns>
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

        /// <summary>
        /// 是否已登录网盘
        /// </summary>
        /// <returns></returns>
        public bool IsLogin()
        {
            byte[] html = null;
            bool ret = HttpGet(BAIDU_DISK_HOME, ref html);
            if (!ret || HttpStatusCode.OK != m_http_code)
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

            //Log("BDUSS = " + m_bduss);
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

            //Log("bdstoken = " + m_bdstoken);
            //Log("sysUID = " + m_sysuid);
            return true;
        }

        /// <summary>
        /// 登出网盘
        /// </summary>
        /// <returns></returns>
        public bool Logout()
        {
            if (!IsLogin())
            {
                return false;
            }

            byte[] html = null;
            return HttpGet(BAIDU_PASSPORT_LOGOUT, ref html);
        }

        /// <summary>
        /// 获取校验码图像
        /// </summary>
        /// <returns></returns>
        public Bitmap GetCaptcha()
        {
            byte[] html = null;
            bool ret = HttpGet(BAIDU_CAPTCHA + m_code_string, ref html);
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

        /// <summary>
        /// 返回文件(夹)列表
        /// </summary>
        /// <param name="bdfl">返回的列表信息</param>
        /// <param name="dir_path">要枚举的目录</param>
        /// <returns></returns>
        public bool List(ref List<BaiduFileInfo> lst_bdfi, string dir_path = "/")
        {
            const int NUM_PER_PAGE = 100;

            int page = 1;
            bool ret = false;
            BaiduFileList bdfl = null;

            lst_bdfi.Clear();
            do
            {
                NameValueCollection nvc = new NameValueCollection();
                nvc.Add("channel", "chunlei");
                nvc.Add("clienttype", "0");
                nvc.Add("web", "1");
                nvc.Add("t", GetTimeStamp());
                nvc.Add("bdstoken", m_bdstoken);
                nvc.Add("_", GetTimeStamp());
                nvc.Add("dir", dir_path);
                nvc.Add("page", page.ToString());
                nvc.Add("num", NUM_PER_PAGE.ToString());
                nvc.Add("order", "name");

                byte[] html = null;
                string str_params = Encoding.UTF8.GetString(BuildKeyValueParams(nvc));
                ret = HttpGet(BAIDU_PAN_API + "list?" + str_params, ref html);
                if (!ret)
                {
                    break;
                }

                string str_json = Encoding.UTF8.GetString(html);
                //Log("json = " + str_json);

                bdfl = DecodeJson<BaiduFileList>(str_json);
                if (null == bdfl ||
                    null == bdfl.m_list ||
                    0 != bdfl.m_errno)
                {
                    break;
                }

                page++;
                lst_bdfi.AddRange(bdfl.m_list);
            } while (bdfl.m_list.Length >= NUM_PER_PAGE);

            return true;
        }

        private bool AnalyFileManagerReturn(BaiduFileMgrInfo bdfmi)
        {
            switch (bdfmi.m_errno)
            {
                case 0:
                    return true;

                case -8:
                    m_last_error = "文件已存在于目标文件夹中";
                    return false;

                case -9:
                    m_last_error = "文件不存在";
                    return false;

                case -10:
                    m_last_error = "剩余空间不足";
                    return false;

                default:
                    m_last_error = "未知错误：" + bdfmi.m_errno;
                    return false;
            }
        }

        private bool DoBaiduFileManagerAPI(string str_params, byte[] post_data)
        {
            byte[] html = null;
            bool ret = HttpPost(BAIDU_PAN_API + "filemanager?" + str_params, ref html, ref post_data);
            if (!ret)
            {
                return false;
            }

            string str_html = Encoding.UTF8.GetString(html);
            //Log("html = " + str_html);

            BaiduFileMgrReturn bd_ret = DecodeJson<BaiduFileMgrReturn>(str_html);
            if (null == bd_ret)
            {
                return false;
            }

            if (0 != bd_ret.m_errno ||
                1 != bd_ret.m_info.Length)
            {
                Log("错误码：" + bd_ret.m_errno);
                return false;
            }

            return AnalyFileManagerReturn(bd_ret.m_info[0]);
        }

        /// <summary>
        /// 重命名文件(夹)
        /// </summary>
        /// <param name="src_path">源路径</param>
        /// <param name="dst_name">目标名称(不能含路径)</param>
        /// <returns></returns>
        public bool Rename(string src_path, string dst_name)
        {
            NameValueCollection nvc = new NameValueCollection();
            nvc.Add("channel", "chunlei");
            nvc.Add("clienttype", "0");
            nvc.Add("web", "1");
            nvc.Add("t", GetTimeStamp());
            nvc.Add("bdstoken", m_bdstoken);
            nvc.Add("opera", "rename");

            string str_params = Encoding.UTF8.GetString(BuildKeyValueParams(nvc));

            BaiduRenameFileInfo[] bdrfi = new BaiduRenameFileInfo[] {
                new BaiduRenameFileInfo() {
                    m_path = src_path,
                    m_newname = dst_name
                }
            };
            string file_list = EncodeJson<BaiduRenameFileInfo[]>(bdrfi);
            Log("file_list = " + file_list);

            NameValueCollection nvc_post = new NameValueCollection();
            nvc_post.Add("filelist", file_list);
            byte[] bytes_post = BuildKeyValueParams(nvc_post);

            return DoBaiduFileManagerAPI(str_params, bytes_post);
        }

        /// <summary>
        /// 删除文件(夹)
        /// </summary>
        /// <param name="file_path">文件路径</param>
        /// <returns></returns>
        public bool Delete(string file_path)
        {
            NameValueCollection nvc = new NameValueCollection();
            nvc.Add("channel", "chunlei");
            nvc.Add("clienttype", "0");
            nvc.Add("web", "1");
            nvc.Add("t", GetTimeStamp());
            nvc.Add("bdstoken", m_bdstoken);
            nvc.Add("opera", "delete");

            string str_params = Encoding.UTF8.GetString(BuildKeyValueParams(nvc));

            NameValueCollection nvc_post = new NameValueCollection();
            nvc_post.Add("filelist", "[\"" + file_path + "\"]");
            byte[] bytes_post = BuildKeyValueParams(nvc_post);

            return DoBaiduFileManagerAPI(str_params, bytes_post);
        }

        /// <summary>
        /// 新建文件夹
        /// </summary>
        /// <param name="dir_path">新文件夹路径</param>
        /// <param name="bdfi">新文件夹信息</param>
        /// <returns></returns>
        public bool MkDir(string dir_path, ref BaiduFileInfo bdfi)
        {
            NameValueCollection nvc = new NameValueCollection();
            nvc.Add("channel", "chunlei");
            nvc.Add("clienttype", "0");
            nvc.Add("web", "1");
            nvc.Add("t", GetTimeStamp());
            nvc.Add("bdstoken", m_bdstoken);
            nvc.Add("a", "commit");

            string str_params = Encoding.UTF8.GetString(BuildKeyValueParams(nvc));

            NameValueCollection nvc_post = new NameValueCollection();
            nvc_post.Add("path", dir_path);
            nvc_post.Add("isdir", "1");
            nvc_post.Add("size", "");
            nvc_post.Add("block_list", "[]");
            nvc_post.Add("method", "post");

            byte[] bytes_post = BuildKeyValueParams(nvc_post);

            byte[] html = null;
            bool ret = HttpPost(BAIDU_PAN_API + "create?" + str_params, ref html, ref bytes_post);
            if (!ret)
            {
                return false;
            }

            string str_html = Encoding.UTF8.GetString(html);
            //Log("html = " + str_html);

            BaiduMkDirInfo bdmdi = DecodeJson<BaiduMkDirInfo>(str_html);
            if (null == bdmdi)
            {
                return false;
            }

            if (0 != bdmdi.m_errno)
            {
                return false;
            }

            bdfi.m_fs_id = bdmdi.m_fs_id;
            bdfi.m_is_dir = bdmdi.m_is_dir;
            bdfi.m_path = bdmdi.m_path;
            bdfi.m_server_ctime = bdmdi.m_ctime;
            bdfi.m_server_filename = GetDirFileName(bdmdi.m_path);

            return true;
        }

        /// <summary>
        /// 创建下载目录
        /// </summary>
        /// <param name="base_path">当前远程目录</param>
        /// <param name="src_path">远程源路径</param>
        /// <param name="dst_path">本地目标路径</param>
        /// <param name="total_size">所有文件总大小</param>
        /// <param name="d_path">所有“远程路径-本地路径”键值对</param>
        /// <returns></returns>
        private bool PrepareDownload(
            string base_path,
            List<BaiduFileInfo> src_path,
            string dst_path,
            ref long total_size,
            ref Dictionary<string, string> d_path)
        {
            try
            {
                foreach (BaiduFileInfo bdfi in src_path)
                {
                    System.Windows.Forms.Application.DoEvents();

                    if (1 == bdfi.m_is_dir)
                    {
                        List<BaiduFileInfo> lst_bdfi = new List<BaiduFileInfo>();
                        if (List(ref lst_bdfi, bdfi.m_path))
                        {
                            string dst_dir = dst_path + "\\" + GetDirFileName(bdfi.m_path);
                            if (!Directory.Exists(dst_dir))
                            {
                                Directory.CreateDirectory(dst_dir);
                            }

                            if (!PrepareDownload(bdfi.m_path, lst_bdfi, dst_dir, ref total_size, ref d_path))
                            {
                                return false;
                            }
                        }
                    }
                    else
                    {
                        string str_file = dst_path + "\\" + bdfi.m_server_filename;
                        d_path.Add(bdfi.m_path, str_file);
                        total_size += bdfi.m_size;
                    }
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 下载文件(夹)
        /// </summary>
        /// <param name="base_path">当前远程目录</param>
        /// <param name="src_path">远程源路径</param>
        /// <param name="dst_path">本地目标路径</param>
        /// <returns></returns>
        public bool Download(string base_path, List<BaiduFileInfo> src_path, string dst_path)
        {
            long current_size = 0, total_size = 0;
            Dictionary<string, string> d_path = new Dictionary<string, string>();
            bool ret = PrepareDownload(base_path, src_path, dst_path, ref total_size, ref d_path);
            if (!ret)
            {
                return false;
            }

            m_status = 0;
            int cur_files = 1, total_files = d_path.Count;
            foreach (KeyValuePair<string, string> kv in d_path)
            {
                FileStream fs = null;
                if (1 == m_status)
                {
                    while (1 == m_status)
                    {
                        System.Windows.Forms.Application.DoEvents();
                        System.Threading.Thread.Sleep(10);
                    }
                }
                else if (2 == m_status)
                {
                    return false;
                }

                try
                {
                    NameValueCollection nvc = new NameValueCollection();
                    nvc.Add("method", "download");
                    nvc.Add("app_id", "250528");
                    nvc.Add("path", kv.Key);

                    string str_params = Encoding.UTF8.GetString(BuildKeyValueParams(nvc));
                    string url = BAIDU_PCS_REST + "?" + str_params;
                    Log("GET " + url);

                    HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(url);
                    req.Proxy = null;
                    req.Timeout = 20000;
                    req.UserAgent = USER_AGENT;
                    req.Method = WebRequestMethods.Http.Get;
                    req.CookieContainer = m_req_cc;

                    HttpWebResponse res = (HttpWebResponse)req.GetResponse();
                    m_http_code = res.StatusCode;

                    if (HttpStatusCode.OK != m_http_code)
                    {
                        Log(kv.Key + " 下载失败：" + m_http_code);
                        continue;
                    }

                    int offset = 0, read_len = 0;

                    long total_read = 0;
                    long total_len = res.ContentLength;

                    byte[] buf = new byte[DOWNLOAD_BUF_SIZE];

                    fs = File.OpenWrite(kv.Value);
                    BinaryReader br = new BinaryReader(res.GetResponseStream());
                    do
                    {
                        offset += read_len;
                        total_read += read_len;
                        current_size += read_len;
                        if (offset + READ_BUF_SIZE > buf.Length)
                        {
                            fs.Write(buf, 0, offset);
                            offset = 0;

                            if (1 == m_status)
                            {
                                while (1 == m_status)
                                {
                                    System.Windows.Forms.Application.DoEvents();
                                    System.Threading.Thread.Sleep(10);
                                }
                            }
                            else if (2 == m_status)
                            {
                                br.Close(); fs.Close(); res.Close();
                                return false;
                            }

                            ReportProgress(true,
                                current_size, total_size,
                                total_read, total_len,
                                cur_files, total_files,
                                kv.Value, kv.Key);
                        }

                        read_len = br.Read(buf, offset, READ_BUF_SIZE);
                    } while (0 != read_len);

                    offset += read_len;
                    total_read += read_len;
                    fs.Write(buf, 0, offset);
                    br.Close(); res.Close();

                    ReportProgress(true,
                        current_size, total_size,
                        total_read, total_len,
                        cur_files, total_files,
                        kv.Value, kv.Key);

                    Log("返回状态码：" + m_http_code.ToString() + "，返回内容大小：" + total_read + " 字节！");
                }
                catch (Exception ex)
                {
                    m_last_error = ex.ToString();
                }

                cur_files++;
                if (null != fs)
                {
                    fs.Close();
                }
            }

            return true;
        }

        /// <summary>
        /// 创建上传目录
        /// </summary>
        /// <param name="base_path">当前本地目录</param>
        /// <param name="src_path">本地源路径</param>
        /// <param name="dst_path">远程目标路径</param>
        /// <param name="total_size">所有文件总大小</param>
        /// <param name="d_path">所有“本地路径-远程路径”键值对</param>
        /// <returns></returns>
        private bool PrepareUpload(
            string base_path,
            string[] src_path,
            string dst_path,
            ref long total_size,
            ref Dictionary<string, string> d_path)
        {
            try
            {
                foreach (string str_path in src_path)
                {
                    System.Windows.Forms.Application.DoEvents();

                    FileInfo fi = new FileInfo(str_path);
                    if (fi.Attributes.HasFlag(FileAttributes.Directory))
                    {
                        BaiduFileInfo bdfi = new BaiduFileInfo();
                        string dst_dir = dst_path + "/" + Path.GetDirectoryName(str_path);
                        MkDir(dst_dir, ref bdfi);

                        if (!PrepareUpload(str_path, Directory.GetFileSystemEntries(str_path), dst_dir, ref total_size, ref d_path))
                        {
                            return false;
                        }
                    }
                    else
                    {
                        total_size = fi.Length;
                        d_path.Add(str_path, dst_path + "/" + fi.Name);
                    }
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 快速上传
        /// </summary>
        /// <param name="fs">源文件</param>
        /// <param name="dst_file">目标路径</param>
        /// <returns></returns>
        private bool RapidUpload(FileStream fs, string dst_file)
        {
            try
            {
                NameValueCollection nvc = new NameValueCollection();
                nvc.Add("method", "rapidupload");
                nvc.Add("app_id", "250528");
                nvc.Add("ondup", "overwrite");
                nvc.Add("dir", GetDirPath(dst_file));
                nvc.Add("filename", GetDirFileName(dst_file));
                nvc.Add("content-length", fs.Length.ToString());
                nvc.Add("content-md5", MD5File(fs));
                nvc.Add("slice-md5", MD5File(fs, 0, RAPIDUPLOAD_THRESHOLD));
                nvc.Add("path", dst_file);
                nvc.Add("BDUSS", m_bduss);
                nvc.Add("bdstoken", m_bdstoken);

                string str_params = Encoding.UTF8.GetString(BuildKeyValueParams(nvc));
                string url = BAIDU_PCS_REST + "?" + str_params;
                Log("GET " + url);

                byte[] html = null;
                if (!HttpGet(url, ref html))
                {
                    Log(dst_file + " 快速上传失败：" + m_http_code);
                    return false;
                }

                if (HttpStatusCode.OK != m_http_code)
                {
                    Log(dst_file + " 快速上传失败：" + m_http_code);
                    return false;
                }

                string str_html = Encoding.UTF8.GetString(html);
                Log("html = " + str_html);

                object error_code = GetJsonValue(str_html, "error_code");
                if (null != error_code)
                {
                    object error_msg = GetJsonValue(str_html, "error_msg");
                    Log(dst_file + " 快速上传失败：" + error_code.ToString() + "，" + error_msg);
                    return false;
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 一般上传
        /// </summary>
        /// <param name="fs">源文件</param>
        /// <param name="src_file">源文件路径</param>
        /// <param name="dst_file">目标路径</param>
        /// <returns></returns>
        private bool NormalUpload(FileStream fs, string src_file, string dst_file)
        {
            try
            {
                NameValueCollection nvc = new NameValueCollection();
                nvc.Add("method", "upload");
                nvc.Add("app_id", "250528");
                nvc.Add("ondup", "overwrite");
                nvc.Add("dir", GetDirPath(dst_file));
                nvc.Add("filename", GetDirFileName(dst_file));
                nvc.Add("BDUSS", m_bduss);

                string str_params = Encoding.UTF8.GetString(BuildKeyValueParams(nvc));
                string url = BAIDU_PCS_REST + "?" + str_params;
                Log("GET " + url);

                string str_boundary = string.Format("----------{0}", DateTime.Now.Ticks.ToString("x"));

                HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(url);
                req.Proxy = null;
                req.Timeout = 20000;
                req.UserAgent = USER_AGENT;
                req.Method = WebRequestMethods.Http.Post;
                req.CookieContainer = m_req_cc;
                req.ContentType = "multipart/form-data; boundary=" + str_boundary;

                byte[] bytes_disposition =
                    Encoding.UTF8.GetBytes(string.Format(
                        "--{0}\r\nContent-Disposition: form-data; name=\"{1}\"; filename=\"{2}\"\r\nContent-Type: {3}\r\n\r\n",
                        str_boundary,
                        Path.GetFileName(src_file),
                        GetDirFileName(dst_file),
                        "application/octet-stream"));

                byte[] file_content = new byte[fs.Length];
                fs.Read(file_content, 0, file_content.Length);

                byte[] bytes_footer = Encoding.UTF8.GetBytes("\r\n--" + str_boundary + "--\r\n");

                req.ContentLength = bytes_disposition.Length + file_content.Length + bytes_footer.Length;
                req.GetRequestStream().Write(bytes_disposition, 0, bytes_disposition.Length);
                req.GetRequestStream().Write(file_content, 0, file_content.Length);
                req.GetRequestStream().Write(bytes_footer, 0, bytes_footer.Length);
                Log("POST " + req.ContentLength + " 字节数据至 " + url);

                HttpWebResponse res = (HttpWebResponse)req.GetResponse();
                m_http_code = res.StatusCode;
                if (HttpStatusCode.OK != m_http_code)
                {
                    return false;
                }

                BinaryReader br = new BinaryReader(res.GetResponseStream());
                byte[] html = new byte[READ_BUF_SIZE * 2];
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

                Log("返回状态码：" + m_http_code.ToString() + "，返回内容大小：" + offset + " 字节！");

                br.Close(); res.Close();

                string str_html = Encoding.UTF8.GetString(html);
                object error_code = GetJsonValue(str_html, "error_code");
                if (null != error_code)
                {
                    object error_msg = GetJsonValue(str_html, "error_msg");
                    Log(dst_file + " 一般上传失败：" + error_code.ToString() + "，" + error_msg);
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                m_last_error = ex.ToString();
                return false;
            }
        }

        /// <summary>
        /// 上传文件(夹)
        /// </summary>
        /// <param name="base_path">当前本地目录</param>
        /// <param name="src_path">源路径</param>
        /// <param name="dst_path">目标路径</param>
        /// <returns></returns>
        public bool Upload(string base_path, string[] src_path, string dst_path)
        {
            long current_size = 0, total_size = 0;
            Dictionary<string, string> d_path = new Dictionary<string, string>();
            bool ret = PrepareUpload(base_path, src_path, dst_path, ref total_size, ref d_path);
            if (!ret)
            {
                return false;
            }

            m_status = 0;
            int cur_files = 1, total_files = d_path.Count;
            foreach (KeyValuePair<string, string> kv in d_path)
            {
                FileStream fs = null;
                if (1 == m_status)
                {
                    while (1 == m_status)
                    {
                        System.Windows.Forms.Application.DoEvents();
                        System.Threading.Thread.Sleep(10);
                    }
                }
                else if (2 == m_status)
                {
                    return false;
                }

                try
                {
                    fs = File.OpenRead(kv.Key);
                    if (fs.Length < RAPIDUPLOAD_THRESHOLD)
                    {
                        ret = NormalUpload(fs, kv.Key, kv.Value);
                    }
                    else
                    {
                        ret = RapidUpload(fs, kv.Value);
                        if (ret)
                        {
                            current_size += fs.Length;
                        }
                        else
                        {
                            ret = NormalUpload(fs, kv.Key, kv.Value);
                        }
                    }

                    if (!ret)
                    {
                        fs.Close();
                        cur_files++;
                        continue;
                    }

                    ReportProgress(false,
                        current_size, total_size,
                        fs.Length, fs.Length,
                        cur_files, total_files,
                        kv.Key, kv.Value);
                }
                catch (Exception)
                {
                    return false;
                }

                fs.Close();
                cur_files++;
            }

            return true;
        }

        #endregion
    }
}
