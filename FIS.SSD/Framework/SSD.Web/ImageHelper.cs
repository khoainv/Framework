using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Web;
using SSD.Framework;
using SSD.Framework.Extensions;
using SSD.Web.Extensions;

namespace SSD.Web
{
    public class ImageHelper : Singleton<ImageHelper>
    {
        public static readonly Size Icon = new Size(50, 50);
        public static readonly Size ProductList = new Size(200, 280);
        public static readonly Size ProductDetail = new Size(300, 300);
        public static readonly Size ProductDetailIcon = new Size(64, 42);


        public static readonly Size W64xH42 = new Size(64, 42);
        public static readonly Size W123xH64 = new Size(123, 64);
        public static readonly Size W300xH200 = new Size(300, 200);
        public static readonly Size W206xH100 = new Size(206, 100);
        public enum ResizeType
        {
            MaxPixels,
            FixWith,
            FixHight,
            Distortion,
            Background
        }

        public List<string> ResetImageOnUrlResize(List<string> urlImgOrgs, int width, int height, ResizeType type)
        {
            List<string> lst = new List<string>();
            foreach (string url in urlImgOrgs)
                lst.Add(GetImageUrlResize(url, width, height, type, true));
            return lst;
        }

        public List<string> ResetImageOnUrl(string relativePath, List<PictureCore> lstPic, string dirImg = "")
        {
            List<string> lst = new List<string>();
            foreach (PictureCore pic in lstPic)
                lst.Add(SavePictureInFile(relativePath, pic, dirImg, true));
            return lst;
        }
        public string GetImageUrlResize(string url, Size size, ResizeType type, bool isCompareIfExist = false)
        {
            return GetImageUrlResize(url, size.Width, size.Height, type, isCompareIfExist);
        }
        public void SaveImageStream(Stream stream, string savePath)
        {
            lock (s_lock)
            {
                using (var fileStream = new FileStream(savePath, FileMode.Create, FileAccess.Write))
                {
                    stream.CopyTo(fileStream);
                }
            }
        }
        public void SaveImageStreamResize(Stream stream, string savePath, int width, int height, ResizeType type)
        {
            Image image2, image3;
            image2 = Image.FromStream(stream);
            switch (type)
            {
                case ResizeType.FixWith:
                    image3 = image2.ResizeWithFixWidth(width);
                    break;
                case ResizeType.FixHight:
                    image3 = image2.ResizeWithFixHeight(height);
                    break;
                case ResizeType.MaxPixels:
                    image3 = image2.ResizeWithMaxPixels(Math.Max(width, height));
                    break;
                case ResizeType.Distortion:
                    image3 = image2.FixedSizeWithDistortion(width, height);
                    break;
                default:
                    image3 = image2.FixedSizeWithBackground(width, height);
                    break;
            }
            image3.Save(savePath);
            image3.Dispose();
            image2.Dispose();
        }
        public string GetImageUrlResize(string url, int width, int height, ResizeType type, bool isCompareIfExist = false)
        {
            Image image2,image3;
            if (!string.IsNullOrEmpty(url))
            {
                string path = (url.IndexOf("~/") == 0) ? url.MapPath() : ("~/" + url).MapPath();
                if (!File.Exists(path))
                {
                    url = url.Substring(0, url.LastIndexOf('/')) + "/noImage.jpg";
                }
                url = url[0] == '~' ? url.Remove(0, 1) : url;
                url = url[0] == '/' ? url.Remove(0, 1) : url;
                string urlResize = "/" + url.Insert(url.LastIndexOf('/') + 1, width + "_" + height + "/").Insert(url.IndexOf('/') + 1, "Resize/");

                string pathResize = urlResize.MapPath();
                if (!File.Exists(pathResize) || isCompareIfExist)
                {
                    CreateDirectoryResize(urlResize);
                    image2 = Image.FromFile(path);
                    switch(type){
                        case ResizeType.FixWith:
                            image3 = image2.ResizeWithFixWidth(width);
                            break;
                        case ResizeType.FixHight:
                            image3 = image2.ResizeWithFixHeight(height);
                            break;
                        case ResizeType.MaxPixels:
                            image3 = image2.ResizeWithMaxPixels(Math.Max(width, height));
                            break;
                        case ResizeType.Distortion:
                            image3 = image2.FixedSizeWithDistortion(width, height);
                            break;
                        default:
                            image3 = image2.FixedSizeWithBackground(width, height);
                            break;
                    }
                    if (File.Exists(pathResize) && Image.FromFile(pathResize).Compare(image3) == CompareResult.ciCompareOk)
                    {
                        image3.Dispose();
                        image2.Dispose();
                        return urlResize;
                    }
                    image3.Save(pathResize);
                    image3.Dispose();
                    image2.Dispose();
                }
                return urlResize;
            }
            return string.Empty;
        }
        private static readonly object s_lock = new object();
        public string SavePictureInFile(string relativePath, PictureCore pic, string dirImg="", bool isCompareIfExist = false)
        {
            string LocalImagePath = HttpContext.Current.Server.MapPath(relativePath + dirImg);
            string[] parts = pic.Extension.Split('/');
            string lastPart = parts[parts.Length - 1];
            switch (lastPart)
            {
                case "pjpeg":
                    lastPart = "jpg";
                    break;
                case "x-png":
                    lastPart = "png";
                    break;
                case "x-icon":
                    lastPart = "ico";
                    break;
            }
            string localFilename = string.Empty;
            localFilename = string.Format("{0}.{1}", pic.FileName, lastPart);

            lock (s_lock)
            {
                if (!System.IO.Directory.Exists(LocalImagePath))
                {
                    System.IO.Directory.CreateDirectory(LocalImagePath);
                }
                if (File.Exists(Path.Combine(LocalImagePath, localFilename)))
                {
                    if (isCompareIfExist && Image.FromFile(Path.Combine(LocalImagePath, localFilename)).Compare(pic.PictureBinary) != CompareResult.ciCompareOk)
                        File.WriteAllBytes(Path.Combine(LocalImagePath, localFilename), pic.PictureBinary);
                    return localFilename;
                }
                File.WriteAllBytes(Path.Combine(LocalImagePath, localFilename), pic.PictureBinary);
                return localFilename;
            }
        }
        public string GetImageExtension(string mimeType)
        {
            string[] parts = mimeType.Split('/');
            string lastPart = parts[parts.Length - 1];
            switch (lastPart)
            {
                case "pjpeg":
                    lastPart = "jpg";
                    break;
                case "x-png":
                    lastPart = "png";
                    break;
                case "x-icon":
                    lastPart = "ico";
                    break;
            }
            return lastPart;
        }
        public bool CheckExistImage(string relativePath, string dirImg, string fileName, string mimeType)
        {
            string LocalImagePath = HttpContext.Current.Server.MapPath(relativePath + dirImg);
            string[] parts = mimeType.Split('/');
            string lastPart = parts[parts.Length - 1];
            switch (lastPart)
            {
                case "pjpeg":
                    lastPart = "jpg";
                    break;
                case "x-png":
                    lastPart = "png";
                    break;
                case "x-icon":
                    lastPart = "ico";
                    break;
            }
            string localFilename = string.Empty;
            localFilename = string.Format("{0}.{1}", fileName, lastPart);
            return File.Exists(Path.Combine(LocalImagePath, localFilename));
        }

        #region Private
        private string CreateDirectoryResize(string path)
        {
            if (!string.IsNullOrEmpty(path))
            {
                string sPath = string.Empty;
                string d = string.Empty;
                string[] aStr = path.Split('/');
                for (int i = 1; i < aStr.Length - 1; i++)
                {
                    sPath = sPath + "/" + aStr[i];
                    d = sPath.MapPath();
                    if (!Directory.Exists(d))
                        Directory.CreateDirectory(d);
                }
                return sPath;
            }
            return string.Empty;
        }
        #endregion
    }
}