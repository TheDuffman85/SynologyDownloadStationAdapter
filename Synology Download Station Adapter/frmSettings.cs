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
        #region Imports

        /// <summary>
        /// Places the given window in the system-maintained clipboard format listener list.
        /// </summary>
        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool AddClipboardFormatListener(IntPtr hwnd);

        /// <summary>
        /// Removes the given window from the system-maintained clipboard format listener list.
        /// </summary>
        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool RemoveClipboardFormatListener(IntPtr hwnd);

        #endregion

        #region Constants

        /// <summary>
        /// Sent when the contents of the clipboard have changed.
        /// </summary>
        private const int WM_CLIPBOARDUPDATE = 0x031D;

        #endregion

        #region Variables

        private static object _lock = new object();
        private static frmSettings _instance;
        private bool _close = false;        
        private bool _openingContainer = false;        
        
        #endregion

        #region Properties

        /// <summary>
        /// Lazy instance
        /// </summary>       
        public static frmSettings Instance
        {
            get
            {
                lock (_lock)
                {                    
                    if (_instance == null ||
                        _instance.IsDisposed)
                    {
                        _instance = new frmSettings();
                    }
                }

                return _instance;
            }
        }
                
        public NotifyIcon NotifyIcon
        {
            get
            {
                return this.notifyIcon;
            }
        }        

        #endregion

        #region Constructor

        private frmSettings()
        {
            InitializeComponent();
            this.Opacity = 0;

            AddClipboardFormatListener(this.Handle); 

            this.btnFileAssociation.Text = string.Format(this.btnFileAssociation.Text, string.Join(",", Adapter.FILE_TYPES_ALL) );
        }

        #endregion

        #region Eventhandler

        private void frmSettings_Load(object sender, EventArgs e)
        {
            this.BeginInvoke(new MethodInvoker(delegate
            {
                this.Hide();
                this.Opacity = 1;
            }));            
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            if (m.Msg == WM_CLIPBOARDUPDATE &&
                Properties.Settings.Default.CheckClipboard)
            {
                if (Clipboard.ContainsText())
                {
                    string text = Clipboard.GetText();
                    
                    if (!string.IsNullOrEmpty(text))
                    {
                        string[] split = text.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

                        bool url = false;

                        foreach (string item in split)
                        {
                            if (Uri.IsWellFormedUriString(item, UriKind.Absolute))
                            {
                                url = true;
                                break;
                            }
                        }

                        if (url)
                        {
                            frmAddLinks.ShowInstance();
                        }
                    }


                    
                }
            }
        }

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
            else
            {
                RemoveClipboardFormatListener(this.Handle);
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
            frmDownloadStation.Instance.Dispose();

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
            frmAddLinks.ShowInstance();
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
                        openFile.Filter = "Container Files (*" + string.Join(" ,*", Adapter.FILE_TYPES_ALL).Trim(" ,".ToCharArray()) + ")|*" + string.Join(";*", Adapter.FILE_TYPES_ALL).Trim(";".ToCharArray()) + "|All files (*.*)|*.*";
                        openFile.Multiselect = false;
                        
                        if (openFile.ShowDialog() == DialogResult.OK)
                        {
                            if (Adapter.FILE_TYPES_NO_DECRYPT.Contains(Path.GetExtension(openFile.FileName).ToLower()))
                            {
                                Adapter.AddFileToDownloadStation(openFile.FileName);
                            }
                            else
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

    }
}
