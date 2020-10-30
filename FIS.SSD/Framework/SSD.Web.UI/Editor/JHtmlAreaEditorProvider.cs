using System;
using System.Text;
using System.Configuration;
using System.Configuration.Provider;

namespace SSD.Web.UI.Editor
{
    public class JHtmlAreaEditorProvider : EditorProvider
    {
        public override string Render(string name)
        {
            StringBuilder sb = new StringBuilder(@"
             <script type='text/javascript' src='/Scripts/Editors/jHtmlArea/jHtmlArea-0.7.0.js'></script>
              <link rel='Stylesheet' type='text/css' href='/Scripts/Editors/jHtmlArea/style/jHtmlArea.css' />
                <script type='text/javascript'>
                    $(function() {
                        $('#" + name + @"').htmlarea(); // Initialize jHtmlArea's with all default values
                    });
                </script>")
                          .Append(string.Format("<textarea id='{0}' name='{0}' cols='30' rows='10'></textarea>", name));
            return sb.ToString();
        }
        public override string Render(string name, string pathFile)
        {
            return Render(name);
        }
    }
}
