using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TheDuffman85.ContainerDecrypter;

namespace TheDuffman85.SynologyDownloadStationAdapter
{
    public partial class frmSettings : Form
    {
        #region Imports

        #if !__MonoCS__

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

        #endif

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

            this.btnFileAssociation.Text = string.Format(this.btnFileAssociation.Text, string.Join(",", Adapter.FILE_TYPES_ALL));
            this.lblVersion.Text = string.Format(this.lblVersion.Text, FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion);

            #if !__MonoCS__
            AddClipboardFormatListener(this.Handle);             
            #else       
            // Show only on Windows                
            btnFileAssociation.Visible = false;
            cbAutostart.Visible = false;
            lblApplicationUrl.Visible = false;
            cbApplicationEnabled.Visible = false;
            txtApplicationPath.Visible = false;
                        
            cbShowDecryptedLinks.Top = cbShowDecryptedLinks.Top - 23;
            cbIgnoreHoster.Top = cbIgnoreHoster.Top - 46;
            Height = Height - 80;
            
            lblVersion.Text = lblVersion.Text + " (Mono)";
            #endif

            #region Upgrade Settings

            bool upgraded = false;
            Uri address;

            if (!Uri.TryCreate(Properties.Settings.Default.Address, UriKind.Absolute, out address) ||
                (address != null && address.Scheme != Uri.UriSchemeHttp && address.Scheme != Uri.UriSchemeHttps))
            {
                Properties.Settings.Default.Address = "http://" + Properties.Settings.Default.Address;
                upgraded = true;
            }            

            if (Properties.Settings.Default.ApplicationEnabled &&
                string.IsNullOrEmpty(Properties.Settings.Default.ApplicationPath))
            {
                Properties.Settings.Default.ApplicationPath = "/download/index.cgi";
                upgraded = true;
            }

            if (upgraded)
            {
                Properties.Settings.Default.Save();
            }

            #endregion

            InitBookmarks();
        }

        #endregion

        #region Methods

        public void InitBookmarks()
        {
            // Remove previous items if necessary
            if (bookmarksToolStripMenuItem.DropDownItems.Count > 2)
            {
                for (int i = bookmarksToolStripMenuItem.DropDownItems.Count-3; i >= 0; i--)
                {
                    bookmarksToolStripMenuItem.DropDownItems.RemoveAt(i);
                }
            }

            DataRow row;

            for (int i = frmBookmarks.Instance.Bookmarks.Rows.Count - 1; i >= 0; i--)
            {
                row = frmBookmarks.Instance.Bookmarks.Rows[i];

                ToolStripMenuItem item = new ToolStripMenuItem(row["Name"].ToString(), (Image)row["Icon"]);
                item.Tag = row["Url"];
                item.Click += bookmarkToolStripMenuItem_Click;

                bookmarksToolStripMenuItem.DropDownItems.Insert(0, item);
            } 
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

        private void frmSettings_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing && !_close)
            {
                e.Cancel = true;
                this.Hide();
            }
            #if !__MonoCS__
            else
            {
                
                RemoveClipboardFormatListener(this.Handle);
                
            }
            #endif
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
            #if !__MonoCS__
            Properties.Settings.Default.ApplicationEnabled = cbApplicationEnabled.Checked;
            Properties.Settings.Default.ApplicationPath = txtApplicationPath.Text;
            #endif
            Properties.Settings.Default.ShowDecryptedLinks = cbShowDecryptedLinks.Checked;
            Properties.Settings.Default.IgnoreHoster = cbIgnoreHoster.Checked;

            Properties.Settings.Default.Save();

            #if !__MonoCS__
            if (cbAutostart.Checked != Adapter.IsAutoStart())
            {
                Adapter.ToggleAutoStart();
            }
            

            // Dispose of Download Station form,
            // because the url could have been changed
            frmDownloadStation.Instance.Dispose();
            #endif

