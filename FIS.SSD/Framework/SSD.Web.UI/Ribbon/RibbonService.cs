using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace SSD.Web.UI.Ribbon
{
    public class RibbonService
    {
        public static string RenderMenu(HtmlHelper html, List<MenuItem> sysMenu, List<MenuRibbon> menu)
        {
            //StringBuilder str = new StringBuilder();
            //if (html.ViewContext.HttpContext.Session["RibbonService.RenderMenu"] == null)
            //{
            //    str.Append(string.Format(@"<div class='mainContainer'><ul class='ribbon'><li>{0}</li><li>{1}</li><li><div class='hide-show-menu' active='true'></div></li></ul></div>", RenderMenuTree(sysMenu), RenderMenuRibbon(html, menu)));
            //    html.ViewContext.HttpContext.Session["RibbonService.RenderMenu"] = str;
            //    return str.ToString();
            //}

            //return html.ViewContext.HttpContext.Session["RibbonService.RenderMenu"].ToString();
            StringBuilder str = new StringBuilder();
            str.Append(string.Format(@"<div class='menuContainer'><ul class='ribbon'><li>{0}</li><li>{1}</li><li><div class='pin-unpin-ribbonmenu' active='true' ></div></li><li><div class='hide-show-menu' active='true'></div></li><li><a target='_blank' class='ug-logo-menu' href='http://www.ugroup.com.vn' ><img alt='UG Corporation' src='/Content/Images/logo_UGSoft.png' ></a></li></ul></div>", RenderMenuTree(sysMenu), RenderMenuRibbon(html, menu)));
            return str.ToString();
        }

        public static string RenderMenuTree(List<MenuItem> menu)
        {
            StringBuilder str = new StringBuilder(@"<ul class='orb'><li><a href='javascript:void(0);' accesskey='1' class='orbButton'>&nbsp;</a><span>Menu</span><ul style='display: none;'>");
            List<MenuItem> level1s = (from m in menu where m.ParentID == 0 orderby m.DisplayOrder select m).ToList();
            foreach (MenuItem level1 in level1s)
            {
                str.AppendFormat(@"<li><a href='{0}'><img src='{1}' alt='{2}' /><span>{2}</span></a>", level1.URLOrDiv, level1.Icon, level1.Name);

                List<MenuItem> level2s = (from m in menu where m.ParentID == level1.ID orderby m.DisplayOrder select m).ToList();
                str = level2s.Count == 0 ? str : str.Append(@"<ul  style='display: none;'>");
                foreach (MenuItem level2 in level2s)
                {
                    //Render item
                    str.AppendFormat(@"<li><a href='{0}'><img src='{1}' alt='{2}' /><span>{2}</span></a></li>", level2.URLOrDiv, level2.Icon, level2.Name);
                }
                //End Tab
                str = level2s.Count == 0 ? str.Append(@"</li>") : str.Append(@"</ul></li>");
            }
            //End MenuItem
            return str.Append(@"</ul></li></ul>").ToString();
        }

        public static string RenderMenuRibbon(HtmlHelper html, List<MenuRibbon> menu)
        {
            StringBuilder str = new StringBuilder(@"<ul class='menu'>");
            List<MenuRibbon> tabs = (from m in menu where m.Type == LevelType.Tab orderby m.DisplayOrder select m).ToList();
            foreach (MenuRibbon tab in tabs)
            {
                List<MenuRibbon> groups = (from m in menu where m.ParentID == tab.ID orderby m.DisplayOrder select m).ToList();
                if (groups.Count > 0)
                {
                    str.AppendFormat(@"<li><a class='tab-item' href='#tab{0}' accesskey='{0}'>{1}</a><ul style='display: none;'>", tab.ID, tab.Name);
                    foreach (MenuRibbon group1 in groups)
                    {
                        if (group1.Type == LevelType.Group)
                        {
                            List<MenuRibbon> itemLists = (from m in menu where m.ParentID == group1.ID orderby m.DisplayOrder select m).ToList();
                            if (itemLists.Count > 0 || (group1.Items != null && group1.Items.Count > 0))
                            {
                                str.AppendFormat(@"<li><h2><span>{0}</span></h2>", group1.Name);
                                if(group1.Items != null)
                                foreach (MenuItem item in group1.Items)
                                {
                                    //Check actionType
                                    switch (item.ActionType)
                                    {
                                        case ActionType.Ajax:
                                        case ActionType.Dialog:
                                        case ActionType.Lookup:
                                            str.Append(RenderAction(html,group1.Type, item));
                                            break;
                                        default:
                                            //Render item
                                            str.AppendFormat(@"<div><a class='menu-item' href='{0}'><img src='{1}' alt='{2}'/>{2}</a></div>", item.URLOrDiv, item.Icon, item.Name);
                                            break;
                                    }
                                }
                                foreach (MenuRibbon itemList in itemLists)
                                {
                                    if (itemList.Type == LevelType.ItemList)
                                    {
                                        List<MenuRibbon> itemMenus = (from m in menu where m.ParentID == itemList.ID orderby m.DisplayOrder select m).ToList();
                                        if (itemMenus.Count > 0 || (itemList.Items != null && itemList.Items.Count > 0))
                                        {
                                            str.Append(@"<div class='ribbon-list'>");
                                            if (itemList.Items != null)
                                            foreach (MenuItem item in itemList.Items)
                                            {
                                                //Check actionType
                                                switch (item.ActionType)
                                                {
                                                    case ActionType.Ajax:
                                                    case ActionType.Dialog:
                                                    case ActionType.Lookup:
                                                        str.Append(RenderAction(html,itemList.Type, item));
                                                        break;
                                                    default:
                                                        //Render item
                                                        str.AppendFormat(@"<div><a class='menu-item' href='{0}'><img src='{1}' alt='{2}'/>{2}</a></div>", item.URLOrDiv, item.Icon, item.Name);
                                                        break;
                                                }
                                            }
                                            foreach (MenuRibbon itemMenu in itemMenus)
                                            {
                                                if (itemMenu.Type == LevelType.ItemMenu && itemMenu.Items != null && itemMenu.Items.Count>0)
                                                {
                                                    str.AppendFormat(@"<div><img src='{0}' alt='{1}'/>{1}<ul class='ribbon-theme'>", itemMenu.Icon, itemMenu.Name);
                                                    foreach (MenuItem item in itemMenu.Items)
                                                    {
                                                        //Check actionType
                                                        switch (item.ActionType)
                                                        {
                                                            case ActionType.Ajax:
                                                            case ActionType.Dialog:
                                                            case ActionType.Lookup:
                                                                str.Append(RenderAction(html,itemMenu.Type, item));
                                                                break;
                                                            default:
                                                                //Render item
                                                                str.AppendFormat(@"<li class='ribbon-windows7'><a class='menu-item' href='{0}'>{1}</a></li>", item.URLOrDiv, item.Name);
                                                                break;
                                                        }
                                                    }
                                                    //End Menu
                                                    str.Append(@"</ul></div>");
                                                }
                                            }
                                            //End List
                                            str.Append(@"</div>");
                                        }
                                    }
                                }
                                //End Group
                                str.Append(@"</li>");
                            }
                        }
                    }
                    //End Tab
                    str.Append(@"</ul></li>");
                }
            }
            //End MenuRibbon
            return str.Append(@"</ul>").ToString();
        }

        private static bool IsEnableAction(HtmlHelper html, MenuItem item)
        {
            bool b = false;
            if (item.EnableActionOnViews == null || item.EnableActionOnViews.Count == 0)
                return b;

            foreach(var enable in item.EnableActionOnViews)
            {
                b = enable.Trim().ToLower() == "/" + html.ViewContext.RouteData.Values["controller"].ToString().Trim().ToLower()
                    + "/" + html.ViewContext.RouteData.Values["action"].ToString().Trim().ToLower() + "/";
                if (b)
                    break;
            }
            return b;
        }

        private static string RenderAction(HtmlHelper html, LevelType levelType, MenuItem item)
        {
            if (levelType != LevelType.Group && levelType != LevelType.ItemList && levelType != LevelType.ItemMenu)
            {
                return string.Empty;
            }
            string id = Guid.NewGuid().ToString();
            StringBuilder script = new StringBuilder();

            if (!IsEnableAction(html,item))
            {
                if (levelType == LevelType.Group || levelType == LevelType.ItemList)
                {
                    script = new StringBuilder(string.Format("<div id='btn{0}' class='disable'><a class='menu-item disable' href='javascript:void(0);'><img src='{1}' alt='{2}'/>{3}</a></div>", id, item.Icon, item.Name, AddShortcutToName(item.Shortcut, item.Name)));
                }
                else
                {
                    script = new StringBuilder(string.Format("<li class='ribbon-itemmenu disable' id='btn{0}' ><a class='menu-item' href='javascript:void(0);'>{1}</a></li>", id, AddShortcutToName(item.Shortcut, item.Name)));
                }
                return script.ToString();
            }

            if (levelType == LevelType.Group || levelType == LevelType.ItemList)
            {
                script = new StringBuilder(string.Format("<div id='btn{0}' ><a class='menu-item disable' href='javascript:void(0);'><img src='{1}' alt='{2}'/>{3}</a><script type=\"text/javascript\">", id, item.Icon, item.Name, AddShortcutToName(item.Shortcut, item.Name)));
            }
            else
            {
                script = new StringBuilder(string.Format("<li class='ribbon-itemmenu' id='btn{0}' ><a class='menu-item' href='javascript:void(0);'>{1}</a><script type=\"text/javascript\">", id, AddShortcutToName(item.Shortcut, item.Name)));
            }

            script.AppendFormat("$('#btn{0} .menu-item').click(function () ", id)
                .Append("{");
            switch(item.ActionType)
            {
                case ActionType.Ajax:
                    script.Append(RenderAjaxScript(item));
                    break;
                case ActionType.Dialog:
                    script.Append(RenderDialogScript(item));
                    break;
                case ActionType.Lookup:
                    script.Append(RenderLookupScript(item));
                    break;
            }
            script.AppendLine("});");


            if (!string.IsNullOrWhiteSpace(item.Shortcut))
            {
                script.Append("$.Shortcuts.add({ type: 'down', mask: '" + item.Shortcut + "', handler: function() { ");
                switch (item.ActionType)
                {
                    case ActionType.Ajax:
                        script.Append(RenderAjaxScript(item));
                        break;
                    case ActionType.Dialog:
                        script.Append(RenderDialogScript(item));
                        break;
                    case ActionType.Lookup:
                        script.Append(RenderLookupScript(item));
                        break;
                }
                script.AppendLine("} }); ")
                .AppendLine("$.Shortcuts.start(); ");
            }

            if (levelType == LevelType.Group || levelType == LevelType.ItemList)
            {
                script.Append("</script></div>");
            }
            else
            {
                script.Append("</script></li>");
            }
            return script.ToString();
        }

        private static string RenderAjaxScript(MenuItem item)
        {
            StringBuilder script = new StringBuilder();

            //if (item.Paras != null&&item.Paras.Count>0)
            //{
            //    script.Append("\r\n $('#Description').val(tinyMCE.get('Description').getContent()); var dataVals = {};");
            //    foreach (var para in item.Paras)
            //    {
            //        script.Append(string.Format("\r\n dataVals['{0}'] = $(\"{2}#{0}\").val() == null ? {1} : $(\"{2}#{0}\").val(); ", para.Key, string.IsNullOrWhiteSpace(para.Value) ? "''" : para.Value, string.IsNullOrWhiteSpace(item.AreaContainerID) ? string.Empty : "#" + item.AreaContainerID + " "));
            //    }
            //}

            if(item.MCEEditorFields!=null)
                foreach(string field in item.MCEEditorFields)
                    script.AppendFormat("\r\n$('#{0}').val(tinyMCE.get('{0}').getContent()); \r\n ", field);

            script.AppendLine("$.ajax({ ")
                .AppendLine("type: \"POST\", ")
                .AppendFormat("url: \"{0}\", \r\n", item.URLOrDiv)
                .AppendFormat("data: $(\"{0}form\").serialize(),\r\n", string.IsNullOrWhiteSpace(item.AreaContainerID) ? string.Empty : "#" + item.AreaContainerID + " ") //$(\"form\").serializeArray(),") //serialize()    //dataVals,")
                .AppendLine("success: function(msg){")
                .AppendLine("alert( \"Data Saved: \" + msg );")
                .AppendLine("} }); ");

            return script.ToString();
        }

        private static string RenderDialogScript(MenuItem item)
        {
            StringBuilder script = new StringBuilder();

            if (item.MCEEditorFields != null)
                foreach (string field in item.MCEEditorFields)
                    script.AppendFormat("\r\n$('#{0}').val(tinyMCE.get('{0}').getContent()); ", field);

            if (item.Paras != null && item.Paras.Count > 0
                && item.DefineMapPopupControlID != null && item.DefineMapPopupControlID.Count > 0)
            {
                foreach (var para in item.Paras)
                {
                    if (!string.IsNullOrWhiteSpace(item.DefineMapPopupControlID[para.Key]))
                        script.AppendFormat("\r\n$(\"#{0} #{1}\").val($(\"{2}#{3}\").val() == null ? '{4}' : $(\"{2}#{3}\").val()); ",
                            item.URLOrDiv, item.DefineMapPopupControlID[para.Key],
                            string.IsNullOrWhiteSpace(item.AreaContainerID) ? string.Empty : "#" + item.AreaContainerID + " ",
                            para.Key, string.IsNullOrWhiteSpace(para.Value) ? "" : para.Value);
                }
            }

            script//.AppendLine("\r\n$(\"#dialog:ui-dialog\").dialog( \"destroy\" ); ")
                .AppendFormat("$(\"#{0}\").dialog(",item.URLOrDiv)
                .Append("{")
                .AppendFormat("width: {0},",item.Width==0?"'auto'":item.Width.ToString())
                .AppendFormat("height: {0},", item.Height == 0 ? "'auto'" : item.Height.ToString())
                .Append("modal: true, ")
                .Append("close: function() { $(this).dialog('destroy');	} ")
                .Append("}); ");

            return script.ToString();
        }

        private static string RenderLookupScript(MenuItem item)
        {
            StringBuilder script = new StringBuilder();

            if (item.MCEEditorFields != null)
                foreach (string field in item.MCEEditorFields)
                    script.AppendFormat("\r\n$('#{0}').val(tinyMCE.get('{0}').getContent()); ", field);

            if (item.Paras != null && item.Paras.Count > 0
                && item.DefineMapPopupControlID != null && item.DefineMapPopupControlID.Count > 0)
            {
                foreach (var para in item.Paras)
                {
                    if (item.DefineMapPopupControlID.Keys.Contains(para.Key) && !string.IsNullOrWhiteSpace(item.DefineMapPopupControlID[para.Key]))
                        script.AppendFormat("\r\n$(\"#{0} #{1}\").val($(\"{2}#{3}\").val() == null ? '{4}' : $(\"{2}#{3}\").val()); ",
                            item.URLOrDiv, item.DefineMapPopupControlID[para.Key],
                            string.IsNullOrWhiteSpace(item.AreaContainerID) ? string.Empty : "#" + item.AreaContainerID + " ",
                            para.Key, string.IsNullOrWhiteSpace(para.Value) ? "" : para.Value);
                }
            }

            script//.AppendLine("\r\n$(\"#dialog:ui-dialog\").dialog( \"destroy\" ); ")
                .AppendFormat("$(\"#{0}\").dialog(", item.URLOrDiv)
                .Append("{")
                .AppendFormat("width: {0},", item.Width == 0 ? "'auto'" : item.Width.ToString())
                .AppendFormat("height: {0},", item.Height == 0 ? "'auto'" : item.Height.ToString())
                .Append("modal: true, ")
                .AppendLine("buttons: { \"Ok\": function () {");

            if (item.GetValueAfterAction != null && item.GetValueAfterAction.Count > 0
                && item.DefineMapPopupControlID != null && item.DefineMapPopupControlID.Count > 0)
            {
                foreach (var val in item.GetValueAfterAction)
                {
                    if (item.DefineMapPopupControlID.Keys.Contains(val.Key) && !string.IsNullOrWhiteSpace(item.DefineMapPopupControlID[val.Key]))
                        script.AppendFormat("$(\"{0}#{1}\").val($(\"#{2} #{3}\").val() == null ? '{4}' : $(\"#{2} #{3}\").val()); \r\n",
                            string.IsNullOrWhiteSpace(item.AreaContainerID) ? string.Empty : "#" + item.AreaContainerID + " ",
                            item.DefineMapPopupControlID[val.Key], item.URLOrDiv, 
                            val.Key, string.IsNullOrWhiteSpace(val.Value) ? "" : val.Value);
                }
            }

            script.AppendLine("$(this).dialog(\"close\");}, ")
                .AppendLine("Cancel: function () { $(this).dialog(\"close\"); } }, ")
                .AppendLine("close: function() { $(this).dialog('destroy');	} ")
                .AppendLine("}); ");

            return script.ToString();
        }

        private static string AddShortcutToName(string shortcut, string name)
        {
            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(shortcut))
                return string.Empty;
            List<char> lst = GetCharOnShortcut(shortcut);

            foreach (char c in lst)
                name = AddShortcut(c, name);
            return name;
        }
        private static string AddShortcut(char c, string name)
        {
            int i = name.IndexOf(c);
            if (i >= 0)
            {
                name = name.Remove(i, 1);
                name = name.Insert(i, string.Format("<span style='text-decoration: underline;' >{0}</span>", c));
            }
            return name;
        }
        private static List<char> GetCharOnShortcut(string shortcut)
        {
            List<char> lst = new List<char>();
            string[] aStr = shortcut.Split(new char[] { '+' });

            foreach (string str in aStr)
                if (str.Trim().Length == 1)
                    lst.Add(str.Trim()[0]);

            return lst;
        }
    }
}
