using Awesomium.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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

        bool _crashed = false;

        #endregion

        #region Properties

        public bool Crashed
        {
            get
            {
                return this._crashed;
            }
        }

        #endregion

        #region Constructor

        public frmDownloadStation()
        {
            InitializeComponent();
        }

        #endregion

        #region Eventhandlers

        private void frmDownloadStation_Load(object sender, EventArgs e)
        {
            webControl.DocumentReady += webControl_DocumentReady;
            webControl.Source = new Uri(Properties.Settings.Default.ApplicationUrl);
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
                                "$('ext-gen32').click()";

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
            this._crashed = true;
        }

        private void Awesomium_Windows_Forms_WebControl_Crashed(object sender, CrashedEventArgs e)
        {
            pbLoadingIndicator.Image = pbLoadingIndicator.ErrorImage;
            this._crashed = true;
        }

        private void Awesomium_Windows_Forms_WebControl_ShowJavascriptDialog(object sender, JavascriptDialogEventArgs e)
        {
            e.Cancel = true;
            e.Handled = true;
        }

        #endregion
     }  
}
