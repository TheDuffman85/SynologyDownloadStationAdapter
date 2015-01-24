using Awesomium.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TheDuffman85.SynologyDownloadStationAdapter.Properties;

namespace TheDuffman85.SynologyDownloadStationAdapter
{
    public partial class frmDownloadStation : Form
    {
        #region Variables

        private static object _lock = new object();
        private static frmDownloadStation _instance;
        private bool _newInstance = false;

        #endregion

        #region Properties

        /// <summary>
        /// Lazy instance
        /// </summary>       
        public static frmDownloadStation Instance
        {
            get
            {
                lock (_lock)
                {
                    if (_instance != null &&
                        _instance._newInstance)
                    {
                        _instance.Dispose();
                    }

                    if (_instance == null ||
                        _instance.IsDisposed)
                    {
                        _instance = new frmDownloadStation();
                    }
                }

                return _instance;
            }
        }

        #endregion

        #region Constructor

        private frmDownloadStation()
        {
            InitializeComponent();

            this.Width = Settings.Default.SizeX;
            this.Height = Settings.Default.SizeY;

            try
            {
                // Add a WebSession.
                webControl.WebSession = WebCore.CreateWebSession(Path.Combine(Adapter.AssemblyDirectory, "Session"), new WebPreferences()
                {
                    AcceptLanguage = CultureInfo.CurrentCulture.Name + "," + CultureInfo.CurrentCulture.TwoLetterISOLanguageName
                }); 
            }
            catch (Exception ex)
            {
                Adapter.ShowBalloonTip(ex.Message, ToolTipIcon.Error);

                pbLoadingIndicator.Image = pbLoadingIndicator.ErrorImage;
                this.NewIntance();
            }               
        }

        #endregion

        #region Static Methods

        public static void ShowInstance()
        {
            Instance.Show();
            Instance.Activate();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Create a new instance on next call
        /// </summary>
        protected void NewIntance()
        {
            _instance._newInstance = true;
        }

        #endregion

        #region Eventhandlers

        private void frmDownloadStation_Load(object sender, EventArgs e)
        {                        
            webControl.DocumentReady += webControl_DocumentReady;
            webControl.Source = new Uri(Properties.Settings.Default.ApplicationUrl);
        }

        private void frmDownloadStation_ResizeEnd(object sender, EventArgs e)
        {
            Settings.Default.SizeX = this.Width;
            Settings.Default.SizeY = this.Height;

            Settings.Default.Save();
        }
        
        private void Awesomium_Windows_Forms_WebControl_CertificateError(object sender, Awesomium.Core.CertificateErrorEventArgs e)
        {
            e.Handled = Awesomium.Core.EventHandling.Modal;
            e.Ignore = true;
        }

        private void frmDownloadStation_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                this.Visible = false;
            }
        }

        private void webControl_DocumentReady(object sender, DocumentReadyEventArgs e)
        {
            if (e.Url.Segments.Length == 3 &&
                e.Url.Segments[2] == "index.cgi")
            {
                if (e.ReadyState != DocumentReadyState.Loaded)
                {
                    string js = "$('login_username').setValue('" + Properties.Settings.Default.Username + "');" +
                                "$('login_passwd').setValue('" + Encoding.UTF8.GetString(Convert.FromBase64String(Properties.Settings.Default.Password)) + "');" +
                                "if($('ext-gen31').className.indexOf('checked') < 0) { $('ext-gen31').click(); }" +
                                "$('ext-gen32').click();";

                    webControl.ExecuteJavascript(js);
                }
                else
                {
                    pbLoadingIndicator.Visible = false;
                }
            }
        }

        private void Awesomium_Windows_Forms_WebControl_LoadingFrameFailed(object sender, LoadingFrameFailedEventArgs e)
        {
            pbLoadingIndicator.Image = pbLoadingIndicator.ErrorImage;
            this.NewIntance();
        }

        private void Awesomium_Windows_Forms_WebControl_Crashed(object sender, CrashedEventArgs e)
        {
            pbLoadingIndicator.Image = pbLoadingIndicator.ErrorImage;
            this.NewIntance();
        }

        private void Awesomium_Windows_Forms_WebControl_ShowJavascriptDialog(object sender, JavascriptDialogEventArgs e)
        {
            e.Cancel = true;
            e.Handled = true;
        }

        #endregion
                
     }  
}
