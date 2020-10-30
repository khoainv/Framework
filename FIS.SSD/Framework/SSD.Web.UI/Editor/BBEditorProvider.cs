using System;
using System.Text;
using System.Configuration;
using System.Configuration.Provider;

namespace SSD.Web.UI.Editor
{
    public class BBEditorProvider : EditorProvider
    {
        public override string Render(string name)
        {
            StringBuilder sb = new StringBuilder(@"<script language='javascript' type='text/javascript' >
                    var webRoot = '/Scripts/';
                    edToolbar('{0}'); 
                    </script>
                    <script src='/Scripts/Editors/BBEditor/ed.js' type='text/javascript' ></script>
                    <textarea id='{0}' name='{0}' cols='30' rows='10'></textarea>");
            return string.Format(sb.ToString(),name);
            
        }
        public override string Render(string name, string pathFile)
        {
            return Render(name);
        }
    }
}
