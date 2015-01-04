using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using TheDuffman85.ContainerDecrypter;

namespace TheDuffman85.SynologyDownloadStationAdapter
{
    public partial class frmSettings : Form
    {
        #region Variables

        private bool _close = false;        
        private bool _openingContainer = false;        
        
        #endregion

        #region Properties

        public NotifyIcon NotifyIcon
        {
            get
            {
                return this.notifyIcon;
            }
        }        

        #endregion

        #region Constructor

        public frmSettings()
        {
            InitializeComponent();
            this.Opacity = 0;
        }

        #endregion

        #region Eventhandler

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
            this.Show();
            this.Activate();
        }        

        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _close = true;
            this.Close();            
        }

        private void frmSettings_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing && !_close)
            {
                e.Cancel = true;
                this.Hide();
            }
        }
        
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();  
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.Address = txtAddress.Text;
            Properties.Settings.Default.Username = txtUsername.Text;
            Properties.Settings.Default.Password = Convert.ToBase64String(Encoding.UTF8.GetBytes(txtPassword.Text));
            Properties.Settings.Default.ApplicationEnabled = cbApplicationEnabled.Checked;
            Properties.Settings.Default.ApplicationUrl = txtApplicationUrl.Text;

            Properties.Settings.Default.Save();

            if (cbAutostart.Checked != Adapter.IsAutoStart())
            {
                Adapter.ToggleAutoStart();
            }

            // Dispose of Download Station form,
            // because the url could have been changed
            Adapter.FrmDownloadStation.Dispose();

            this.Close();  
        }

        private void frmSettings_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible)
            {                
                txtAddress.Text = Properties.Settings.Default.Address;
                txtUsername.Text = Properties.Settings.Default.Username;
                txtPassword.Text = Encoding.UTF8.GetString(Convert.FromBase64String(Properties.Settings.Default.Password));
                cbApplicationEnabled.Checked = Properties.Settings.Default.ApplicationEnabled;
                txtApplicationUrl.Text = Properties.Settings.Default.ApplicationUrl;

                cbAutostart.Checked = Adapter.IsAutoStart();
            }
        }

        private void openDiskstationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Adapter.OpenDownloadStation();
        }
        
        private void btnFileAssociation_Click(object sender, EventArgs e)
        {
            Adapter.AssociateFileTypes();
        }    

        private void addLinkToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Adapter.FrmAddLinks.Show();
            Adapter.FrmAddLinks.Activate();
        }
        
        private void addContainerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!this._openingContainer)
            {
                this._openingContainer = true;

                try
                {
                    using (OpenFileDialog openFile = new OpenFileDialog())
                    {
                        openFile.Title = "Select a Container File";
                        openFile.Filter = "DLC files (*.dlc)|*.dlc|CCF files (*.ccf)|*.ccf|RSDF files (*.rsdf)|*.rsdf";
                        openFile.Multiselect = false;
                        
                        if (openFile.ShowDialog() == DialogResult.OK)
                        {
                            DcryptItDecrypter decrypter = null;

                            try
                            {
                                decrypter = new DcryptItDecrypter(openFile.FileName);
                            }
                            catch (ArgumentException ex)
                            {
                                Adapter.ShowBalloonTip(ex.Message, ToolTipIcon.Warning);
                            }

                            if (decrypter != null)
                            {
                                decrypter.Decrypt();
                                Adapter.AddLinksToDownloadStation(decrypter.Links.ToList());
                            }                           
                        }
                    }
                }
                catch (Exception ex)
                {
                    Adapter.ShowBalloonTip(ex.Message, ToolTipIcon.Error);
                }
                finally
                {
                    this._openingContainer = false;
                }
            }                       
        }

        private void cbApplicationUrl_CheckedChanged(object sender, EventArgs e)
        {
            if (!cbApplicationEnabled.Checked)
            {
                txtApplicationUrl.Enabled = false;
                txtApplicationUrl.Text = string.Empty;
            }
            else
            {
                txtApplicationUrl.Enabled = true;                
                if (string.IsNullOrEmpty(txtApplicationUrl.Text))
                {
                    txtApplicationUrl.Text = "http://" + txtAddress.Text + "/download/index.cgi";
                }
            }
        }

        private void contextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            if (Properties.Settings.Default.ApplicationEnabled)
            {
                openDiskstationToolStripMenuItem.Text = "Open Download Station";
            }
            else
            {
                openDiskstationToolStripMenuItem.Text = "Open Diskstation";
            }
        }

        private void notifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Adapter.OpenDownloadStation();
        }

        #endregion  

        private void frmSettings_Load(object sender, EventArgs e)
        {
            this.BeginInvoke(new MethodInvoker(delegate
            {
                this.Hide();
                this.Opacity = 1;
            }));            
        }

    }
}
