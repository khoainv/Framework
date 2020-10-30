using System;
namespace SSD.Web
{
    public partial class PictureCore
    {
        public PictureCore() { }
        public PictureCore(int picID, string picFileName, byte[] picBinary, string ext)
        {
            PictureID = picID;
            FileName = picFileName;
            PictureBinary = picBinary;
            Extension = ext;
        }
        public PictureCore(int picID,string pathFile, string picFileName, byte[] picBinary, string ext, string displayName="",string description="")
        {
            PictureID = picID;
            FileName = picFileName;
            PictureBinary = picBinary;
            Extension = ext;
            DisplayName = displayName;
            Description = description;

            ImgUrl = string.Format("{0}{1}.{2}", pathFile, picFileName, Extension);
        }
        public PictureCore(int picID, string imgUrl, string displayName = "", string description = "")
        {
            ImgUrl = imgUrl;
            DisplayName = displayName;
            Description = description;
        }
        public int PictureID { get; set; }
        public string Extension { get; set; }
        public string FileName { get; set; }
        public bool IsNew { get; set; }
        public Byte[] PictureBinary { get; set; }

        public string ImgUrl { get; set; }
        public string ImgUrlThumb { get; set; }
        public string ImgUrlThumb_W64xH42 { get; set; }
        public string ImgUrlThumb_W300xH200 { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public string LinkReference { get; set; }
    }
}
