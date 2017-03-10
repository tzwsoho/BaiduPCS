using System;
using System.IO;
using System.Drawing;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Windows.Forms;
using Microsoft.VisualBasic;

namespace BaiduPCS
{
    public partial class frmMain : Form
    {
        BaiduPCSUtil m_util = new BaiduPCSUtil();

        #region 公共

        private string GetDatetime(DateTime dt)
        {
            return dt.ToString("yyyy-MM-dd HH:mm:ss");
        }

        private string FormatCapability(Int64 nCapability, int nDigits = 2)
        {
            string szCapability = "";
            if (nCapability < 1024)
            {
                szCapability = nCapability + " B";
            }
            else if (nCapability < 1024 * 1024)
            {
                szCapability = (nCapability / 1024.0).ToString("F" + nDigits) + " KB";
            }
            else if (nCapability < 1024 * 1024 * 1024)
            {
                szCapability = (nCapability / 1024.0 / 1024.0).ToString("F" + nDigits) + " MB";
            }
            else
            {
                szCapability = (nCapability / 1024.0 / 1024.0 / 1024.0).ToString("F" + nDigits) + " GB";
            }

            return szCapability;
        }

        private void CopyLocalDirectory(string src_dir, string dst_dir)
        {
            try
            {
                if (!Directory.Exists(dst_dir))
                {
                    Directory.CreateDirectory(dst_dir);
                    File.SetAttributes(dst_dir, File.GetAttributes(src_dir));
                }

                foreach (string str_path in Directory.GetFileSystemEntries(src_dir))
                {
                    string dst_path = "\\" + Path.GetFileName(str_path);
                    if (File.GetAttributes(str_path).HasFlag(FileAttributes.Directory))
                    {
                        CopyLocalDirectory(str_path, dst_dir + dst_path);
                    }
                    else
                    {
                        File.Copy(str_path, dst_dir + dst_path, true);
                        File.SetAttributes(dst_dir + dst_path, File.GetAttributes(str_path));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public frmMain()
        {
            InitializeComponent();

            CheckForIllegalCrossThreadCalls = false;

            pbStatus.ControlAlign = ContentAlignment.MiddleRight;

            m_util.OnNewLog += new BaiduPCSUtil.OnNewLogDelegate(OnNewLog);
            m_util.OnReportProgress += new BaiduPCSUtil.OnReportProgressDelegate(OnReportProgress);
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            cmbRemoteThreads.SelectedIndex = 0;

            btnLocalRefresh_Click(null, null);
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            btnLogout_Click(null, null);
        }

        private void lvwBrowser_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            new ListViewSorter(sender as ListView, e.Column);
        }

        private void lblPause_Click(object sender, EventArgs e)
        {
            if (0 == m_util.Status)
            {
                m_util.Status = 1;
                lblPause.Text = "继续";
            }
            else if (1 == m_util.Status)
            {
                m_util.Status = 0;
                lblPause.Text = "暂停";
            }
        }

        private void lblStop_Click(object sender, EventArgs e)
        {
            m_util.Status = 2;
        }

        #endregion

        #region 登录

        private void picCaptcha_Click(object sender, EventArgs e)
        {
            Bitmap bmp_captcha = m_util.GetCaptcha();
            if (null == bmp_captcha ||
                "" != m_util.LastErrorStr)
            {
                MessageBox.Show("获取验证码失败！错误提示：\r\n\r\n" + m_util.LastErrorStr);
            }
            else
            {
                if (null != picCaptcha.Image)
                {
                    picCaptcha.Image.Dispose();
                }

                picCaptcha.Image = bmp_captcha;
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            int ret = m_util.Login(txtUsername.Text, txtPassword.Text, txtCaptcha.Text);
            if (0 == ret)
            {
                gbLogin.Enabled = false;
                gbRemote.Enabled = true;
                btnLogin.Enabled = false;
                btnLogout.Enabled = true;
                this.Text = "百度 PCS -- " + m_util.SysUID + " 已登录";

                btnLocalRefresh_Click(null, null);
                Application.DoEvents();

                btnRemoteRefresh_Click(null, null);
                Application.DoEvents();

                MessageBox.Show("登录成功！");
            }
            else if (100 == ret)
            {
                MessageBox.Show("需要填写验证码！");

                picCaptcha_Click(null, null);
            }
            else
            {
                MessageBox.Show("登录错误：" + ret);
            }
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            m_util.Status = 2;

            if (m_util.IsLogin())
            {
                if (m_util.Logout())
                {
                    //MessageBox.Show("登出成功！");

                    lvwRemote.Items.Clear();

                    gbLogin.Enabled = true;
                    gbRemote.Enabled = false;
                    btnLogin.Enabled = true;
                    btnLogout.Enabled = false;

                    this.Text = "百度 PCS";
                }
                else
                {
                    MessageBox.Show("登出错误：\r\n\r\n" + m_util.LastErrorStr);
                }
            }
        }

        #endregion

        #region 日志

        private long m_last_size = 0;
        private long m_last_delta_size = 0;
        private DateTime m_last_time = DateTime.Now;

        public void OnNewLog(string new_log)
        {
            if (lbLog.Items.Count > 100)
            {
                lbLog.Items.RemoveAt(lbLog.Items.Count - 1);
            }

            lbLog.Items.Insert(0, DateTime.Now.ToString("MM-dd HH:mm:ss ") + new_log);
            lbLog.TopIndex = 0;
        }

        public void OnReportProgress(BaiduPCSUtil.BaiduProgressInfo pi)
        {
            long delta_ticks = DateTime.Now.Ticks - m_last_time.Ticks;
            long rate = (delta_ticks < TimeSpan.TicksPerMillisecond ? 0 :
                m_last_delta_size * 1000 / (delta_ticks / TimeSpan.TicksPerMillisecond));
            double total_percent = (0 == pi.total_size ? 0.0 : Math.Round(pi.current_size * 100.0 / pi.total_size, 2));
            double current_percent = (0 == pi.total_bytes ? 0.0 : Math.Round(pi.current_bytes * 100.0 / pi.total_bytes, 2));
            string str_status =
                "当前文件进度：" +
                FormatCapability(pi.current_bytes) + "/" +
                FormatCapability(pi.total_bytes) +
                "(" + current_percent + " %)，" +
                "总进度：" +
                pi.current_files + " / " + pi.total_files + "，" +
                FormatCapability(pi.current_size) + "/" +
                FormatCapability(pi.total_size) +
                "(" + total_percent + " %)，" +
                FormatCapability(rate) + "/s";
            if (delta_ticks / TimeSpan.TicksPerSecond >= 1)
            {
                m_last_delta_size = pi.current_size - m_last_size;
                m_last_size = pi.current_size;
                m_last_time = DateTime.Now;
            }

            pbStatus.Value = (int)current_percent;
            pbStatus.Maximum = 100;

            lblStatus.Text = str_status;
            Application.DoEvents();
        }

        private void btnClearLog_Click(object sender, EventArgs e)
        {
            lbLog.Items.Clear();
        }

        private void lbLog_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            MessageBox.Show(lbLog.SelectedItem.ToString());
        }

        #endregion

        #region 本地

        private bool m_local_is_cut = false;
        private string m_local_current_path = "";

        private void lvwLocal_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (1 != lvwLocal.SelectedItems.Count) return;

            if (".." == lvwLocal.SelectedItems[0].Text)
            {
                // 已经在根目录
                if ("" == m_local_current_path ||
                    Directory.GetDirectoryRoot(m_local_current_path) == m_local_current_path)
                {
                    m_local_current_path = "";
                    btnLocalRefresh_Click(null, null);
                    return;
                }
            }

            string str_path = Path.GetFullPath(Path.Combine(m_local_current_path, lvwLocal.SelectedItems[0].Text));
            if (File.GetAttributes(str_path).HasFlag(FileAttributes.Directory))
            {
                m_local_current_path = str_path;
                btnLocalRefresh_Click(null, null);
            }
        }

        private void lvwLocal_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control)
            {
                if (Keys.X == e.KeyCode)
                {
                    btnLocalCut_Click(null, null);
                }
                else if (Keys.C == e.KeyCode)
                {
                    btnLocalCopy_Click(null, null);
                }
                else if (Keys.V == e.KeyCode)
                {
                    btnLocalPaste_Click(null, null);
                }
            }
            else if (Keys.F2 == e.KeyCode)
            {
                btnLocalRename_Click(null, null);
            }
            else if (Keys.Delete == e.KeyCode)
            {
                btnLocalDelete_Click(null, null);
            }
        }

