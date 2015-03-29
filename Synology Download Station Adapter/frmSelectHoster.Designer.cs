namespace TheDuffman85.SynologyDownloadStationAdapter
{
    partial class frmSelectHoster
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSelectHoster));
            this.clbHoster = new System.Windows.Forms.CheckedListBox();
            this.btnSelectAll = new System.Windows.Forms.Button();
            this.btnSelectNone = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.dsSelectHoster = new System.Data.DataSet();
            this.dtSelectHoster = new System.Data.DataTable();
            this.colHoster = new System.Data.DataColumn();
            this.colSelected = new System.Data.DataColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dsSelectHoster)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtSelectHoster)).BeginInit();
            this.SuspendLayout();
            // 
            // clbHoster
            // 
            this.clbHoster.CheckOnClick = true;
            this.clbHoster.FormattingEnabled = true;
            this.clbHoster.Location = new System.Drawing.Point(12, 12);
            this.clbHoster.Name = "clbHoster";
            this.clbHoster.Size = new System.Drawing.Size(281, 109);
            this.clbHoster.TabIndex = 0;
            this.clbHoster.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.clbHoster_ItemCheck);
            // 
            // btnSelectAll
            // 
            this.btnSelectAll.Location = new System.Drawing.Point(12, 127);
            this.btnSelectAll.Name = "btnSelectAll";
            this.btnSelectAll.Size = new System.Drawing.Size(75, 23);
            this.btnSelectAll.TabIndex = 20;
            this.btnSelectAll.Text = "Select all";
            this.btnSelectAll.UseVisualStyleBackColor = true;
            this.btnSelectAll.Click += new System.EventHandler(this.btnSelectAll_Click);
            // 
            // btnSelectNone
            // 
            this.btnSelectNone.Location = new System.Drawing.Point(93, 127);
            this.btnSelectNone.Name = "btnSelectNone";
            this.btnSelectNone.Size = new System.Drawing.Size(75, 23);
            this.btnSelectNone.TabIndex = 30;
            this.btnSelectNone.Text = "Select none";
            this.btnSelectNone.UseVisualStyleBackColor = true;
            this.btnSelectNone.Click += new System.EventHandler(this.btnSelectNone_Click);
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(218, 127);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 40;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // dsSelectHoster
            // 
            this.dsSelectHoster.DataSetName = "dsSelectHoster";
            this.dsSelectHoster.Tables.AddRange(new System.Data.DataTable[] {
            this.dtSelectHoster});
            // 
            // dtSelectHoster
            // 
            this.dtSelectHoster.Columns.AddRange(new System.Data.DataColumn[] {
            this.colHoster,
            this.colSelected});
            this.dtSelectHoster.TableName = "SelectHoster";
            // 
            // colHoster
            // 
            this.colHoster.ColumnName = "Hoster";
            // 
            // colSelected
            // 
            this.colSelected.ColumnName = "Selected";
            this.colSelected.DataType = typeof(bool);
            // 
            // frmSelectHoster
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(305, 159);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.btnSelectNone);
            this.Controls.Add(this.btnSelectAll);
            this.Controls.Add(this.clbHoster);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmSelectHoster";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Select Hoster";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmSelectHoster_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.dsSelectHoster)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtSelectHoster)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckedListBox clbHoster;
        private System.Windows.Forms.Button btnSelectAll;
        private System.Windows.Forms.Button btnSelectNone;
        private System.Windows.Forms.Button btnOk;
        private System.Data.DataSet dsSelectHoster;
        private System.Data.DataTable dtSelectHoster;
        private System.Data.DataColumn colHoster;
        private System.Data.DataColumn colSelected;
    }
}