using SSD.Mobile.Common;
using SSD.Mobile.Share;
using System;
using System.IO;
using Windows.Storage;
using SSD.Framework;
//using SQLite.Net.Interop;

namespace SSD.Mobile.WP
{
    public class WPPlatform : IPlatform
    {
        const string sqliteFilename = "FISInsight.db3";
        public TargetPlatform OS { get { return TargetPlatform.WinPhone; } }
        public string DatabasePath
        {
            get
            {
                return Path.Combine(ApplicationData.Current.LocalFolder.Path, sqliteFilename);
            }
        }

        //public ISQLitePlatform SqlitePlatform
        //{
        //    get { return new SQLite.Net.Platform.WindowsPhone8.SQLitePlatformWP8(); }
        //}
    }
}

