using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SSD.Web.UI.Ribbon
{
    public enum LevelType : int
    {
        Tab = 0,
        Group = 1,
        ItemList = 2,
        ItemMenu = 3
        //Item = 4
    }
    public class MenuRibbon
    {
        public int ID { get; set; }
        public int ParentID { get; set; }
        public LevelType Type { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
        public bool Status { get; set; }
        public int DisplayOrder { get; set; }

        /// <summary>
        /// Tab => Items=null
        /// </summary>
        public List<MenuItem> Items { get; set; }
    }
}
