using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using PostSharp.Patterns.Contracts;
using Svg;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel.Entities;
using ThreatsManager.Utilities.WinForms.Properties;

namespace ThreatsManager.Utilities.WinForms
{
    public partial class ImageSelector : UserControl
    {
        #region Private member variables.
        private bool _bigImageChanged;
        private bool _bigImageAutoChanged;
        private bool _imageChanged;
        private bool _imageAutoChanged;
        private bool _smallImageChanged;
        private bool _smallImageAutoChanged;
        #endregion

        public ImageSelector()
        {
            InitializeComponent();
        }

        #region Public Members.

        public event Action Ready;

        public void Initialize([NotNull] IEntity entity)
        {
            _bigImage.Image = entity.GetImage(ImageSize.Big);
            _image.Image = entity.GetImage(ImageSize.Medium);
            _smallImage.Image = entity.GetImage(ImageSize.Small);
        }

        public void Initialize([NotNull] IEntityTemplate template)
        {
            _bigImage.Image = template.GetImage(ImageSize.Big);
            _image.Image = template.GetImage(ImageSize.Medium);
            _smallImage.Image = template.GetImage(ImageSize.Small);
        }

        public Bitmap BigImage => _bigImageChanged || _bigImageAutoChanged ? (Bitmap)_bigImage.Image : null;

        public Bitmap Image => _imageChanged || _imageAutoChanged ? (Bitmap)_image.Image : null;

        public Bitmap SmallImage => _smallImageChanged || _smallImageAutoChanged ? (Bitmap)_image.Image : null;
        #endregion

