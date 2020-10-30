using System;
using System.Text;
using System.Configuration;
using System.Configuration.Provider;

namespace SSD.Web.UI.Editor
{
    public class CKEditorProvider : EditorProvider
    {
        public override string Render(string name)
        {
            StringBuilder sb = new StringBuilder(@"<script type='text/javascript' src='/Scripts/Editors/ckeditor/ckeditor.js'></script>
                    <textarea id='{0}' name='{0}' cols='30' rows='10'></textarea>
                    <script type='text/javascript'>
                    CKEDITOR.replace('{0}');
	                </script>");
            return string.Format(sb.ToString(),name);
        }
        public override string Render(string name, string pathFile)
        {
            return Render(name);
        }
    }
}
