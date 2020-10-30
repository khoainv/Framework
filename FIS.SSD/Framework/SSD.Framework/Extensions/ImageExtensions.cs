using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace SSD.Framework.Extensions
{
    public enum CompareResult
    {
        ciCompareOk,
        ciPixelMismatch,
        ciSizeMismatch
    };
    public static class ImageExtensions
    {
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
            using (Graphics grPhoto = Graphics.FromImage(bmPhoto))
            {
                grPhoto.Clear(Color.White);
                grPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;

                grPhoto.DrawImage(imgPhoto,
                    new Rectangle(destX, destY, destWidth, destHeight),
                    new Rectangle(sourceX, sourceY, sourceWidth, sourceHeight),
                    GraphicsUnit.Pixel);
                grPhoto.Dispose();
            }
            //imgPhoto.Dispose();
            //imgPhoto = null;
            //bmPhoto.UnlockBits(
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
                graphic.Dispose();
            }
            //image.Dispose();
            //image = null;
            return thumbnail;
        }

        public static Image Crop(this Image img, Rectangle cropArea)
        {
            var bmpImage = new Bitmap(img);
            var bmpCrop = bmpImage.Clone(cropArea, bmpImage.PixelFormat);
            return bmpCrop;
        }
        public static CompareResult Compare(this Image img, Image img1)
        {
            return Compare(img, img1);
        }
        public static CompareResult Compare(this Image img, byte[] btImage1)
        {
            return Compare(img, btImage1);
        }
        private static CompareResult CompareImage(Bitmap bmp1, byte[] btImage2)
        {
            CompareResult cr = CompareResult.ciCompareOk;
            //Convert each image to a byte array
            System.Drawing.ImageConverter ic =
                    new System.Drawing.ImageConverter();
            byte[] btImage1 = new byte[1];
            btImage1 = (byte[])ic.ConvertTo(bmp1, btImage1.GetType());

            //Compute a hash for each image
            System.Security.Cryptography.SHA256Managed shaM = new System.Security.Cryptography.SHA256Managed();
            byte[] hash1 = shaM.ComputeHash(btImage1);
            byte[] hash2 = shaM.ComputeHash(btImage2);

            //Compare the hash values
            for (int i = 0; i < hash1.Length && i < hash2.Length
                                && cr == CompareResult.ciCompareOk; i++)
            {
                if (hash1[i] != hash2[i])
                    cr = CompareResult.ciPixelMismatch;
            }
            return cr;
        }
        private static CompareResult CompareImage(Bitmap bmp1, Bitmap bmp2)
        {
            //Test to see if we have the same size of image
            if (bmp1.Size != bmp2.Size)
            {
                return CompareResult.ciSizeMismatch;
            }
            else
            {
                //Convert each image to a byte array
                System.Drawing.ImageConverter ic =
                       new System.Drawing.ImageConverter();
                byte[] btImage2 = new byte[1];
                btImage2 = (byte[])ic.ConvertTo(bmp2, btImage2.GetType());

                return CompareImage(bmp1, btImage2);
            }
        }

        /// <summary>
        /// Author: Nguyen Duc Thuan
        /// Created on: 27/07/2011  12:33 SA
        /// Todo: Lay size anh
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        public static Size GetSizeImage(string path)
        {
            Bitmap img = new Bitmap(path, true);
            return img.Size;
        }
    }
}
