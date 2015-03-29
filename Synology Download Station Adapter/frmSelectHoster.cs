using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
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
            SaveHosterSelection(); 

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
            else 
            { 
                RemoveUnselected();
            }
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

        #region Methods

        private void RemoveUnselected()
        {
            for (int i = 0; i < clbHoster.Items.Count; i++)
            {
                if (!clbHoster.GetItemChecked(i))
                {
                    _validHostLinks.Remove(clbHoster.Items[i].ToString());
                }
            }
        }

        public void SelectHoster(Dictionary<string, List<string>> validHostLinks)
        {
            this._validHostLinks = validHostLinks;

            this.clbHoster.Items.Clear();

            foreach (string host in _validHostLinks.Keys)
            {
                this.clbHoster.Items.Add(host);
            }

            LoadHosterSelection();


            if (this.clbHoster.CheckedItems.Count == 1 &&
                Properties.Settings.Default.IgnoreHoster)
            {
                RemoveUnselected();                
            }
            else
            {
                this.ShowDialog();
            }
        }

        private void LoadHosterSelection()
        {
            if (!string.IsNullOrEmpty(Properties.Settings.Default.HosterSelection))
            {
                StringReader reader = new StringReader(Properties.Settings.Default.HosterSelection);
                dtSelectHoster.ReadXml(reader);

                for (int i = 0; i < this.clbHoster.Items.Count; i++)
                {
                    foreach (DataRow row in dtSelectHoster.Rows)
                    {
                        if ((string)this.clbHoster.Items[i] == (string)row[0])
                        {
                            this.clbHoster.SetItemChecked(i, (bool)row[1]);
                            break;
                        }
                    }
                }                
            }
        }

        private void SaveHosterSelection()
        {
            for (int i = 0; i < this.clbHoster.Items.Count; i++)
            {
                bool exists = false;

                foreach (DataRow row in dtSelectHoster.Rows)
                {
                    if ((string)this.clbHoster.Items[i] == (string)row[0])
                    {
                        row[1] = this.clbHoster.GetItemChecked(i);
                        exists = true;
                        break;
                    }
                }

                if (!exists)
                {
                    DataRow row = dtSelectHoster.NewRow();
                    row[0] = this.clbHoster.Items[i];
                    row[1] = this.clbHoster.GetItemChecked(i);

                    dtSelectHoster.Rows.Add(row);
                }
            } 

            StringWriter writer = new StringWriter();
            dtSelectHoster.WriteXml(writer);

            Properties.Settings.Default.HosterSelection = writer.ToString();
            Properties.Settings.Default.Save();
        }

        #endregion 

        
    }
}
