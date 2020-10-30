using System;

namespace SSD.Framework.Security
{
    public class UGPermissionAttribute : Attribute
    {
        public string Key { get; set; }
        public string Name { get; set; }
        public string GroupKey { get; set; }
    }
    public class UGFollowPermissionAttribute : Attribute
    {
        public string FollowKey { get; set; }
    }
}
