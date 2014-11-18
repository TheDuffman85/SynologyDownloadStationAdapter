using SynologyAPI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TheDuffman85.SynologyDownloadStationAdapter
{
    public partial class frmAddLinks : Form
    {
        #region Variable

        private bool _loading = true;

        #endregion

        #region Constructor

        public frmAddLinks()
        {
            InitializeComponent();
        }

        #endregion

        #region Eventhandler

        private void frmAddLinks_Load(object sender, EventArgs e)
        {
            cbClipboard.Checked = Properties.Settings.Default.CheckClipboard;

            CheckClipboard();

            _loading = false;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            Adapter.AddLinksToDownloadStation(txtLinks.Lines.ToList());
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtLinks_TextChanged(object sender, EventArgs e)
        {
            List<string> links = txtLinks.Lines.ToList();
            links.RemoveAll(str => String.IsNullOrEmpty(str.Trim()));

            btnAdd.Enabled = links.Count > 0;
        }

        private void cbClipboard_CheckedChanged(object sender, EventArgs e)
        {
            if (!_loading)
            {
                Properties.Settings.Default.CheckClipboard = cbClipboard.Checked;
                Properties.Settings.Default.Save();

                CheckClipboard();
            }            
        }

        #endregion

        #region Methods

        private void CheckClipboard()
        {
            if (Properties.Settings.Default.CheckClipboard)
            {
                if (Clipboard.ContainsText())
                {
                    txtLinks.Text = Clipboard.GetText();
                }
            }
        }

        #endregion
    }
}
