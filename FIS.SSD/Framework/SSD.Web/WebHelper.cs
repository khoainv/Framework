using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;

namespace SSD.Web
{
    public class WebHelper
    {
        /// <summary>
        /// Maps a virtual path to a physical disk path.
        /// </summary>
        /// <param name="path">The path to map. E.g. "~/bin"</param>
        /// <returns>The physical path. E.g. "c:\inetpub\wwwroot\bin"</returns>
        public virtual string MapPath(string path)
        {
            if (HostingEnvironment.IsHosted)
            {
                //hosted
                return HostingEnvironment.MapPath(path);
            }

            //not hosted. For example, run in unit tests
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            path = path.Replace("~/", "").TrimStart('/').Replace('/', '\\');
            return Path.Combine(baseDirectory, path);
        }

        private static AspNetHostingPermissionLevel? _trustLevel;
        /// <summary>
        /// Finds the trust level of the running application (http://blogs.msdn.com/dmitryr/archive/2007/01/23/finding-out-the-current-trust-level-in-asp-net.aspx)
        /// </summary>
        /// <returns>The current trust level.</returns>
        public static AspNetHostingPermissionLevel GetTrustLevel()
        {
            if (!_trustLevel.HasValue)
            {
                //set minimum
                _trustLevel = AspNetHostingPermissionLevel.None;

                //determine maximum
                foreach (AspNetHostingPermissionLevel trustLevel in new[] {
                                AspNetHostingPermissionLevel.Unrestricted,
                                AspNetHostingPermissionLevel.High,
                                AspNetHostingPermissionLevel.Medium,
                                AspNetHostingPermissionLevel.Low,
                                AspNetHostingPermissionLevel.Minimal
                            })
                {
                    try
                    {
                        new AspNetHostingPermission(trustLevel).Demand();
                        _trustLevel = trustLevel;
                        break; //we've set the highest permission we can
                    }
                    catch (System.Security.SecurityException)
                    {
                        continue;
                    }
                }
            }
            return _trustLevel.Value;
        }
        protected virtual bool TryWriteWebConfig()
        {
            try
            {
                // In medium trust, "UnloadAppDomain" is not supported. Touch web.config
                // to force an AppDomain restart.
                File.SetLastWriteTimeUtc(MapPath("~/web.config"), DateTime.UtcNow);
                return true;
            }
            catch
            {
                return false;
            }
        }
        protected virtual bool TryWriteGlobalAsax()
        {
            try
            {
                //When a new plugin is dropped in the Plugins folder and is installed into nopCommerce, 
                //even if the plugin has registered routes for its controllers, 
                //these routes will not be working as the MVC framework couldn't 
                //find the new controller types and couldn't instantiate the requested controller. 
                //That's why you get these nasty errors 
                //i.e "Controller does not implement IController".
                //The issue is described here: http://www.nopcommerce.com/boards/t/10969/nop-20-plugin.aspx?p=4#51318
                //The solution is to touch global.asax file
                File.SetLastWriteTimeUtc(MapPath("~/global.asax"), DateTime.UtcNow);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public static string GetRootSite()
        {
            var httpContext = new System.Web.HttpContextWrapper(HttpContext.Current);
            return GetRootSite(httpContext);
        }
        public static string GetRootSite(HttpContextBase httpContext)
        {
            var request = httpContext.Request;
            var appUrl = HttpRuntime.AppDomainAppVirtualPath;

            if (appUrl != "/") appUrl += "/";

            var baseUrl = string.Format("{0}://{1}{2}", request.Url.Scheme, request.Url.Authority, appUrl);
            return baseUrl;
        }
        /// <summary>
        /// Restart application domain
        /// </summary>
        /// <param name="makeRedirect">A value indicating whether we should made redirection after restart</param>
        /// <param name="redirectUrl">Redirect URL; empty string if you want to redirect to the current page URL</param>
        public virtual void RestartAppDomain(HttpContextBase httpContext, bool makeRedirect = false, string redirectUrl = "")
        {
            if (GetTrustLevel() > AspNetHostingPermissionLevel.Medium)
            {
                //full trust
                HttpRuntime.UnloadAppDomain();

                TryWriteGlobalAsax();
            }
            else
            {
                //medium trust
                bool success = TryWriteWebConfig();
                if (!success)
                {
                    throw new Exception("nopCommerce needs to be restarted due to a configuration change, but was unable to do so." + Environment.NewLine +
                        "To prevent this issue in the future, a change to the web server configuration is required:" + Environment.NewLine +
                        "- run the application in a full trust environment, or" + Environment.NewLine +
                        "- give the application write access to the 'web.config' file.");
                }

                success = TryWriteGlobalAsax();
                if (!success)
                {
                    throw new Exception("nopCommerce needs to be restarted due to a configuration change, but was unable to do so." + Environment.NewLine +
                        "To prevent this issue in the future, a change to the web server configuration is required:" + Environment.NewLine +
                        "- run the application in a full trust environment, or" + Environment.NewLine +
                        "- give the application write access to the 'Global.asax' file.");
                }
            }

            // If setting up extensions/modules requires an AppDomain restart, it's very unlikely the
            // current request can be processed correctly.  So, we redirect to the same URL, so that the
            // new request will come to the newly started AppDomain.
            if (httpContext != null && makeRedirect)
            {
                if (String.IsNullOrEmpty(redirectUrl))
                    redirectUrl = GetRootSite(httpContext);//GetThisPageUrl(true);
                httpContext.Response.Redirect(redirectUrl, true /*endResponse*/);
            }
        }
    }

    /*
    //System.DirectoryServices and System.Management
    public class WebSiteController
    {
        public string Password { get; set; }
        public string Server { get; set; }
        public string User { get; set; }
        public string WebSite { get; set; }

        /// <summary>
        /// Returns the site ID from the specified WebSite name
        /// </summary>
        /// <returns>Site ID</returns>
        private string GetSiteIdFromWebSiteName()
        {
            var path = string.Format(@"IIS://{0}/W3SVC", Server);
            var root = new DirectoryEntry(path);

            return root.Children.Cast<DirectoryEntry>()
                .Where(x => x.SchemaClassName == "IIsWebServer"
                    && x.Properties["ServerComment"].Value.ToString()
                        .Equals(WebSite, StringComparison.CurrentCultureIgnoreCase))
                .Select(x => x.Name)
                .FirstOrDefault();
        }

        /// <summary>
        /// Stops the specified WebSite
        /// </summary>
        public void Stop()
        {
            if (!ExecuteAsync("Stop").WaitOne(30000))
            {
                Console.WriteLine("Stop operation timed out");
            }
        }

        /// <summary>
        /// Starts the specified WebSite
        /// </summary>
        public void Start()
        {
            if (!ExecuteAsync("Start").WaitOne(30000))
            {
                Console.WriteLine("Start operation timed out");
            }
        }

        /// <summary>
        /// Executes the specified function asynchronously
        /// </summary>
        /// <param name="function">Function to execute</param>
        /// <returns>WaitHandle that will be set when execution completes</returns>
        private WaitHandle ExecuteAsync(string function)
        {
            var wh = new AutoResetEvent(false);
            ThreadPool.QueueUserWorkItem(x =>
            {
                Execute(function);
                wh.Set();
            });
            return wh;
        }

        /// <summary>
        /// Executes the specified function
        /// </summary>
        /// <param name="function">Function to execute</param>
        private void Execute(string function)
        {
            var options = new ConnectionOptions
            {
                Username = User,
                Password = Password,
                Authentication = AuthenticationLevel.PacketPrivacy
            };
            var mgmtPath = new ManagementPath
            {
                Server = Server,
                NamespacePath = "root/MicrosoftIISv2"
            };
            var mgmtScope = new ManagementScope(mgmtPath, options);
            mgmtScope.Connect();

            var siteId = GetSiteIdFromWebSiteName();
            if (!string.IsNullOrEmpty(siteId))
            {
                var selectQuery = new SelectQuery("SELECT * FROM IIsWebServer WHERE Name = 'W3SVC/" + siteId + "'");
                using (var managementObjectSearcher = new ManagementObjectSearcher(mgmtScope, selectQuery))
                {
                    foreach (ManagementObject objMgmt in managementObjectSearcher.Get())
                    {
                        objMgmt.InvokeMethod(function, new object[0]);
                    }
                }
            }
        }
    }
    */
}