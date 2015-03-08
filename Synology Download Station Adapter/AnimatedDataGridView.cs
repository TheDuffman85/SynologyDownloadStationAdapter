using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TheDuffman85.SynologyDownloadStationAdapter
{
    public class AnimatedDataGridView : DataGridView
    {
        private DataGridViewImageAnimator _imageAnimator;

        public AnimatedDataGridView()
            : base()
        {
            _imageAnimator = new DataGridViewImageAnimator(this);
        }

        private class DataGridViewImageAnimator
        {
            private class RowCol
            {
                public int Column { get; set; }
                public int Row { get; set; }

                public RowCol(int column, int row)
                {
                    Column = column;
                    Row = row;
                }

                public override bool Equals(object obj)
                {
                    if (obj == null)
                        return false;

                    RowCol other = obj as RowCol;
                    if (other == null)
                        return false;

                    return (other.Column == Column && other.Row == Row);
                }

                public bool Equals(RowCol other)
                {
                    if (other == null)
                        return false;

                    return (other.Column == Column && other.Row == Row);
                }

                public override int GetHashCode()
                {
                    return Column.GetHashCode() ^ Row.GetHashCode();
                }

                public static bool operator ==(RowCol a, RowCol b)
                {
                    // If both are null, or both are same instance, return true.
                    if (object.ReferenceEquals(a, b))
                    {
                        return true;
                    }

                    // If one is null, but not both, return false.
                    if (((object)a == null) || ((object)b == null))
                    {
                        return false;
                    }

                    // Return true if the fields match:
                    return a.Column == b.Column && a.Row == b.Row;
                }

                public static bool operator !=(RowCol a, RowCol b)
                {
                    return !(a == b);
                }
            }

            private class AnimatedImage
            {
                private DataGridView DataGridView { get; set; }
                private HashSet<RowCol> _cells = new HashSet<RowCol>();

                public Image Image { get; set; }

                public AnimatedImage(Image image, DataGridView dataGridView)
                {
                    Image = image;
                    DataGridView = dataGridView;
                }

                public bool IsUsed { get { return _cells.Count > 0; } }

                public void AddCell(RowCol rowCol)
                {
                    Debug.Assert(!_cells.Contains(rowCol));

                    if (!_cells.Contains(rowCol))
                    {
                        _cells.Add(rowCol);

                        if (_cells.Count == 1)
                        {
                            // this is the first cell we are using this image, so start animation
                            ImageAnimator.Animate(Image, new EventHandler(OnFrameChanged));
                        }
                    }
                }

                public void RemoveCell(RowCol rowCol)
                {
                    Debug.Assert(_cells.Contains(rowCol));

                    if (_cells.Contains(rowCol))
                    {
                        _cells.Remove(rowCol);

                        if (_cells.Count == 0)
                        {
                            // this was the last cell we were using this image, so stop animation
                            ImageAnimator.StopAnimate(Image, new EventHandler(OnFrameChanged));
                        }
                    }
                }

                private void OnFrameChanged(object o, EventArgs e)
                {
                    // invalidate each cell in which it's being used
                    RowCol[] rcs = new RowCol[_cells.Count];
                    _cells.CopyTo(rcs);
                    foreach (RowCol rc in rcs)
                    {
                        DataGridView.InvalidateCell(rc.Column, rc.Row);
                    }
                }
            }

            private Dictionary<RowCol, Image> _values = new Dictionary<RowCol, Image>();
            private Dictionary<Image, AnimatedImage> _animatedImages = new Dictionary<Image, AnimatedImage>();
            private DataGridView _dataGridView;

            public DataGridViewImageAnimator(DataGridView dataGridView)
            {
                _dataGridView = dataGridView;
                _dataGridView.CellPainting += new DataGridViewCellPaintingEventHandler(OnDatagridCellPainting);
            }

            void OnDatagridCellPainting(object sender, DataGridViewCellPaintingEventArgs e)
            {
                if (e.ColumnIndex >= 0 && e.RowIndex >= 0)
                {
                    object value = _dataGridView[e.ColumnIndex, e.RowIndex].Value;
                    CheckValue(e.ColumnIndex, e.RowIndex, value);
                    ImageAnimator.UpdateFrames();
                }
            }

            private void AddImage(RowCol rowCol, Image image)
            {
                _values[rowCol] = image;

                AnimatedImage animatedImage;
                if (!_animatedImages.TryGetValue(image, out animatedImage))
                {
                    animatedImage = new AnimatedImage(image, _dataGridView);
                    _animatedImages[image] = animatedImage;
                }

                animatedImage.AddCell(rowCol);
            }

            private void RemoveImage(RowCol rowCol, Image image)
            {
                Debug.Assert(_values.ContainsKey(rowCol));
                Debug.Assert(_animatedImages.ContainsKey(image));

                _values.Remove(rowCol);

                AnimatedImage animatedImage;
                if (_animatedImages.TryGetValue(image, out animatedImage))
                {
                    animatedImage.RemoveCell(rowCol);
                    if (!animatedImage.IsUsed)
                    {
                        _animatedImages.Remove(image);
                    }
                }
            }

            private void CheckValue(int columnIndex, int rowIndex, object value)
            {
                RowCol rowCol = new RowCol(columnIndex, rowIndex);

                // is the new value an Image, and can it be animated?
                Image newImage = value as Image;
                bool newValueIsImage = (newImage != null && ImageAnimator.CanAnimate(newImage));

                // is there a previous image value?
                Image oldImage;
                if (_values.TryGetValue(rowCol, out oldImage))
                {
                    if (newImage == oldImage)
                    {
                        // same old image --> nothing else to do
                        return;
                    }

                    RemoveImage(rowCol, oldImage);
                }

                if (newValueIsImage)
                {
                    AddImage(rowCol, newImage);
                }
            }
        }
    }
}