            this.Close();  
        }

        private void frmSettings_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible)
            {                
                txtAddress.Text = Properties.Settings.Default.Address;
                txtUsername.Text = Properties.Settings.Default.Username;
                txtPassword.Text = Encoding.UTF8.GetString(Convert.FromBase64String(Properties.Settings.Default.Password));
                #if !__MonoCS__
                cbApplicationEnabled.Checked = Properties.Settings.Default.ApplicationEnabled;
                txtApplicationPath.Text = Properties.Settings.Default.ApplicationPath;
                #endif
                cbShowDecryptedLinks.Checked = Properties.Settings.Default.ShowDecryptedLinks;
                cbIgnoreHoster.Checked = Properties.Settings.Default.IgnoreHoster;

                #if !__MonoCS__
                cbAutostart.Checked = Adapter.IsAutoStart();
                #endif
            }
        }
                
        
        private void btnFileAssociation_Click(object sender, EventArgs e)
        {   
            #if !__MonoCS__
            Adapter.AssociateFileTypes();
            #endif
        }    
        
        
        private void cbApplicationPath_CheckedChanged(object sender, EventArgs e)
        {
            #if !__MonoCS__
            if (!cbApplicationEnabled.Checked)
            {
                txtApplicationPath.Enabled = false;
                txtApplicationPath.Text = string.Empty;
            }
            else
            {
                txtApplicationPath.Enabled = true;                
                if (string.IsNullOrEmpty(txtApplicationPath.Text))
                {
                    txtApplicationPath.Text = "/download/index.cgi";
                }
            }
            #endif
        }

        private void txtAddress_Validating(object sender, CancelEventArgs e)
        {
            Uri address;
            DialogResult result = System.Windows.Forms.DialogResult.None;

            if (!Uri.TryCreate(txtAddress.Text, UriKind.Absolute, out address))
            {
                result = MessageBox.Show("Please enter a valid uri. eg.: http://diskstation:5000", "Settings", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            }
            else
            {
                if (address.Scheme != Uri.UriSchemeHttp &&
                    address.Scheme != Uri.UriSchemeHttps)
                {
                    result = MessageBox.Show("The protocol has to be http or https.", "Settings", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                }
            }

            if (result == System.Windows.Forms.DialogResult.OK)
            {
                e.Cancel = true;
            }
            else if (result == System.Windows.Forms.DialogResult.Cancel)
            {
                txtAddress.Text = Properties.Settings.Default.Address;
            }
        }

        private void txtApplicationPath_Validating(object sender, CancelEventArgs e)
        {
            Uri path;
            DialogResult result = System.Windows.Forms.DialogResult.None;

            if (!Uri.TryCreate(txtApplicationPath.Text, UriKind.Relative, out path))
            {
                result = MessageBox.Show("Please enter a relative uri. eg.: /download/index.cgi", "Settings", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            }

            if (result == System.Windows.Forms.DialogResult.OK)
            {
                e.Cancel = true;
            }
            else if (result == System.Windows.Forms.DialogResult.Cancel)
            {
                txtApplicationPath.Text = Properties.Settings.Default.ApplicationPath;
            }
        }

        private void notifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Adapter.OpenDownloadStation();
        }

        private void lblVersion_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(Adapter.RELEASE_URL);
        }

        #region ContextMenu

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

        private void openDiskstationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Adapter.OpenDownloadStation();
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
                                new Task(() => { Adapter.AddFileToDownloadStation(openFile.FileName); }).Start();  
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

                                    if (Properties.Settings.Default.ShowDecryptedLinks)
                                    {
                                        frmAddLinks.ShowInstance(decrypter.Links);
                                    }
                                    else
                                    {
                                        Adapter.AddLinksToDownloadStation(decrypter.Links.ToList());
                                    }
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

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmBookmarks.ShowInstance();
        }

        private void bookmarkToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string url = ((ToolStripMenuItem)sender).Tag.ToString();
            Process.Start(url);
        }

        #endregion
                        
        #endregion         

    }
}
