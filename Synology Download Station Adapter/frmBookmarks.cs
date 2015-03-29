using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TheDuffman85.Tools;

namespace TheDuffman85.SynologyDownloadStationAdapter
{
    public partial class frmBookmarks : Form
    {
        #region Variables

        private static object _lock = new object();
        private static frmBookmarks _instance;
 
        #endregion

        #region Properties

        /// <summary>
        /// Lazy instance
        /// </summary>       
        public static frmBookmarks Instance
        {
            get
            {
                lock (_lock)
                {                    
                    if (_instance == null ||
                        _instance.IsDisposed)
                    {
                        _instance = new frmBookmarks();
                    }
                }

                return _instance;
            }
        }

        public DataTable Bookmarks
        {
            get
            {
                return dtBookmarks;
            }
        }

        #endregion

        #region Constructor

        private frmBookmarks()
        {
            InitializeComponent();

            if (string.IsNullOrEmpty(Properties.Settings.Default.Bookmarks))
            {
                dtBookmarks.Rows.Add(Properties.Resources.synology_forum_de, "Synology Download Station Adapter", "http://www.synology-forum.de/showthread.html?59848-Synology-Download-Station-Adapter");
                dtBookmarks.Rows.Add(Properties.Resources.github_com, "TheDuffman85/SynologyDownloadStationAdapter", "https://github.com/TheDuffman85/SynologyDownloadStationAdapter");
            }
            else
            {
                // Unbind datagridview
                dgvBookmarks.DataSource = null;

                StringReader reader = new StringReader(Properties.Settings.Default.Bookmarks);
                dtBookmarks.ReadXml(reader);
                
                foreach (DataRow row in dtBookmarks.Rows)
                {
                    MemoryStream ms = new MemoryStream((byte[])row[0]);
                    row[0] = Image.FromStream(ms);
                }

                // Bind datagridview
                dgvBookmarks.DataSource = bsBookmark;
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

        #region Eventhandler

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            StringWriter writer = new StringWriter();
            ImageConverter imageConverter = new ImageConverter();

            // Unbind datagridview
            dgvBookmarks.DataSource = null;

            frmSettings.Instance.InitBookmarks();
            
            // Convert images to byte[]
            foreach (DataRow row in dtBookmarks.Rows)
	        {
                row[0] = imageConverter.ConvertTo(row[0], typeof(byte[]));
	        }            

            dtBookmarks.WriteXml(writer);

            Properties.Settings.Default.Bookmarks = writer.ToString();          
            Properties.Settings.Default.Save();

            this.Close();
        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            int currentPosition = bsBookmark.Position;  
            DataRow oldRow = ((DataRowView)bsBookmark.Current).Row;
            DataRow newRow = dtBookmarks.NewRow();


            newRow.ItemArray = oldRow.ItemArray;
            
            if (currentPosition > 0)
            {
                bsBookmark.SuspendBinding();
                dtBookmarks.Rows.RemoveAt(currentPosition);
                dtBookmarks.Rows.InsertAt(newRow, currentPosition - 1);
                bsBookmark.ResumeBinding();

                bsBookmark.Position = bsBookmark.Count-1;
                bsBookmark.Position = currentPosition - 1;
            }            
        }

        private void btnDown_Click(object sender, EventArgs e)
        {
            int currentPosition = bsBookmark.Position;
            DataRow oldRow = ((DataRowView)bsBookmark.Current).Row;
            DataRow newRow = dtBookmarks.NewRow();

            newRow.ItemArray = oldRow.ItemArray;

            if (currentPosition < bsBookmark.Count - 1)
            {
                bsBookmark.SuspendBinding();
                dtBookmarks.Rows.RemoveAt(currentPosition);
                dtBookmarks.Rows.InsertAt(newRow, currentPosition + 1);
                bsBookmark.ResumeBinding();

                bsBookmark.Position = -1;
                bsBookmark.Position = currentPosition + 1;
            } 
        }
        
        private void btnAdd_Click(object sender, EventArgs e)
        {

            DataRowView drv = (DataRowView)bsBookmark.AddNew();
            
            dgvBookmarks.CurrentCell = dgvBookmarks[1, bsBookmark.Position];

            dgvBookmarks[0, bsBookmark.Position].Value = new Bitmap(16, 16);

            dgvBookmarks.BeginEdit(true);
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            bsBookmark.SuspendBinding();

            int newIndex = int.MaxValue;

            foreach (DataGridViewRow row in dgvBookmarks.SelectedRows)
            {
                if (row.Index < newIndex)
                {
                    newIndex = row.Index;
                }

                bsBookmark.RemoveAt(row.Index);
            }

            bsBookmark.ResetBindings(false);
            bsBookmark.Position = newIndex - 1;
        }
                
        private void dgvBookmarks_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvBookmarks.SelectedRows.Count == 1)
            {
                btnUp.Enabled = true;
                btnDown.Enabled = true;
            }
            else
            {
                btnUp.Enabled = false;
                btnDown.Enabled = false;
            }

