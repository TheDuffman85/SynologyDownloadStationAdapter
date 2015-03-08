namespace TheDuffman85.SynologyDownloadStationAdapter
{
    partial class frmSettings
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSettings));
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.openDiskstationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addLinkToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addContainerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bookmarksToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.quitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.txtAddress = new System.Windows.Forms.TextBox();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.cbAutostart = new System.Windows.Forms.CheckBox();
            this.btnFileAssociation = new System.Windows.Forms.Button();
            this.cbApplicationEnabled = new System.Windows.Forms.CheckBox();
            this.txtApplicationUrl = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cbShowDecryptedLinks = new System.Windows.Forms.CheckBox();
            this.lblVersion = new System.Windows.Forms.LinkLabel();
            this.contextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // notifyIcon
            // 
            this.notifyIcon.ContextMenuStrip = this.contextMenuStrip;
            this.notifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon.Icon")));
            this.notifyIcon.Text = "Synology Download Station Adapter";
            this.notifyIcon.Visible = true;
            this.notifyIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon_MouseDoubleClick);
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openDiskstationToolStripMenuItem,
            this.addLinkToolStripMenuItem,
            this.addContainerToolStripMenuItem,
            this.bookmarksToolStripMenuItem,
            this.settingsToolStripMenuItem,
            this.quitToolStripMenuItem});
            this.contextMenuStrip.Name = "contextMenuStrip";
            this.contextMenuStrip.Size = new System.Drawing.Size(165, 158);
            this.contextMenuStrip.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip_Opening);
            // 
            // openDiskstationToolStripMenuItem
            // 
            this.openDiskstationToolStripMenuItem.Image = global::TheDuffman85.SynologyDownloadStationAdapter.Properties.Resources.world;
            this.openDiskstationToolStripMenuItem.Name = "openDiskstationToolStripMenuItem";
            this.openDiskstationToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.openDiskstationToolStripMenuItem.Text = "Open Diskstation";
            this.openDiskstationToolStripMenuItem.Click += new System.EventHandler(this.openDiskstationToolStripMenuItem_Click);
            // 
            // addLinkToolStripMenuItem
            // 
            this.addLinkToolStripMenuItem.Image = global::TheDuffman85.SynologyDownloadStationAdapter.Properties.Resources.textfield_add;
            this.addLinkToolStripMenuItem.Name = "addLinkToolStripMenuItem";
            this.addLinkToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.addLinkToolStripMenuItem.Text = "Add Links";
            this.addLinkToolStripMenuItem.Click += new System.EventHandler(this.addLinkToolStripMenuItem_Click);
            // 
            // addContainerToolStripMenuItem
            // 
            this.addContainerToolStripMenuItem.Image = global::TheDuffman85.SynologyDownloadStationAdapter.Properties.Resources.package_add;
            this.addContainerToolStripMenuItem.Name = "addContainerToolStripMenuItem";
            this.addContainerToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.addContainerToolStripMenuItem.Text = "Add Container";
            this.addContainerToolStripMenuItem.Click += new System.EventHandler(this.addContainerToolStripMenuItem_Click);
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.Image = global::TheDuffman85.SynologyDownloadStationAdapter.Properties.Resources.cog;
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.settingsToolStripMenuItem.Text = "Settings";
            this.settingsToolStripMenuItem.Click += new System.EventHandler(this.settingsToolStripMenuItem_Click);
            // 
            // bookmarksToolStripMenuItem
            // 
            this.bookmarksToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSeparator1,
            this.editToolStripMenuItem});
            this.bookmarksToolStripMenuItem.Image = global::TheDuffman85.SynologyDownloadStationAdapter.Properties.Resources.bookmark;
            this.bookmarksToolStripMenuItem.Name = "bookmarksToolStripMenuItem";
            this.bookmarksToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.bookmarksToolStripMenuItem.Text = "Bookmarks";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(149, 6);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.Image = global::TheDuffman85.SynologyDownloadStationAdapter.Properties.Resources.pencil;
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.editToolStripMenuItem.Text = "Edit";
            this.editToolStripMenuItem.Click += new System.EventHandler(this.editToolStripMenuItem_Click);
            // 
            // quitToolStripMenuItem
            // 
            this.quitToolStripMenuItem.Image = global::TheDuffman85.SynologyDownloadStationAdapter.Properties.Resources.cross;
            this.quitToolStripMenuItem.Name = "quitToolStripMenuItem";
            this.quitToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.quitToolStripMenuItem.Text = "Quit";
            this.quitToolStripMenuItem.Click += new System.EventHandler(this.quitToolStripMenuItem_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(167, 201);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 80;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(248, 201);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 90;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // txtAddress
            // 
            this.txtAddress.Location = new System.Drawing.Point(101, 12);
            this.txtAddress.Name = "txtAddress";
            this.txtAddress.Size = new System.Drawing.Size(222, 20);
            this.txtAddress.TabIndex = 10;
            // 
            // txtUsername
            // 
            this.txtUsername.Location = new System.Drawing.Point(101, 38);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(222, 20);
            this.txtUsername.TabIndex = 20;
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(101, 64);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(222, 20);
            this.txtPassword.TabIndex = 30;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(48, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Address:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(32, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "User:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 67);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(56, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Password:";
            // 
            // cbAutostart
            // 
            this.cbAutostart.AutoSize = true;
            this.cbAutostart.Location = new System.Drawing.Point(101, 139);
            this.cbAutostart.Name = "cbAutostart";
            this.cbAutostart.Size = new System.Drawing.Size(117, 17);
            this.cbAutostart.TabIndex = 70;
            this.cbAutostart.Text = "Start with Windows";
            this.cbAutostart.UseVisualStyleBackColor = true;
            // 
            // btnFileAssociation
            // 
            this.btnFileAssociation.Location = new System.Drawing.Point(101, 162);
            this.btnFileAssociation.Name = "btnFileAssociation";
            this.btnFileAssociation.Size = new System.Drawing.Size(222, 23);
            this.btnFileAssociation.TabIndex = 60;
            this.btnFileAssociation.Text = "Associate with ({0})";
            this.btnFileAssociation.UseVisualStyleBackColor = true;
            this.btnFileAssociation.Click += new System.EventHandler(this.btnFileAssociation_Click);
            // 
            // cbApplicationEnabled
            // 
            this.cbApplicationEnabled.AutoSize = true;
            this.cbApplicationEnabled.Location = new System.Drawing.Point(101, 90);
            this.cbApplicationEnabled.Name = "cbApplicationEnabled";
            this.cbApplicationEnabled.Size = new System.Drawing.Size(15, 14);
            this.cbApplicationEnabled.TabIndex = 40;
            this.cbApplicationEnabled.UseVisualStyleBackColor = true;
            this.cbApplicationEnabled.CheckedChanged += new System.EventHandler(this.cbApplicationUrl_CheckedChanged);
            // 
            // txtApplicationUrl
            // 
            this.txtApplicationUrl.Enabled = false;
            this.txtApplicationUrl.Location = new System.Drawing.Point(118, 90);
            this.txtApplicationUrl.Name = "txtApplicationUrl";
            this.txtApplicationUrl.Size = new System.Drawing.Size(205, 20);
            this.txtApplicationUrl.TabIndex = 50;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(13, 93);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(78, 13);
            this.label4.TabIndex = 14;
            this.label4.Text = "Application Url:";
            // 
            // cbShowDecryptedLinks
            // 
            this.cbShowDecryptedLinks.AutoSize = true;
            this.cbShowDecryptedLinks.Location = new System.Drawing.Point(101, 116);
            this.cbShowDecryptedLinks.Name = "cbShowDecryptedLinks";
            this.cbShowDecryptedLinks.Size = new System.Drawing.Size(195, 17);
            this.cbShowDecryptedLinks.TabIndex = 91;
            this.cbShowDecryptedLinks.Text = "Show decrypted links before adding";
            this.cbShowDecryptedLinks.UseVisualStyleBackColor = true;
            // 
            // lblVersion
            // 
            this.lblVersion.AutoSize = true;
            this.lblVersion.Location = new System.Drawing.Point(13, 211);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(59, 13);
            this.lblVersion.TabIndex = 92;
            this.lblVersion.TabStop = true;
            this.lblVersion.Text = "Version {0}";
            this.lblVersion.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lblVersion_LinkClicked);
            // 
            // frmSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(335, 237);
            this.Controls.Add(this.lblVersion);
            this.Controls.Add(this.cbShowDecryptedLinks);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtApplicationUrl);
            this.Controls.Add(this.cbApplicationEnabled);
            this.Controls.Add(this.btnFileAssociation);
            this.Controls.Add(this.cbAutostart);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.txtUsername);
            this.Controls.Add(this.txtAddress);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmSettings";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Settings";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmSettings_FormClosing);
            this.Load += new System.EventHandler(this.frmSettings_Load);
            this.VisibleChanged += new System.EventHandler(this.frmSettings_VisibleChanged);
            this.contextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NotifyIcon notifyIcon;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem quitToolStripMenuItem;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TextBox txtAddress;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ToolStripMenuItem openDiskstationToolStripMenuItem;
        private System.Windows.Forms.CheckBox cbAutostart;
        private System.Windows.Forms.Button btnFileAssociation;
        private System.Windows.Forms.ToolStripMenuItem addLinkToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addContainerToolStripMenuItem;
        private System.Windows.Forms.CheckBox cbApplicationEnabled;
        private System.Windows.Forms.TextBox txtApplicationUrl;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox cbShowDecryptedLinks;
        private System.Windows.Forms.LinkLabel lblVersion;
        private System.Windows.Forms.ToolStripMenuItem bookmarksToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
    }
}