using System;
using System.Text;
using System.Configuration;
using System.Configuration.Provider;
using System.Collections.Generic;
using System.Web;
using SSD.Framework.Extensions;

namespace SSD.Web.UI.Editor
{
    public class CodeEditorProvider : EditorProvider
    {
        public override string Render(string name, string pathFile)
        {
            if (string.IsNullOrEmpty(pathFile))
                return string.Empty;
            StringBuilder sb = new StringBuilder(@"<script type='text/javascript' src='/scripts/editors/edit_area/edit_area_full.js'></script>
            <script type='text/javascript'>
            editAreaLoader.init({
            id: '"+name+@"',
            start_highlight: true,
            syntax: '").Append((new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase) {
                // Ouch, but this will do for now...
                {".cs", "c"},
                {".js", "js"},
                {".vb", "vb"},
                {".rb", "ruby"},
                {".py", "python"},
                {".htm", "html"},
                {".html", "html"},
                {".aspx", "html"},
                {".ascx", "html"},
                {".master", "html"},
                {".xml", "xml"},
                {".css", "css"}
            })[System.IO.Path.GetExtension(pathFile)])
            .Append(@"' });</script>
            <textarea name='"+name+"' id='"+name+"'>").Append(HttpContext.Current.Server.MapPath(pathFile).GetFileText()).Append(@"</textarea>");
            return sb.ToString();
        }
        public override string Render(string name)
        {
            return Render(name, null);
        }
    }
}
