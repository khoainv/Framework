using System;
using System.Configuration;
using System.Configuration.Provider;

namespace SSD.Web.UI.Editor
{
    public class EditorProviderCollection : System.Configuration.Provider.ProviderCollection
    {
        public new EditorProvider this[string name]
        {
            get { return (EditorProvider)base[name]; }
        }

        public override void Add(ProviderBase provider)
        {
            if (provider == null)
                throw new ArgumentNullException("provider");

            if (!(provider is EditorProvider))
                throw new ArgumentException
                    ("Invalid provider type", "provider");

            base.Add(provider);
        }
    }

    public abstract class EditorProvider : ProviderBase
    {

        private const string _providerType = "EditorProvider";

        //private static volatile EditorProvider _provider = null;
        private static object padLock = new object();

        public abstract string Render(string name);
        public abstract string Render(string name,string pathFile);
    }
}
