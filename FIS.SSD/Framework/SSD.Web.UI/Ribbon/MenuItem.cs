using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SSD.Web.UI.Ribbon
{
    public enum ActionType : int
    {
        Link = 0,
        Ajax = 1,
        Dialog = 2,
        Lookup = 3
    }
    public class MenuItem
    {
        public int ID { get; set; }
        public int ParentID { get; set; }
        public string Name { get; set; }
        public string Alias { get; set; }
        public string Icon { get; set; }
        public bool Status { get; set; }
        public int DisplayOrder { get; set; }

        public MenuItem Parent { get; set; }
        public List<MenuItem> Children { get; set; }

        public ActionType ActionType { get; set; }
        
        /// <summary>
        /// Nếu ActionType = Link thì Url = Controller/Action hoặc Ext Url
        /// Nếu ActionType = Ajax thì Url = Controller/Action
        /// Nếu ActionType = Dialog thì Url = id của thẻ Div popup
        /// Nếu ActionType = Lookup thì Url = id của thẻ Div popup
        /// </summary>
        public string URLOrDiv { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
        /// <summary>
        /// Vùng chứa dữ liệu Input
        /// </summary>
        public string AreaContainerID { get; set; }
        /// <summary>
        /// Nếu ActionType = Link (Url?Key1=val1&Key2=val2)
        /// (Nếu ActionType = Ajax key <=> controlID, val = default)
        /// Nếu ActionType = Dialog key <=> controlID, val = default => Get value show Dialog và Set val vào controlID
        /// Nếu ActionType = Lookup key <=> controlID, val = default => Get value show Lookup và Set val vào controlID
        /// </summary>
        public Dictionary<string, string> Paras { get; set; }
        public Dictionary<string, string> DefineMapPopupControlID { get; set; }
        public List<string> MCEEditorFields { get; set; }
        /// <summary>
        /// Nếu ActionType = Ajax key <=> controlID, val = default => Get value sau thực hiện action và set val vào controlID
        /// Nếu ActionType = Dialog key <=> controlID, val = default => Get value từ Form Dialog tới Set val vào controlID và close form
        /// Nếu ActionType = Lookup key <=> controlID, val = default => Get value từ Form Lookup tới Set val vào controlID và close form
        /// </summary>
        public Dictionary<string, string> GetValueAfterAction { get; set; }

        /// <summary>
        /// /Controller/Action/
        /// </summary>
        public List<string> EnableActionOnViews { get; set; }

        public string Shortcut { get; set; }
    }
}
