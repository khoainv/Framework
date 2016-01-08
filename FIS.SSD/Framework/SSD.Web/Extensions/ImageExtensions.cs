using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

namespace SSD.Web.Extensions
{
    public static class ImageExtensions
    {
        //Chuyen xuong Framework
        /*
        private static Size GetThumbnailSize(Image original, int maxPixels)
        {
            // Width and height.
            int originalWidth = original.Width;
            int originalHeight = original.Height;
            // Compute best factor to scale entire image based on larger dimension.
            double factor;
            if (originalWidth > originalHeight)
            {
                factor = (double)maxPixels / originalWidth;
            }
            else
            {
                factor = (double)maxPixels / originalHeight;
            }
            return new Size((int)(originalWidth * factor), (int)(originalHeight * factor));
        }
        public static Image ResizeWithFixWidth(this Image imgPhoto, int width)
        {
            double factor = (double)width / imgPhoto.Width;
            return imgPhoto.GetThumbnailImage((int)(imgPhoto.Width * factor), (int)(imgPhoto.Height * factor), null, new System.IntPtr());
        }
        public static Image ResizeWithFixHeight(this Image imgPhoto, int height)
        {
            if (imgPhoto == null) return null;
            double factor = (double)height / imgPhoto.Width;
            return imgPhoto.GetThumbnailImage((int)(imgPhoto.Width * factor), (int)(imgPhoto.Height * factor), null, new System.IntPtr());
        }
        public static Image ResizeWithMaxPixels(this Image imgPhoto, int maxPixels)
        {
            Size sz = GetThumbnailSize(imgPhoto, maxPixels);
            return imgPhoto.GetThumbnailImage(sz.Width, sz.Height, null, new System.IntPtr());
        }

        public static Image FixedSizeWithDistortion(this Image imgPhoto, int Width, int Height)
        {
            return imgPhoto.GetThumbnailImage(Width, Height, null, new System.IntPtr());
        }
        public static Image FixedSizeWithBackground(this Image imgPhoto, int Width, int Height)
        {
            int sourceWidth = imgPhoto.Width;
            int sourceHeight = imgPhoto.Height;
            int sourceX = 0;
            int sourceY = 0;
            int destX = 0;
            int destY = 0;

            float nPercent = 0;
            float nPercentW = 0;
            float nPercentH = 0;

            nPercentW = ((float)Width / (float)sourceWidth);
            nPercentH = ((float)Height / (float)sourceHeight);

            //if we have to pad the height pad both the top and the bottom
            //with the difference between the scaled height and the desired height
            if (nPercentH < nPercentW)
            {
                nPercent = nPercentH;
                destX = (int)((Width - (sourceWidth * nPercent)) / 2);
            }
            else
            {
                nPercent = nPercentW;
                destY = (int)((Height - (sourceHeight * nPercent)) / 2);
            }

            int destWidth = (int)(sourceWidth * nPercent);
            int destHeight = (int)(sourceHeight * nPercent);

            Bitmap bmPhoto = new Bitmap(Width, Height, PixelFormat.Format24bppRgb);
            if (nPercentW < 1 || nPercentH < 1)
                bmPhoto.SetResolution(imgPhoto.HorizontalResolution, imgPhoto.VerticalResolution);
            else
            {
                destX = (int)((Width - sourceWidth) / 2);
                destY = (int)((Height - sourceHeight) / 2);
                destWidth = sourceWidth;
                destHeight = sourceHeight;
            }

            Graphics grPhoto = Graphics.FromImage(bmPhoto);
            grPhoto.Clear(Color.White);
            grPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;

            grPhoto.DrawImage(imgPhoto,
                new Rectangle(destX, destY, destWidth, destHeight),
                new Rectangle(sourceX, sourceY, sourceWidth, sourceHeight),
                GraphicsUnit.Pixel);

            grPhoto.Dispose();
            return bmPhoto;
        }

        public static Image Resize(this Image image, int newWidth, int maxHeight, bool onlyResizeIfWider)
        {
            if (onlyResizeIfWider && image.Width <= newWidth) newWidth = image.Width;

            var newHeight = image.Height * newWidth / image.Width;
            if (newHeight > maxHeight)
            {
                // Resize with height instead
                newWidth = image.Width * maxHeight / image.Height;
                newHeight = maxHeight;
            }

            var thumbnail = new Bitmap(newWidth, newHeight);

            using (var graphic = Graphics.FromImage(thumbnail))
            {
                graphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphic.SmoothingMode = SmoothingMode.HighQuality;
                graphic.PixelOffsetMode = PixelOffsetMode.HighQuality;
                graphic.CompositingQuality = CompositingQuality.HighQuality;
                graphic.DrawImage(image, 0, 0, newWidth, newHeight);
            }
            return thumbnail;
        }

        public static Image Crop(this Image img, Rectangle cropArea)
        {
            var bmpImage = new Bitmap(img);
            var bmpCrop = bmpImage.Clone(cropArea, bmpImage.PixelFormat);
            return bmpCrop;
        }
        */
    }
}