            if (dgvBookmarks.SelectedRows.Count > 0)
            {
                btnRemove.Enabled = true;
            }
            else
            {
                btnRemove.Enabled = false;
            }
        }

        private void dgvBookmarks_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            Uri url;

            if (e.ColumnIndex == 2 &&
                Uri.TryCreate(dgvBookmarks[e.ColumnIndex, e.RowIndex].Value as string, UriKind.RelativeOrAbsolute, out url))
            {                   
                url = new UriBuilder(dgvBookmarks[e.ColumnIndex, e.RowIndex].Value as string).Uri;

                dgvBookmarks[0, e.RowIndex].Value = Properties.Resources.indicator_small;
                dgvBookmarks[2, e.RowIndex].Value = url.ToString();

                if (url.IsFile)
                {
                    dgvBookmarks[0, e.RowIndex].Value = Properties.Resources.folder; 
                }
                else
                {
                    if (url.Scheme == "http" ||
                        url.Scheme == "https")
                    {
                        Favicon favicon = new Favicon();
                        favicon.GetFromUrlAsyncCompleted += favicon_GetFromUrlAsyncCompleted;
                        favicon.Tag = dgvBookmarks[0, e.RowIndex];

                        btnSave.Enabled = false;
                        favicon.GetFromUrlAsync(url);
                    }
                    else if (url.Scheme == "ftp")
                    {
                        dgvBookmarks[0, e.RowIndex].Value = Properties.Resources.ftp;
                    }
                    else
                    {
                        dgvBookmarks[0, e.RowIndex].Value = Properties.Resources.world; 
                    }
                }
            }
        }

        private void favicon_GetFromUrlAsyncCompleted(object sender, AsyncCompletedEventArgs e)
        {
            if (!this.IsDisposed)
            {
                Favicon favicon = (Favicon)sender;
                DataGridViewCell cell = (DataGridViewCell)favicon.Tag;

                if (e.Error == null && favicon.Icon != null)
                {
                    cell.Value = favicon.Icon.GetThumbnailImage(16, 16, null, IntPtr.Zero);
                }
                else
                {
                    cell.Value = Properties.Resources.world;
                }

                btnSave.Invoke((MethodInvoker)(() =>
                {
                    btnSave.Enabled = true;
                }
                ));
            }
        }

        private void dgvBookmarks_RowValidating(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (dgvBookmarks.IsCurrentRowDirty)
            {
                Uri url;

                if (string.IsNullOrEmpty(dgvBookmarks[1, e.RowIndex].Value as string))
                {
                    e.Cancel = true;
                    MessageBox.Show("Enter a name", "Bookmarks", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                }
                else if (!Uri.TryCreate(dgvBookmarks[2, e.RowIndex].Value as string, UriKind.RelativeOrAbsolute, out url))
                {
                    e.Cancel = true;
                    MessageBox.Show("Enter a valid uri", "Bookmarks", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }
        
        #endregion     
    }
}
