using System;
using System.Configuration;
using System.Configuration.Provider;
using System.Web.Configuration;
using System.Collections.Generic;

namespace SSD.Web.UI.Editor
{
    public class EditorService : MarshalByRefObject
    {
        #region Provider-specific bits
        private static EditorProvider _provider = null;
        private static EditorProviderCollection _providers = null;
        private static object _lock = new object();

        public static EditorProvider Provider
        {
            get
            {
                LoadProviders();
                return _provider;
            }
        }

        public EditorProviderCollection Providers
        {
            get 
            {
                LoadProviders();
                return _providers; 
            }
        }
        private static EditorService service;
        private static EditorService Instance
        {
            get
            {
                if (service == null)
                {
                    lock (_lock)
                    {
                        service = new EditorService();
                        return service;
                    }
                }
                return service;
            }
        }
        private static void LoadProviders()
        {
            // Avoid claiming lock if providers are already loaded
            if (_provider == null)
            {
                lock (_lock)
                {
                    // Do this again to make sure _provider is still null
                    if (_provider == null)
                    {
                        // Get a reference to the <imageService> section
                        EditorServiceSection section = (EditorServiceSection)
                            WebConfigurationManager.GetSection
                            ("EditorService");
                        if (section != null)
                        {
                            if (_providers == null)
                            {
                                listProviderName = new List<string>();
                                foreach (ProviderSettings ps in section.Providers)
                                    listProviderName.Add(ps.Name);
                                // Load registered providers and point _provider
                                // to the default provider
                                _providers = new EditorProviderCollection();
                                ProvidersHelper.InstantiateProviders
                                    (section.Providers, _providers,
                                    typeof(EditorProvider));
                            }
                            _provider = _providers[section.DefaultProvider];
                        }

                        if (_provider == null)
                            throw new ProviderException
                                ("Unable to load default EditorProvider");
                    }
                }
            }
        }
        #endregion

        private static List<string> listProviderName;
        public static List<string> ListProviderName
        {
            get
            {
                if (_provider == null)
                {
                    lock (_lock)
                    {
                        EditorServiceSection section = (EditorServiceSection)
                            WebConfigurationManager.GetSection
                            ("EditorService");
                        if (section != null && _providers == null)
                        {
                            listProviderName = new List<string>();
                            foreach (ProviderSettings ps in section.Providers)
                                listProviderName.Add(ps.Name);
                        }
                    }
                }
                return listProviderName;
            }
        }

        public static string Render(string name)
        {
            return Provider.Render(name);
        }
        public static string Render(string providerName, string name)
        {
            return Instance.Providers[providerName].Render(name);
        }
        public static string CodeRender(string providerName, string name,string pathFile)
        {
            return Instance.Providers[providerName].Render(name, pathFile);
        }
    }
}
