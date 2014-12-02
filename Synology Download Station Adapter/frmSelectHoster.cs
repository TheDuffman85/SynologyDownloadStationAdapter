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

        private frmSelectHoster(Dictionary<string, List<string>> validHostLinks)
        {
            InitializeComponent();

            this._validHostLinks = validHostLinks;

            foreach (string host in _validHostLinks.Keys)
            {
                clbHoster.Items.Add(host, true);
            }
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

        public static void SelectHoster(Dictionary<string, List<string>> validHostLinks)
        {
            using (frmSelectHoster frm = new frmSelectHoster(validHostLinks))
            {
                frm.ShowDialog(); 
            }
        }

        #endregion
 
    }
}
