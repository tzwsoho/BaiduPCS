namespace BaiduPCS
{
    partial class frmMain
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.gbLogin = new System.Windows.Forms.GroupBox();
            this.picCaptcha = new System.Windows.Forms.PictureBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtCaptcha = new System.Windows.Forms.TextBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.btnLogout = new System.Windows.Forms.Button();
            this.btnLogin = new System.Windows.Forms.Button();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.gbLog = new System.Windows.Forms.GroupBox();
            this.lbLog = new System.Windows.Forms.ListBox();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.gbLocal = new System.Windows.Forms.GroupBox();
            this.lvwLocal = new System.Windows.Forms.ListView();
            this.colLclName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colLclSize = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colLclDatetime = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.imgIcon = new System.Windows.Forms.ImageList(this.components);
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnLocalRefresh = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnLocalCut = new System.Windows.Forms.ToolStripButton();
            this.btnLocalCopy = new System.Windows.Forms.ToolStripButton();
            this.btnLocalPaste = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.btnLocalRename = new System.Windows.Forms.ToolStripButton();
            this.btnLocalDelete = new System.Windows.Forms.ToolStripButton();
            this.btnLocalMkdir = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.btnUpload = new System.Windows.Forms.ToolStripButton();
            this.gbRemote = new System.Windows.Forms.GroupBox();
            this.lvwRemote = new System.Windows.Forms.ListView();
            this.colRmtName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colRmtSize = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colRmtDatetime = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colRmtMD5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.toolStrip2 = new System.Windows.Forms.ToolStrip();
            this.btnRemoteRefresh = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.btnRemoteCut = new System.Windows.Forms.ToolStripButton();
            this.btnRemoteCopy = new System.Windows.Forms.ToolStripButton();
            this.btnRemotePaste = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.btnRemoteRename = new System.Windows.Forms.ToolStripButton();
            this.btnRemoteDelete = new System.Windows.Forms.ToolStripButton();
            this.btnRemoteMkdir = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.btnDownload = new System.Windows.Forms.ToolStripButton();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblPause = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblStop = new System.Windows.Forms.ToolStripStatusLabel();
            this.pbStatus = new System.Windows.Forms.ToolStripProgressBar();
            this.btnClearLog = new System.Windows.Forms.Button();
            this.gbLogin.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picCaptcha)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.gbLog.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.gbLocal.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.gbRemote.SuspendLayout();
            this.toolStrip2.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbLogin
            // 
            this.gbLogin.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbLogin.Controls.Add(this.picCaptcha);
            this.gbLogin.Controls.Add(this.label3);
            this.gbLogin.Controls.Add(this.txtCaptcha);
            this.gbLogin.Controls.Add(this.txtPassword);
            this.gbLogin.Controls.Add(this.label2);
            this.gbLogin.Controls.Add(this.label1);
            this.gbLogin.Controls.Add(this.txtUsername);
            this.gbLogin.Location = new System.Drawing.Point(12, 12);
            this.gbLogin.Name = "gbLogin";
            this.gbLogin.Size = new System.Drawing.Size(746, 51);
            this.gbLogin.TabIndex = 0;
            this.gbLogin.TabStop = false;
            this.gbLogin.Text = "登录网盘";
            // 
            // picCaptcha
            // 
            this.picCaptcha.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picCaptcha.Location = new System.Drawing.Point(640, 12);
            this.picCaptcha.Name = "picCaptcha";
            this.picCaptcha.Size = new System.Drawing.Size(100, 33);
            this.picCaptcha.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picCaptcha.TabIndex = 7;
            this.picCaptcha.TabStop = false;
            this.picCaptcha.Click += new System.EventHandler(this.picCaptcha_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(487, 25);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "验证码";
            // 
            // txtCaptcha
            // 
            this.txtCaptcha.Location = new System.Drawing.Point(534, 20);
            this.txtCaptcha.Name = "txtCaptcha";
            this.txtCaptcha.Size = new System.Drawing.Size(100, 21);
            this.txtCaptcha.TabIndex = 5;
            this.txtCaptcha.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(281, 20);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(200, 21);
            this.txtPassword.TabIndex = 3;
            this.txtPassword.Text = "tzw19860806";
            this.txtPassword.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtPassword.UseSystemPasswordChar = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(246, 25);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "密码";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "账号";
            // 
            // txtUsername
            // 
            this.txtUsername.Location = new System.Drawing.Point(41, 20);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(199, 21);
            this.txtUsername.TabIndex = 1;
            this.txtUsername.Text = "tzwsoho";
            this.txtUsername.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // btnLogout
            // 
            this.btnLogout.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLogout.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnLogout.Enabled = false;
            this.btnLogout.Location = new System.Drawing.Point(764, 40);
            this.btnLogout.Name = "btnLogout";
            this.btnLogout.Size = new System.Drawing.Size(75, 23);
            this.btnLogout.TabIndex = 7;
            this.btnLogout.Text = "登出";
            this.btnLogout.UseVisualStyleBackColor = true;
            this.btnLogout.Click += new System.EventHandler(this.btnLogout_Click);
            // 
            // btnLogin
            // 
            this.btnLogin.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLogin.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnLogin.Location = new System.Drawing.Point(764, 11);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(75, 23);
            this.btnLogin.TabIndex = 6;
            this.btnLogin.Text = "登录";
            this.btnLogin.UseVisualStyleBackColor = true;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point(12, 69);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.gbLog);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(830, 443);
            this.splitContainer1.SplitterDistance = 90;
            this.splitContainer1.TabIndex = 1;
            // 
            // gbLog
            // 
            this.gbLog.Controls.Add(this.btnClearLog);
            this.gbLog.Controls.Add(this.lbLog);
            this.gbLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbLog.Location = new System.Drawing.Point(0, 0);
            this.gbLog.Name = "gbLog";
            this.gbLog.Size = new System.Drawing.Size(830, 90);
            this.gbLog.TabIndex = 0;
            this.gbLog.TabStop = false;
            this.gbLog.Text = "通讯日志";
            // 
            // lbLog
            // 
            this.lbLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbLog.FormattingEnabled = true;
            this.lbLog.IntegralHeight = false;
            this.lbLog.ItemHeight = 12;
            this.lbLog.Location = new System.Drawing.Point(3, 17);
            this.lbLog.Name = "lbLog";
            this.lbLog.Size = new System.Drawing.Size(824, 70);
            this.lbLog.TabIndex = 0;
            this.lbLog.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lbLog_MouseDoubleClick);
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.gbLocal);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.gbRemote);
            this.splitContainer2.Size = new System.Drawing.Size(830, 349);
            this.splitContainer2.SplitterDistance = 410;
            this.splitContainer2.TabIndex = 0;
            // 
            // gbLocal
            // 
            this.gbLocal.Controls.Add(this.lvwLocal);
            this.gbLocal.Controls.Add(this.toolStrip1);
            this.gbLocal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbLocal.Location = new System.Drawing.Point(0, 0);
            this.gbLocal.Name = "gbLocal";
            this.gbLocal.Size = new System.Drawing.Size(410, 349);
            this.gbLocal.TabIndex = 0;
            this.gbLocal.TabStop = false;
            this.gbLocal.Text = "本地";
            // 
            // lvwLocal
            // 
            this.lvwLocal.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colLclName,
            this.colLclSize,
            this.colLclDatetime});
            this.lvwLocal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvwLocal.FullRowSelect = true;
            this.lvwLocal.GridLines = true;
            this.lvwLocal.HideSelection = false;
            this.lvwLocal.LabelEdit = true;
            this.lvwLocal.LargeImageList = this.imgIcon;
            this.lvwLocal.Location = new System.Drawing.Point(3, 42);
            this.lvwLocal.Name = "lvwLocal";
            this.lvwLocal.ShowItemToolTips = true;
            this.lvwLocal.Size = new System.Drawing.Size(404, 304);
            this.lvwLocal.SmallImageList = this.imgIcon;
            this.lvwLocal.TabIndex = 0;
            this.lvwLocal.UseCompatibleStateImageBehavior = false;
            this.lvwLocal.View = System.Windows.Forms.View.Details;
            this.lvwLocal.AfterLabelEdit += new System.Windows.Forms.LabelEditEventHandler(this.lvwLocal_AfterLabelEdit);
            this.lvwLocal.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvwBrowser_ColumnClick);
            this.lvwLocal.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lvwLocal_KeyDown);
            this.lvwLocal.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lvwLocal_MouseDoubleClick);
            // 
            // colLclName
            // 
            this.colLclName.Text = "名称";
            this.colLclName.Width = 300;
            // 
            // colLclSize
            // 
            this.colLclSize.Text = "大小";
            this.colLclSize.Width = 80;
            // 
            // colLclDatetime
            // 
            this.colLclDatetime.Text = "创建日期";
            this.colLclDatetime.Width = 130;
            // 
            // imgIcon
            // 
            this.imgIcon.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgIcon.ImageStream")));
            this.imgIcon.TransparentColor = System.Drawing.Color.Transparent;
            this.imgIcon.Images.SetKeyName(0, "Drive");
            this.imgIcon.Images.SetKeyName(1, "File");
            this.imgIcon.Images.SetKeyName(2, "Folder");
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnLocalRefresh,
            this.toolStripSeparator1,
            this.btnLocalCut,
            this.btnLocalCopy,
            this.btnLocalPaste,
            this.toolStripSeparator2,
            this.btnLocalRename,
            this.btnLocalDelete,
            this.btnLocalMkdir,
            this.toolStripSeparator5,
            this.btnUpload});
            this.toolStrip1.Location = new System.Drawing.Point(3, 17);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(404, 25);
            this.toolStrip1.TabIndex = 1;
            // 
            // btnLocalRefresh
            // 
            this.btnLocalRefresh.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnLocalRefresh.Image = ((System.Drawing.Image)(resources.GetObject("btnLocalRefresh.Image")));
            this.btnLocalRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnLocalRefresh.Name = "btnLocalRefresh";
            this.btnLocalRefresh.Size = new System.Drawing.Size(23, 22);
            this.btnLocalRefresh.Text = "刷新";
            this.btnLocalRefresh.Click += new System.EventHandler(this.btnLocalRefresh_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // btnLocalCut
            // 
            this.btnLocalCut.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnLocalCut.Image = ((System.Drawing.Image)(resources.GetObject("btnLocalCut.Image")));
            this.btnLocalCut.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnLocalCut.Name = "btnLocalCut";
            this.btnLocalCut.Size = new System.Drawing.Size(23, 22);
            this.btnLocalCut.Text = "剪切";
            this.btnLocalCut.Click += new System.EventHandler(this.btnLocalCut_Click);
            // 
            // btnLocalCopy
            // 
            this.btnLocalCopy.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnLocalCopy.Image = ((System.Drawing.Image)(resources.GetObject("btnLocalCopy.Image")));
            this.btnLocalCopy.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnLocalCopy.Name = "btnLocalCopy";
            this.btnLocalCopy.Size = new System.Drawing.Size(23, 22);
            this.btnLocalCopy.Text = "复制";
            this.btnLocalCopy.Click += new System.EventHandler(this.btnLocalCopy_Click);
            // 
            // btnLocalPaste
            // 
            this.btnLocalPaste.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnLocalPaste.Image = ((System.Drawing.Image)(resources.GetObject("btnLocalPaste.Image")));
            this.btnLocalPaste.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnLocalPaste.Name = "btnLocalPaste";
            this.btnLocalPaste.Size = new System.Drawing.Size(23, 22);
            this.btnLocalPaste.Text = "粘贴";
            this.btnLocalPaste.Click += new System.EventHandler(this.btnLocalPaste_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // btnLocalRename
            // 
            this.btnLocalRename.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnLocalRename.Image = ((System.Drawing.Image)(resources.GetObject("btnLocalRename.Image")));
            this.btnLocalRename.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnLocalRename.Name = "btnLocalRename";
            this.btnLocalRename.Size = new System.Drawing.Size(23, 22);
            this.btnLocalRename.Text = "重命名";
            this.btnLocalRename.Click += new System.EventHandler(this.btnLocalRename_Click);
            // 
            // btnLocalDelete
            // 
            this.btnLocalDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnLocalDelete.Image = ((System.Drawing.Image)(resources.GetObject("btnLocalDelete.Image")));
            this.btnLocalDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnLocalDelete.Name = "btnLocalDelete";
            this.btnLocalDelete.Size = new System.Drawing.Size(23, 22);
            this.btnLocalDelete.Text = "删除";
            this.btnLocalDelete.Click += new System.EventHandler(this.btnLocalDelete_Click);
            // 
            // btnLocalMkdir
            // 
            this.btnLocalMkdir.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnLocalMkdir.Image = global::BaiduPCS.Properties.Resources.MkDir;
            this.btnLocalMkdir.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnLocalMkdir.Name = "btnLocalMkdir";
            this.btnLocalMkdir.Size = new System.Drawing.Size(23, 22);
            this.btnLocalMkdir.Text = "新建文件夹";
            this.btnLocalMkdir.Click += new System.EventHandler(this.btnLocalMkdir_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(6, 25);
            // 
            // btnUpload
            // 
            this.btnUpload.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnUpload.Image = global::BaiduPCS.Properties.Resources.Upload;
            this.btnUpload.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnUpload.Name = "btnUpload";
            this.btnUpload.Size = new System.Drawing.Size(23, 22);
            this.btnUpload.Text = "上传";
            this.btnUpload.ToolTipText = "上传文件/文件夹到网盘";
            this.btnUpload.Click += new System.EventHandler(this.btnUpload_Click);
            // 
            // gbRemote
            // 
            this.gbRemote.Controls.Add(this.lvwRemote);
            this.gbRemote.Controls.Add(this.toolStrip2);
            this.gbRemote.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbRemote.Enabled = false;
            this.gbRemote.Location = new System.Drawing.Point(0, 0);
            this.gbRemote.Name = "gbRemote";
            this.gbRemote.Size = new System.Drawing.Size(416, 349);
            this.gbRemote.TabIndex = 0;
            this.gbRemote.TabStop = false;
            this.gbRemote.Text = "网盘";
            // 
            // lvwRemote
            // 
            this.lvwRemote.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colRmtName,
            this.colRmtSize,
            this.colRmtDatetime,
            this.colRmtMD5});
            this.lvwRemote.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvwRemote.FullRowSelect = true;
            this.lvwRemote.GridLines = true;
            this.lvwRemote.HideSelection = false;
            this.lvwRemote.LabelEdit = true;
            this.lvwRemote.LargeImageList = this.imgIcon;
            this.lvwRemote.Location = new System.Drawing.Point(3, 42);
            this.lvwRemote.Name = "lvwRemote";
            this.lvwRemote.ShowItemToolTips = true;
            this.lvwRemote.Size = new System.Drawing.Size(410, 304);
            this.lvwRemote.SmallImageList = this.imgIcon;
            this.lvwRemote.TabIndex = 0;
            this.lvwRemote.UseCompatibleStateImageBehavior = false;
            this.lvwRemote.View = System.Windows.Forms.View.Details;
            this.lvwRemote.AfterLabelEdit += new System.Windows.Forms.LabelEditEventHandler(this.lvwRemote_AfterLabelEdit);
            this.lvwRemote.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvwBrowser_ColumnClick);
            this.lvwRemote.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lvwRemote_KeyDown);
            this.lvwRemote.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lvwRemote_MouseDoubleClick);
            // 
            // colRmtName
            // 
            this.colRmtName.Text = "名称";
            this.colRmtName.Width = 300;
            // 
            // colRmtSize
            // 
            this.colRmtSize.Text = "大小";
            this.colRmtSize.Width = 80;
            // 
            // colRmtDatetime
            // 
            this.colRmtDatetime.Text = "创建日期";
            this.colRmtDatetime.Width = 130;
            // 
            // colRmtMD5
            // 
            this.colRmtMD5.Text = "MD5";
            this.colRmtMD5.Width = 210;
            // 
            // toolStrip2
            // 
            this.toolStrip2.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnRemoteRefresh,
            this.toolStripSeparator3,
            this.btnRemoteCut,
            this.btnRemoteCopy,
            this.btnRemotePaste,
            this.toolStripSeparator4,
            this.btnRemoteRename,
            this.btnRemoteDelete,
            this.btnRemoteMkdir,
            this.toolStripSeparator6,
            this.btnDownload});
            this.toolStrip2.Location = new System.Drawing.Point(3, 17);
            this.toolStrip2.Name = "toolStrip2";
            this.toolStrip2.Size = new System.Drawing.Size(410, 25);
            this.toolStrip2.TabIndex = 2;
            // 
            // btnRemoteRefresh
            // 
            this.btnRemoteRefresh.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnRemoteRefresh.Image = ((System.Drawing.Image)(resources.GetObject("btnRemoteRefresh.Image")));
            this.btnRemoteRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnRemoteRefresh.Name = "btnRemoteRefresh";
            this.btnRemoteRefresh.Size = new System.Drawing.Size(23, 22);
            this.btnRemoteRefresh.Text = "刷新";
            this.btnRemoteRefresh.Click += new System.EventHandler(this.btnRemoteRefresh_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // btnRemoteCut
            // 
            this.btnRemoteCut.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnRemoteCut.Enabled = false;
            this.btnRemoteCut.Image = ((System.Drawing.Image)(resources.GetObject("btnRemoteCut.Image")));
            this.btnRemoteCut.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnRemoteCut.Name = "btnRemoteCut";
            this.btnRemoteCut.Size = new System.Drawing.Size(23, 22);
            this.btnRemoteCut.Text = "剪切";
            this.btnRemoteCut.Click += new System.EventHandler(this.btnRemoteCut_Click);
            // 
            // btnRemoteCopy
            // 
            this.btnRemoteCopy.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnRemoteCopy.Enabled = false;
            this.btnRemoteCopy.Image = ((System.Drawing.Image)(resources.GetObject("btnRemoteCopy.Image")));
            this.btnRemoteCopy.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnRemoteCopy.Name = "btnRemoteCopy";
            this.btnRemoteCopy.Size = new System.Drawing.Size(23, 22);
            this.btnRemoteCopy.Text = "复制";
            this.btnRemoteCopy.Click += new System.EventHandler(this.btnRemoteCopy_Click);
            // 
            // btnRemotePaste
            // 
            this.btnRemotePaste.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnRemotePaste.Enabled = false;
            this.btnRemotePaste.Image = ((System.Drawing.Image)(resources.GetObject("btnRemotePaste.Image")));
            this.btnRemotePaste.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnRemotePaste.Name = "btnRemotePaste";
            this.btnRemotePaste.Size = new System.Drawing.Size(23, 22);
            this.btnRemotePaste.Text = "粘贴";
            this.btnRemotePaste.Click += new System.EventHandler(this.btnRemotePaste_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
            // 
            // btnRemoteRename
            // 
            this.btnRemoteRename.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnRemoteRename.Image = ((System.Drawing.Image)(resources.GetObject("btnRemoteRename.Image")));
            this.btnRemoteRename.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnRemoteRename.Name = "btnRemoteRename";
            this.btnRemoteRename.Size = new System.Drawing.Size(23, 22);
            this.btnRemoteRename.Text = "重命名";
            this.btnRemoteRename.Click += new System.EventHandler(this.btnRemoteRename_Click);
            // 
            // btnRemoteDelete
            // 
            this.btnRemoteDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnRemoteDelete.Image = ((System.Drawing.Image)(resources.GetObject("btnRemoteDelete.Image")));
            this.btnRemoteDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnRemoteDelete.Name = "btnRemoteDelete";
            this.btnRemoteDelete.Size = new System.Drawing.Size(23, 22);
            this.btnRemoteDelete.Text = "删除";
            this.btnRemoteDelete.Click += new System.EventHandler(this.btnRemoteDelete_Click);
            // 
            // btnRemoteMkdir
            // 
            this.btnRemoteMkdir.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnRemoteMkdir.Image = global::BaiduPCS.Properties.Resources.MkDir;
            this.btnRemoteMkdir.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnRemoteMkdir.Name = "btnRemoteMkdir";
            this.btnRemoteMkdir.Size = new System.Drawing.Size(23, 22);
            this.btnRemoteMkdir.Text = "新建文件夹";
            this.btnRemoteMkdir.Click += new System.EventHandler(this.btnRemoteMkdir_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(6, 25);
            // 
            // btnDownload
            // 
            this.btnDownload.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnDownload.Image = global::BaiduPCS.Properties.Resources.Download;
            this.btnDownload.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnDownload.Name = "btnDownload";
            this.btnDownload.Size = new System.Drawing.Size(23, 22);
            this.btnDownload.Text = "下载";
            this.btnDownload.ToolTipText = "下载文件/文件夹到本地";
            this.btnDownload.Click += new System.EventHandler(this.btnDownload_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblStatus,
            this.toolStripStatusLabel1,
            this.lblPause,
            this.lblStop,
            this.pbStatus});
            this.statusStrip1.Location = new System.Drawing.Point(0, 515);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(854, 22);
            this.statusStrip1.TabIndex = 2;
            // 
            // lblStatus
            // 
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(32, 17);
            this.lblStatus.Text = "就绪";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(807, 17);
            this.toolStripStatusLabel1.Spring = true;
            // 
            // lblPause
            // 
            this.lblPause.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.lblPause.Name = "lblPause";
            this.lblPause.Size = new System.Drawing.Size(36, 21);
            this.lblPause.Text = "暂停";
            this.lblPause.Visible = false;
            this.lblPause.Click += new System.EventHandler(this.lblPause_Click);
            // 
            // lblStop
            // 
            this.lblStop.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.lblStop.Name = "lblStop";
            this.lblStop.Size = new System.Drawing.Size(36, 21);
            this.lblStop.Text = "停止";
            this.lblStop.Visible = false;
            this.lblStop.Click += new System.EventHandler(this.lblStop_Click);
            // 
            // pbStatus
            // 
            this.pbStatus.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.pbStatus.Name = "pbStatus";
            this.pbStatus.Size = new System.Drawing.Size(100, 20);
            this.pbStatus.Step = 1;
            this.pbStatus.Visible = false;
            // 
            // btnClearLog
            // 
            this.btnClearLog.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClearLog.Location = new System.Drawing.Point(784, 0);
            this.btnClearLog.Name = "btnClearLog";
            this.btnClearLog.Size = new System.Drawing.Size(40, 23);
            this.btnClearLog.TabIndex = 1;
            this.btnClearLog.Text = "清空";
            this.btnClearLog.UseVisualStyleBackColor = true;
            this.btnClearLog.Click += new System.EventHandler(this.btnClearLog_Click);
            // 
            // frmMain
            // 
            this.AcceptButton = this.btnLogin;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnLogout;
            this.ClientSize = new System.Drawing.Size(854, 537);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.btnLogout);
            this.Controls.Add(this.gbLogin);
            this.Controls.Add(this.btnLogin);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "百度 PCS";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.gbLogin.ResumeLayout(false);
            this.gbLogin.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picCaptcha)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.gbLog.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.gbLocal.ResumeLayout(false);
            this.gbLocal.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.gbRemote.ResumeLayout(false);
            this.gbRemote.PerformLayout();
            this.toolStrip2.ResumeLayout(false);
            this.toolStrip2.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox gbLogin;
        private System.Windows.Forms.Button btnLogout;
        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.GroupBox gbLog;
        private System.Windows.Forms.ListBox lbLog;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.GroupBox gbLocal;
        private System.Windows.Forms.ListView lvwLocal;
        private System.Windows.Forms.ColumnHeader colLclName;
        private System.Windows.Forms.ColumnHeader colLclSize;
        private System.Windows.Forms.ColumnHeader colLclDatetime;
        private System.Windows.Forms.GroupBox gbRemote;
        private System.Windows.Forms.ListView lvwRemote;
        private System.Windows.Forms.ColumnHeader colRmtName;
        private System.Windows.Forms.ColumnHeader colRmtSize;
        private System.Windows.Forms.ColumnHeader colRmtDatetime;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel lblStatus;
        private System.Windows.Forms.ToolStripProgressBar pbStatus;
        private System.Windows.Forms.PictureBox picCaptcha;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtCaptcha;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStrip toolStrip2;
        private System.Windows.Forms.ImageList imgIcon;
        private System.Windows.Forms.ToolStripButton btnLocalRefresh;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton btnLocalCut;
        private System.Windows.Forms.ToolStripButton btnLocalCopy;
        private System.Windows.Forms.ToolStripButton btnLocalPaste;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton btnLocalRename;
        private System.Windows.Forms.ToolStripButton btnLocalDelete;
        private System.Windows.Forms.ToolStripButton btnRemoteRefresh;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton btnRemoteCut;
        private System.Windows.Forms.ToolStripButton btnRemoteCopy;
        private System.Windows.Forms.ToolStripButton btnRemotePaste;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripButton btnRemoteRename;
        private System.Windows.Forms.ToolStripButton btnRemoteDelete;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripButton btnUpload;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripButton btnDownload;
        private System.Windows.Forms.ToolStripButton btnLocalMkdir;
        private System.Windows.Forms.ToolStripButton btnRemoteMkdir;
        private System.Windows.Forms.ColumnHeader colRmtMD5;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel lblPause;
        private System.Windows.Forms.ToolStripStatusLabel lblStop;
        private System.Windows.Forms.Button btnClearLog;
    }
}

