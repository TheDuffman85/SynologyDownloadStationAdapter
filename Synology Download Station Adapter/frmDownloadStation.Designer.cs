namespace TheDuffman85.SynologyDownloadStationAdapter
{
    partial class frmDownloadStation
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmDownloadStation));
            this.webControl = new Awesomium.Windows.Forms.WebControl(this.components);
            this.pbLoadingIndicator = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pbLoadingIndicator)).BeginInit();
            this.SuspendLayout();
            // 
            // webControl
            // 
            this.webControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.webControl.Location = new System.Drawing.Point(0, -35);
            this.webControl.Size = new System.Drawing.Size(884, 696);
            this.webControl.Source = new System.Uri("about:blank", System.UriKind.Absolute);
            this.webControl.TabIndex = 0;
            this.webControl.ShowJavascriptDialog += new Awesomium.Core.JavascriptDialogEventHandler(this.Awesomium_Windows_Forms_WebControl_ShowJavascriptDialog);
            this.webControl.LoadingFrameFailed += new Awesomium.Core.LoadingFrameFailedEventHandler(this.Awesomium_Windows_Forms_WebControl_LoadingFrameFailed);
            this.webControl.CertificateError += new Awesomium.Core.CertificateErrorEventHandler(this.Awesomium_Windows_Forms_WebControl_CertificateError);
            this.webControl.Crashed += new Awesomium.Core.CrashedEventHandler(this.Awesomium_Windows_Forms_WebControl_Crashed);
            // 
            // pbLoadingIndicator
            // 
            this.pbLoadingIndicator.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbLoadingIndicator.ErrorImage = ((System.Drawing.Image)(resources.GetObject("pbLoadingIndicator.ErrorImage")));
            this.pbLoadingIndicator.Image = global::TheDuffman85.SynologyDownloadStationAdapter.Properties.Resources.indicator;
            this.pbLoadingIndicator.Location = new System.Drawing.Point(0, 0);
            this.pbLoadingIndicator.Name = "pbLoadingIndicator";
            this.pbLoadingIndicator.Size = new System.Drawing.Size(884, 661);
            this.pbLoadingIndicator.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pbLoadingIndicator.TabIndex = 1;
            this.pbLoadingIndicator.TabStop = false;
            // 
            // frmDownloadStation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(884, 661);
            this.Controls.Add(this.pbLoadingIndicator);
            this.Controls.Add(this.webControl);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmDownloadStation";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Download Station";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmDownloadStation_FormClosing);
            this.Load += new System.EventHandler(this.frmDownloadStation_Load);
            this.ResizeEnd += new System.EventHandler(this.frmDownloadStation_ResizeEnd);
            ((System.ComponentModel.ISupportInitialize)(this.pbLoadingIndicator)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Awesomium.Windows.Forms.WebControl webControl;
        private System.Windows.Forms.PictureBox pbLoadingIndicator;
        
    }
}