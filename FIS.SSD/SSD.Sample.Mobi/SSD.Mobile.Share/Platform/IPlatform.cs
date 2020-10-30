using SSD.Mobile.Common;
using System;
using SSD.Framework;

namespace SSD.Mobile.Share
{
    public interface IPlatform
    {
        TargetPlatform OS { get; }

        string DatabasePath { get; }

        //TODO: Step 2b - Add ISQLitePlatform to IPlatform.
        //ISQLitePlatform SqlitePlatform { get; }
    }
}

