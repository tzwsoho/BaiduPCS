using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace BaiduPCS
{
    public partial class frmMain : Form
    {
        BaiduPCSUtil m_util = new BaiduPCSUtil();

        public frmMain()
        {
            InitializeComponent();

            m_util.OnNewLog += new BaiduPCSUtil.OnNewLogDelegate(OnNewLog);
        }

        #region 登录

        private void picCaptcha_Click(object sender, EventArgs e)
        {
            Bitmap bmp_captcha = m_util.GetCaptcha();
            if (null == bmp_captcha ||
                null != m_util.LastErrorStr)
            {
                MessageBox.Show("获取验证码失败！错误提示：\r\n" + m_util.LastErrorStr);
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
                MessageBox.Show("登录成功！");

                gbLogin.Enabled = false;
                gbLocal.Enabled = true;
                gbRemote.Enabled = true;
                btnLogin.Enabled = false;
                btnLogout.Enabled = true;
            }
            else if (10 == ret)
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
            if (m_util.Logout() && !m_util.IsLogin())
            {
                MessageBox.Show("登出成功！");

                gbLogin.Enabled = true;
                gbLocal.Enabled = false;
                gbRemote.Enabled = false;
                btnLogin.Enabled = true;
                btnLogout.Enabled = false;
            }
            else
            {
                MessageBox.Show("登出错误：\r\n" + m_util.LastErrorStr);
            }
        }

        #endregion

        #region 日志

        public void OnNewLog(string new_log)
        {
            lbLog.Items.Insert(0, DateTime.Now.ToString("MM-dd HH:mm:ss ") + new_log);
            lbLog.TopIndex = 0;
        }

        private void lbLog_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            MessageBox.Show(lbLog.SelectedItem.ToString());
        }

        #endregion
    }
}