        private void _bigImage_DoubleClick(object sender, EventArgs e)
        {
            if (_openImage.ShowDialog(Form.ActiveForm) == DialogResult.OK)
            {
                try
                {
                    if (string.Compare(Path.GetExtension(_openImage.FileName), ".svg", 
                            StringComparison.InvariantCultureIgnoreCase) != 0)
                    {
                        var image = Bitmap.FromFile(_openImage.FileName);
                        _bigImage.Image = new Bitmap(image, 64, 64);
                        _bigImageChanged = true;

                        if (!_imageChanged)
                        {
                            _image.Image = new Bitmap(image, 32, 32);
                            _imageAutoChanged = true;
                        }

                        if (!_smallImageChanged)
                        {
                            _smallImage.Image = new Bitmap(image, 16, 16);
                            _smallImageAutoChanged = true;
                        }
                    }
                    else
                    {
                        SvgDocument svg = SvgDocument.Open(_openImage.FileName);
                        if (CalculateBorder(svg, 5, 512, out var left, out var right, out var top, out var bottom))
                        {
                            _bigImage.Image = ConvertSvg(svg, 64, 512, left, right, top, bottom);
                            _bigImageChanged = true;

                            if (!_imageChanged)
                            {
                                _image.Image = ConvertSvg(svg, 32, 512, left, right, top, bottom);
                                _imageAutoChanged = true;
                            }

                            if (!_smallImageChanged)
                            {
                                _smallImage.Image = ConvertSvg(svg, 16, 512, left, right, top, bottom);
                                _smallImageAutoChanged = true;
                            }
                        }
                    }

                    Ready?.Invoke();
                }
                catch
                {
                    MessageBox.Show(Resources.ErrorImageProcessing, 
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void _image_DoubleClick(object sender, EventArgs e)
        {
            if (_openImage.ShowDialog(Form.ActiveForm) == DialogResult.OK)
            {
                try
                {
                    if (string.Compare(Path.GetExtension(_openImage.FileName), ".svg",
                            StringComparison.InvariantCultureIgnoreCase) != 0)
                    {
                        var image = Bitmap.FromFile(_openImage.FileName);
                        _image.Image = new Bitmap(image, 32, 32);
                        _imageChanged = true;

                        if (!_bigImageChanged)
                        {
                            _bigImage.Image = new Bitmap(image, 64, 64);
                            _bigImageAutoChanged = true;
                        }

                        if (!_smallImageChanged)
                        {
                            _smallImage.Image = new Bitmap(image, 16, 16);
                            _smallImageAutoChanged = true;
                        }
                    }
                    else
                    {
                        SvgDocument svg = SvgDocument.Open(_openImage.FileName);
                        if (CalculateBorder(svg, 5, 512, out var left, out var right, out var top, out var bottom))
                        {
                            _image.Image = ConvertSvg(svg, 32, 512, left, right, top, bottom);
                            _imageChanged = true;

                            if (!_bigImageChanged)
                            {
                                _bigImage.Image = ConvertSvg(svg, 64, 512, left, right, top, bottom);
                                _bigImageAutoChanged = true;
                            }

                            if (!_smallImageChanged)
                            {
                                _smallImage.Image = ConvertSvg(svg, 16, 512, left, right, top, bottom);
                                _smallImageAutoChanged = true;
                            }
                        }
                    }

                    Ready?.Invoke();
                }
                catch
                {
                    MessageBox.Show(Resources.ErrorImageProcessing,
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void _smallImage_DoubleClick(object sender, EventArgs e)
        {
            if (_openImage.ShowDialog(Form.ActiveForm) == DialogResult.OK)
            {
                try
                {
                    if (string.Compare(Path.GetExtension(_openImage.FileName), ".svg",
                            StringComparison.InvariantCultureIgnoreCase) != 0)
                    {
                        var image = Bitmap.FromFile(_openImage.FileName);
                        _smallImage.Image = new Bitmap(image, 16, 16);
                        _smallImageChanged = true;

                        if (!_bigImageChanged)
                        {
                            _bigImage.Image = new Bitmap(image, 64, 64);
                            _bigImageAutoChanged = true;
                        }

                        if (!_imageChanged)
                        {
                            _image.Image = new Bitmap(image, 32, 32);
                            _imageAutoChanged = true;
                        }
                    }
                    else
                    {
                        SvgDocument svg = SvgDocument.Open(_openImage.FileName);
                        if (CalculateBorder(svg, 5, 512, out var left, out var right, out var top, out var bottom))
                        {
                            _smallImage.Image = ConvertSvg(svg, 16, 512, left, right, top, bottom);
                            _smallImageChanged = true;

                            if (!_bigImageChanged)
                            {
                                _bigImage.Image = ConvertSvg(svg, 64, 512, left, right, top, bottom);
                                _bigImageAutoChanged = true;
                            }

                            if (!_imageChanged)
                            {
                                _image.Image = ConvertSvg(svg, 32, 512, left, right, top, bottom);
                                _imageAutoChanged = true;
                            }
                        }
                    }

                    Ready?.Invoke();
                }
                catch
                {
                    MessageBox.Show(Resources.ErrorImageProcessing,
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void _changeBigImage_Click(object sender, EventArgs e)
        {
            _bigImage_DoubleClick(sender, e);
        }

        private void _changeMediumImage_Click(object sender, EventArgs e)
        {
            _image_DoubleClick(sender, e);
        }

        private void _changeSmallImage_Click(object sender, EventArgs e)
        {
            _smallImage_DoubleClick(sender, e);
        }

        private Bitmap ConvertSvg([NotNull] SvgDocument svg, int targetHeight, int referenceHeight, int left, int right, int top, int bottom)
        {
            Bitmap result = null;

            if (targetHeight > 0)
            {
                var factor = ((float)targetHeight) / ((float)(referenceHeight - top - bottom));
                var height = (int)(targetHeight + ((top + bottom) * factor));
                var bitmap = svg.Draw(0, height);

                var fullHeight = bitmap.Height;
                var topCrop = (int)(top * factor * height / fullHeight);
                var bottomCrop = (int)(bottom * factor * height / fullHeight);
                var leftCrop = (int)(left * factor * height / fullHeight);
                var rightCrop = (int)(right * factor * height / fullHeight);


                Rectangle cropRect = new Rectangle(leftCrop, topCrop, bitmap.Width - leftCrop - rightCrop, targetHeight);
                result = new Bitmap(cropRect.Width, cropRect.Height);

                using (Graphics g = Graphics.FromImage(result))
                {
                    g.DrawImage(bitmap, new Rectangle(0, 0, result.Width, result.Height),
                        cropRect,
                        GraphicsUnit.Pixel);
                }
            }

            return result;
        }

        private bool CalculateBorder([NotNull] SvgDocument svg, int pixelMargin, int referenceHeight,
            out int left, out int right, out int top, out int bottom)
        {
            bool result = false;

            left = -1;
            right = -1;
            top = -1;
            bottom = -1;

            var bitmap = svg.Draw(0, referenceHeight);

            int fullHeight = bitmap.Height;
            int fullWidth = bitmap.Width;

            int? leftEmpty = null;
            int? rightEmpty = null;
            int? topEmpty = null;
            int? bottomEmpty = null;

            for (int y = 0; y < fullHeight; y++)
            {
                for (int x = 0; x < fullWidth; x++)
                {
                    var color = bitmap.GetPixel(x, y);
                    if (color != Color.White && !color.A.Equals(0))
                    {
                        if (!leftEmpty.HasValue || leftEmpty.Value > x)
                            leftEmpty = x;
                        if (!rightEmpty.HasValue || rightEmpty.Value < x)
                            rightEmpty = x;
                        if (!topEmpty.HasValue || topEmpty.Value > y)
                            topEmpty = y;
                        if (!bottomEmpty.HasValue || bottomEmpty.Value < y)
                            bottomEmpty = y;
                    }
                }
            }

            if (leftEmpty.HasValue && rightEmpty.HasValue && topEmpty.HasValue && bottomEmpty.HasValue)
            {
                if (topEmpty.Value > pixelMargin && bottomEmpty.Value < fullHeight - pixelMargin)
                {
                    top = topEmpty.Value - pixelMargin;
                    bottom = fullHeight - bottomEmpty.Value - pixelMargin;
                }
                else
                {
                    top = topEmpty.Value;
                    bottom = fullHeight - bottomEmpty.Value;
                }

                if (leftEmpty.Value > pixelMargin && rightEmpty.Value < fullWidth - pixelMargin)
                {
                    left = leftEmpty.Value - pixelMargin;
                    right = fullWidth - rightEmpty.Value - pixelMargin;
                }
                else
                {
                    left = leftEmpty.Value;
                    right = fullWidth - rightEmpty.Value;
                }

                var netHeight = fullHeight - bottom - top + 1;
                var netWidth = fullWidth - right - left + 1;

                if (netHeight > netWidth)
                {
                    var deltaWidth = netHeight - netWidth;

                    if (netWidth + deltaWidth <= fullWidth)
                    {
                        // Expand Width to square.
                        left = left - (deltaWidth / 2);
                        right = right - (deltaWidth / 2);
                    }
                }
                else if (netHeight < netWidth)
                {
                    var deltaHeight = netWidth - netHeight;

                    if (netHeight + deltaHeight <= fullHeight)
                    {
                        // Expand Height to square.
                        top = top - (deltaHeight / 2);
                        bottom = bottom - (deltaHeight / 2);
                    }
                }

                result = true;
            }

            return result;
        }
    }
}
