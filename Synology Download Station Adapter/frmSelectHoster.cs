using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TheDuffman85.SynologyDownloadStationAdapter
{
    public partial class frmSelectHoster : Form
    {
        #region Variables

        Dictionary<string, List<string>> _validHostLinks;
        bool _close = false;

        #endregion

        #region Constructor

        public frmSelectHoster()
        {
            InitializeComponent();
        }

        #endregion

        #region Eventhandlers

        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < clbHoster.Items.Count; i++)
			{
                clbHoster.SetItemChecked(i, true);
			}
        }

        private void btnSelectNone_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < clbHoster.Items.Count; i++)
            {
                clbHoster.SetItemChecked(i, false);
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < clbHoster.Items.Count; i++)
            {
                if(!clbHoster.GetItemChecked(i))
                {
                    _validHostLinks.Remove(clbHoster.Items[i].ToString());
                }
            }

            this._close = true;
            this.Close();
        }

        private void frmSelectHoster_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing
                && !this._close)
            {
                e.Cancel = true;
            }
        }

        #endregion

        #region Methods

        public void SelectHoster(Dictionary<string, List<string>> validHostLinks)
        {
            this._validHostLinks = validHostLinks;

            this.clbHoster.Items.Clear();

            foreach (string host in _validHostLinks.Keys)
            {
                this.clbHoster.Items.Add(host, true);
            }

            this.ShowDialog();
        }

        private void frmSelectHoster_Shown(object sender, EventArgs e)
        {
            // For some reason the very first time the windows was shown in background. This workaround will fix that.
            this.TopMost = true;            
        }

        #endregion 
    }
}
