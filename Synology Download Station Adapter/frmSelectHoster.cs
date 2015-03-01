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

        private static object _lock = new object();
        private static frmSelectHoster _instance;
        Dictionary<string, List<string>> _validHostLinks;
        bool _close = false;

        #endregion

        #region Properties

        /// <summary>
        /// Lazy instance
        /// </summary>       
        public static frmSelectHoster Instance
        {
            get
            {
                lock (_lock)
                {                    
                    if (_instance == null ||
                        _instance.IsDisposed)
                    {
                        _instance = new frmSelectHoster();
                    }
                }

                return _instance;
            }
        }      

        #endregion

        #region Constructor

        private frmSelectHoster()
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
        
        private void clbHoster_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (e.NewValue == CheckState.Checked ||
                clbHoster.CheckedItems.Count > 1 ||
               (clbHoster.CheckedItems.Count == 1 && e.NewValue != CheckState.Unchecked))
            {
                btnOk.Text = "Ok";
            }
            else
            {
                btnOk.Text = "Cancel";
            }
        }

        #endregion 

        
    }
}
