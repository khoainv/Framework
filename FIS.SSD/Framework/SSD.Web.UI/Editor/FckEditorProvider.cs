using System;
using System.Text;
using System.Configuration;
using System.Configuration.Provider;

namespace SSD.Web.UI.Editor
{
    public class FckEditorProvider : EditorProvider
    {
        public override string Render(string name)
        {
            StringBuilder sb = new StringBuilder(@"<script type='text/javascript' src='/Scripts/Editors/fckeditor/fckeditor.js'></script>");
            sb.Append(@"<script type='text/javascript' src='/Scripts/Editors/fckeditorapi.js'></script>")
            .Append(string.Format(@"<textarea id='{0}' name='{0}' cols='30' rows='10'></textarea>",name))
            .Append(@"<script type='text/javascript'>
                window.onload = function() {
                var oFCKeditor = new FCKeditor('").Append(name).Append(@"');
                oFCKeditor.BasePath = '/Scripts/Editors/fckeditor/';
                oFCKeditor.Height = 300;
                oFCKeditor.ReplaceTextarea();}
                </script>");
            return sb.ToString();
        }
        public override string Render(string name,string pathFile)
        {
            return Render(name);
        }
    }
}