        private void lvwLocal_AfterLabelEdit(object sender, LabelEditEventArgs e)
        {
            if (e.CancelEdit) return;
            if (null == e.Label ||
                "" == m_local_current_path ||
                1 != lvwLocal.SelectedItems.Count ||
                e.Item != lvwLocal.SelectedItems[0].Index ||
                e.Label == lvwLocal.SelectedItems[0].Text)
            {
                e.CancelEdit = true;
                return;
            }

            List<char> l = new List<char>(Path.GetInvalidFileNameChars());
            foreach (char c in e.Label)
            {
                if (l.Contains(c))
                {
                    e.CancelEdit = true;
                    return;
                }
            }

            try
            {
                ListViewItem lvi = lvwLocal.Items[e.Item];
                string src_path = m_local_current_path + "\\" + lvi.Text;
                string dst_path = m_local_current_path + "\\" + e.Label;
                if (File.GetAttributes(src_path).HasFlag(FileAttributes.Directory))
                {
                    Directory.Move(src_path, dst_path);
                }
                else
                {
                    File.Move(src_path, dst_path);
                }

                e.CancelEdit = true;
                lblStatus.Text = "所有操作已完成";
                btnLocalRefresh_Click(null, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void btnLocalRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                lvwLocal.BeginUpdate();
                lvwLocal.Items.Clear();
                if ("" == m_local_current_path)
                {
                    foreach (string str_drive in Directory.GetLogicalDrives())
                    {
                        lvwLocal.Items.Add(new ListViewItem(
                            new string[] {
                                str_drive, "", ""
                            }, "Drive"));
                    }
                }
                else if (Directory.Exists(m_local_current_path))
                {
                    lvwLocal.Items.Add(new ListViewItem(new string[] { "..", "", "" }));
                    foreach (string str_path in Directory.GetDirectories(m_local_current_path))
                    {
                        lvwLocal.Items.Add(new ListViewItem(
                            new string[] {
                                Path.GetFileName(str_path), "", ""
                            }, "Folder"));
                    }

                    foreach (string str_path in Directory.GetFiles(m_local_current_path))
                    {
                        FileInfo fi = new FileInfo(str_path);
                        lvwLocal.Items.Add(new ListViewItem(
                            new string[] {
                            fi.Name,
                            FormatCapability(fi.Length),
                            GetDatetime(fi.CreationTime)
                        }, "File"));
                    }
                }
                lvwLocal.EndUpdate();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());

                m_local_current_path = "";
                lvwLocal.EndUpdate();
            }
        }

        private void btnLocalCut_Click(object sender, EventArgs e)
        {
            try
            {
                if ("" == m_local_current_path) return;
                if (lvwLocal.SelectedItems.Count <= 0) return;

                StringCollection sc = new StringCollection();
                foreach (ListViewItem lvi in lvwLocal.SelectedItems)
                {
                    sc.Add(m_local_current_path + "\\" + lvi.Text);
                }

                Clipboard.Clear();
                Clipboard.SetFileDropList(sc);

                m_local_is_cut = true;
                lblStatus.Text = "已将 " + sc.Count + " 个文件(夹)放入剪贴板！";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void btnLocalCopy_Click(object sender, EventArgs e)
        {
            try
            {
                if ("" == m_local_current_path) return;
                if (lvwLocal.SelectedItems.Count <= 0) return;

                StringCollection sc = new StringCollection();
                foreach (ListViewItem lvi in lvwLocal.SelectedItems)
                {
                    sc.Add(m_local_current_path + "\\" + lvi.Text);
                }

                Clipboard.Clear();
                Clipboard.SetFileDropList(sc);

                m_local_is_cut = false;
                lblStatus.Text = "已将 " + sc.Count + " 个文件(夹)放入剪贴板！";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void btnLocalPaste_Click(object sender, EventArgs e)
        {
            try
            {
                if ("" == m_local_current_path) return;

                StringCollection sc = Clipboard.GetFileDropList();
                if (sc.Count <= 0) return;

                foreach (string str_path in sc)
                {
                    string dst_path = "\\" + Path.GetFileName(str_path);
                    if (File.GetAttributes(str_path).HasFlag(FileAttributes.Directory))
                    {
                        if (m_local_is_cut)
                        {
                            Directory.Move(str_path, m_local_current_path + dst_path);
                        }
                        else
                        {
                            CopyLocalDirectory(str_path, m_local_current_path + dst_path);
                        }
                    }
                    else
                    {
                        if (m_local_is_cut)
                        {
                            File.Move(str_path, m_local_current_path + dst_path);
                        }
                        else
                        {
                            File.Copy(str_path, m_local_current_path + dst_path, true);
                        }

                        File.SetAttributes(m_local_current_path + dst_path, File.GetAttributes(str_path));
                    }
                }

                lblStatus.Text = "所有操作已完成";
                btnLocalRefresh_Click(null, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void btnLocalRename_Click(object sender, EventArgs e)
        {
            try
            {
                if ("" == m_local_current_path) return;
                if (1 != lvwLocal.SelectedItems.Count) return;

                lvwLocal.SelectedItems[0].BeginEdit();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void btnLocalDelete_Click(object sender, EventArgs e)
        {
            if ("" == m_local_current_path) return;
            if (lvwLocal.SelectedItems.Count <= 0) return;
            if (lvwLocal.SelectedItems.Count > 1)
            {
                MessageBox.Show("一次只能删除一个文件(夹)！");
                return;
            }

            ListViewItem lvi = lvwLocal.SelectedItems[0];
            string dst_path = m_local_current_path + "\\" + lvi.Text;
            if (DialogResult.Yes != MessageBox.Show(
                "是否确认删除 " + dst_path + "？",
                "删除确认",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Stop,
                MessageBoxDefaultButton.Button2))
            {
                return;
            }

            try
            {
                if (File.GetAttributes(dst_path).HasFlag(FileAttributes.Directory))
                {
                    Directory.Delete(dst_path, true);
                }
                else
                {
                    File.Delete(dst_path);
                }

                lblStatus.Text = "所有操作已完成";
                btnLocalRefresh_Click(null, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void btnLocalMkdir_Click(object sender, EventArgs e)
        {
            try
            {
                if ("" == m_local_current_path) return;

                int i = 1;
                string str_new_dir = m_local_current_path + "\\新建文件夹";
                while (Directory.Exists(str_new_dir))
                {
                    str_new_dir = m_local_current_path + "\\新建文件夹(" + i++ + ")";
                }

                Directory.CreateDirectory(str_new_dir);
                ListViewItem lvi = new ListViewItem(new string[] {
                    Path.GetFileName(str_new_dir), "", ""
                }, "Folder");

                lvwLocal.Items.Add(lvi);
                lvi.BeginEdit();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void btnUpload_Click(object sender, EventArgs e)
        {
            if ("" == m_local_current_path) return;
            if (lvwLocal.SelectedItems.Count <= 0) return;

            if (!m_util.IsLogin())
            {
                return;
            }

            List<string> lst_path = new List<string>();
            foreach (ListViewItem lvi in lvwLocal.SelectedItems)
            {
                lst_path.Add(m_local_current_path + "\\" + lvi.Text);
            }

            gbLocal.Enabled = false;
            gbRemote.Enabled = false;
            pbStatus.Visible = true;
            lblPause.Visible = true;
            lblStop.Visible = true;

            m_last_size = 0;
            m_last_time = DateTime.Now;
            long ticks = DateTime.Now.Ticks;

            bool ret = m_util.Upload(m_local_current_path, lst_path.ToArray(), m_remote_current_path);
            if (!ret)
            {
                MessageBox.Show("上传文件失败：" + m_util.LastErrorStr);
            }

            pbStatus.Visible = false;
            lblPause.Visible = false;
            lblStop.Visible = false;
            gbLocal.Enabled = true;
            gbRemote.Enabled = true;

            btnRemoteRefresh_Click(null, null);
            lblStatus.Text = "所有操作已完成，耗时 " +
                (DateTime.Now.Ticks - ticks) / TimeSpan.TicksPerMillisecond +
                " 毫秒！";
        }

        #endregion

        #region 远程

        private bool m_remote_is_cut = false;
        private string m_remote_current_path = "/";

        private void lvwRemote_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (1 != lvwRemote.SelectedItems.Count) return;

            ListViewItem lvi = lvwRemote.SelectedItems[0];
            if (".." == lvi.Text)
            {
                if ("/" != m_remote_current_path)
                {
                    string[] arr_path = m_remote_current_path.Split("/".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    m_remote_current_path = "/" + string.Join("/", arr_path, 0, arr_path.Length - 1);
                }

                btnRemoteRefresh_Click(null, null);
                return;
            }

            BaiduPCSUtil.BaiduFileInfo bdfi = lvi.Tag as BaiduPCSUtil.BaiduFileInfo;
            if (null == bdfi)
            {
                return;
            }

            if (0 == bdfi.m_is_dir)
            {
                return;
            }

            m_remote_current_path = bdfi.m_path;
            btnRemoteRefresh_Click(null, null);
        }

        private void lvwRemote_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control)
            {
                if (Keys.X == e.KeyCode)
                {
                    //btnRemoteCut_Click(null, null);
                }
                else if (Keys.C == e.KeyCode)
                {
                    //btnRemoteCopy_Click(null, null);
                }
                else if (Keys.V == e.KeyCode)
                {
                    //btnRemotePaste_Click(null, null);
                }
            }
            else if (Keys.F2 == e.KeyCode)
            {
                btnRemoteRename_Click(null, null);
            }
            else if (Keys.Delete == e.KeyCode)
            {
                btnRemoteDelete_Click(null, null);
            }
        }

        private void lvwRemote_AfterLabelEdit(object sender, LabelEditEventArgs e)
        {
            if (e.CancelEdit) return;
            if (null == e.Label ||
                1 != lvwRemote.SelectedItems.Count ||
                e.Item != lvwRemote.SelectedItems[0].Index ||
                e.Label == lvwRemote.SelectedItems[0].Text)
            {
                e.CancelEdit = true;
                return;
            }

            List<char> l = new List<char>(Path.GetInvalidFileNameChars());
            foreach (char c in e.Label)
            {
                if (l.Contains(c))
                {
                    e.CancelEdit = true;
                    return;
                }
            }

            ListViewItem lvi = lvwRemote.SelectedItems[0];
            BaiduPCSUtil.BaiduFileInfo bdfi = lvi.Tag as BaiduPCSUtil.BaiduFileInfo;
            if (null == bdfi)
            {
                e.CancelEdit = true;
                return;
            }

            if (!m_util.Rename(bdfi.m_path, e.Label))
            {
                MessageBox.Show("重命名发生错误：" + m_util.LastErrorStr);
            }

            e.CancelEdit = true;
            lblStatus.Text = "所有操作已完成";
            btnRemoteRefresh_Click(null, null);
        }

        private void btnRemoteRefresh_Click(object sender, EventArgs e)
        {
            if (!m_util.IsLogin())
            {
                return;
            }

            List<BaiduPCSUtil.BaiduFileInfo> lst_bdfi = new List<BaiduPCSUtil.BaiduFileInfo>();
            m_util.List(ref lst_bdfi, m_remote_current_path);

            gbRemote.Enabled = false;

            lvwRemote.BeginUpdate();
            lvwRemote.Items.Clear();
            if ("/" != m_remote_current_path)
            {
                lvwRemote.Items.Add(new ListViewItem(new string[] { "..", "", "" }));
            }

            int dir_num = 0, file_num = 0;
            foreach (BaiduPCSUtil.BaiduFileInfo bdfi in lst_bdfi)
            {
                ListViewItem lvi = new ListViewItem(bdfi.m_server_filename);
                lvi.Tag = bdfi;

                if (1 == bdfi.m_is_dir)
                {
                    dir_num++;
                    lvi.ImageKey = "Folder";
                    lvi.SubItems.Add("");
                    lvi.SubItems.Add(BaiduPCSUtil.FromUnixtime(bdfi.m_server_ctime));
                    lvi.SubItems.Add("");
                }
                else
                {
                    file_num++;
                    lvi.ImageKey = "File";
                    lvi.SubItems.Add(FormatCapability(bdfi.m_size));
                    lvi.SubItems.Add(BaiduPCSUtil.FromUnixtime(bdfi.m_server_ctime));
                    lvi.SubItems.Add(bdfi.m_md5);
                }

                lvwRemote.Items.Add(lvi);
            }

            lvwRemote.EndUpdate();
            gbRemote.Enabled = true;

            lblStatus.Text = "共计 " + dir_num + " 个文件夹，" + file_num + " 个文件！";
        }

        private void btnRemoteCut_Click(object sender, EventArgs e)
        {
            if (!m_util.IsLogin())
            {
                return;
            }


        }

        private void btnRemoteCopy_Click(object sender, EventArgs e)
        {
            if (!m_util.IsLogin())
            {
                return;
            }


        }

        private void btnRemotePaste_Click(object sender, EventArgs e)
        {
            if (!m_util.IsLogin())
            {
                return;
            }


        }

        private void btnRemoteRename_Click(object sender, EventArgs e)
        {
            if ("" == m_remote_current_path) return;
            if (lvwRemote.SelectedItems.Count <= 0) return;
            if (lvwRemote.SelectedItems.Count > 1)
            {
                MessageBox.Show("一次只能重命名一个文件(夹)！");
                return;
            }

            if (!m_util.IsLogin())
            {
                return;
            }

            lvwRemote.SelectedItems[0].BeginEdit();
        }

        private void btnRemoteDelete_Click(object sender, EventArgs e)
        {
            if ("" == m_remote_current_path) return;
            if (lvwRemote.SelectedItems.Count <= 0) return;
            if (lvwRemote.SelectedItems.Count > 1)
            {
                MessageBox.Show("一次只能删除一个文件(夹)！");
                return;
            }

            BaiduPCSUtil.BaiduFileInfo bdfi = lvwRemote.SelectedItems[0].Tag as BaiduPCSUtil.BaiduFileInfo;
            if (null == bdfi)
            {
                return;
            }

            if (DialogResult.Yes != MessageBox.Show(
                "是否确认删除 " + bdfi.m_path + "？",
                "删除确认",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Stop,
                MessageBoxDefaultButton.Button2))
            {
                return;
            }

            if (!m_util.IsLogin())
            {
                return;
            }

            if (!m_util.Delete(bdfi.m_path))
            {
                MessageBox.Show("删除文件失败：" + m_util.LastErrorStr);
            }

            lblStatus.Text = "所有操作已完成";
            btnRemoteRefresh_Click(null, null);
        }

        private void btnRemoteMkdir_Click(object sender, EventArgs e)
        {
            string str_dir = Interaction.InputBox("请输入新文件夹的名称", "新文件夹名称", "新建文件夹");
            if ("" == str_dir)
            {
                return;
            }

            List<char> l = new List<char>(Path.GetInvalidFileNameChars());
            foreach (char c in str_dir)
            {
                if (l.Contains(c))
                {
                    MessageBox.Show("新文件夹包含非法字符！");
                    return;
                }
            }

            if (!m_util.IsLogin())
            {
                return;
            }

            BaiduPCSUtil.BaiduFileInfo bdfi = new BaiduPCSUtil.BaiduFileInfo();
            bool ret = m_util.MkDir(m_remote_current_path + "/" + str_dir, ref bdfi);
            if (!ret)
            {
                MessageBox.Show("创建文件夹失败：" + m_util.LastErrorStr);
            }

            lblStatus.Text = "所有操作已完成";
            btnRemoteRefresh_Click(null, null);
        }

        private void btnDownload_Click(object sender, EventArgs e)
        {
            if ("" == m_local_current_path) return;
            if (lvwRemote.SelectedItems.Count <= 0) return;

            if (!m_util.IsLogin())
            {
                return;
            }

            List<BaiduPCSUtil.BaiduFileInfo> lst_path = new List<BaiduPCSUtil.BaiduFileInfo>();
            foreach (ListViewItem lvi in lvwRemote.SelectedItems)
            {
                BaiduPCSUtil.BaiduFileInfo bdfi = lvi.Tag as BaiduPCSUtil.BaiduFileInfo;
                if (null == bdfi)
                {
                    continue;
                }

                lst_path.Add(bdfi);
            }

            int threads_max = 0;
            switch (cmbRemoteThreads.SelectedIndex)
            {
                case 1:
                    threads_max = 1;
                    break;

                case 2:
                    threads_max = 10;
                    break;

                default:
                    break;
            }

            gbLocal.Enabled = false;
            gbRemote.Enabled = false;
            pbStatus.Visible = true;
            lblPause.Visible = true;
            lblStop.Visible = true;

            m_last_size = 0;
            m_last_time = DateTime.Now;
            long ticks = DateTime.Now.Ticks;

            bool ret = m_util.Download(m_remote_current_path, lst_path, m_local_current_path, threads_max);
            if (!ret)
            {
                MessageBox.Show("下载文件失败：" + m_util.LastErrorStr);
            }

            pbStatus.Visible = false;
            lblPause.Visible = false;
            lblStop.Visible = false;
            gbLocal.Enabled = true;
            gbRemote.Enabled = true;

            btnLocalRefresh_Click(null, null);
            lblStatus.Text = "所有操作已完成，耗时 " +
                (DateTime.Now.Ticks - ticks) / TimeSpan.TicksPerMillisecond +
                " 毫秒！";
        }

        #endregion
    }
}
