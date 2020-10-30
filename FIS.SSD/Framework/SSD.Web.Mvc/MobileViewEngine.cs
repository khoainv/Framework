using System;
using System.Web.Mvc;

namespace SSD.Web.Mvc
{
    public class MobileViewEngine : IViewEngine
    {
        public IViewEngine BaseViewEngine { get; private set; }
        public Func<ControllerContext, bool> IsTheRightDevice { get; private set; }
        public string PathToSearch { get; private set; }

        public MobileViewEngine(Func<ControllerContext, bool> isTheRightDevice, string pathToSearch, IViewEngine baseViewEngine)
        {
            BaseViewEngine = baseViewEngine;
            IsTheRightDevice = isTheRightDevice;
            PathToSearch = pathToSearch;
        }

        public ViewEngineResult FindPartialView(ControllerContext context, string viewName, bool useCache)
        {
            if (IsTheRightDevice(context))
            {
                return BaseViewEngine.FindPartialView(context, PathToSearch + "/" + viewName, useCache);
            }
            return new ViewEngineResult(new string[] { }); //we found nothing and we pretend we looked nowhere
        }

        public ViewEngineResult FindView(ControllerContext context, string viewName, string masterName, bool useCache)
        {
            if (IsTheRightDevice(context))
            {
                return BaseViewEngine.FindView(context, PathToSearch + "/" + viewName, masterName, useCache);
            }
            return new ViewEngineResult(new string[] { }); //we found nothing and we pretend we looked nowhere
        }

        public void ReleaseView(ControllerContext controllerContext, IView view)
        {
            throw new NotImplementedException();
        }
    }

    public static class MobileHelpers
    {
        public const string K_UserAgent_IPhone = "iphone";
        public const string K_UserAgent_IPod = "ipod";
        public const string K_UserAgent_IPad = "ipad";
        public const string K_UserAgent_Blackberry = "blackberry";
        public const string K_UserAgent_WindowsPhoneOS7_0 = "windows phone os 7.0";
        public const string K_UserAgent_Windows = "windows";
        public const string K_UserAgent_Mobile = "mobile";
        public const string K_UserAgent_WindowsCE = "windows ce";
        public const string K_UserAgent_Android = "android";
        public const string K_UserAgent_Symbianos = "symbianos";
        public const string K_UserAgent_Bada = "bada";
        public const string K_UserAgent_J2me = "j2me";
        public const string K_UserAgent_Midp = "midp";

        public static bool UserAgentContains(this ControllerContext c, string agentToFind)
        {
            return (c.HttpContext.Request.UserAgent.IndexOf(agentToFind, StringComparison.OrdinalIgnoreCase) > 0);
        }

        public static bool IsMobileDevice(this ControllerContext c)
        {
            return c.HttpContext.Request.Browser.IsMobileDevice;
        }
        public static bool IsTabletDevice(this ControllerContext c)
        {
            return c.HttpContext.Request.Browser.ScreenPixelsWidth>=800 && c.HttpContext.Request.Browser.IsMobileDevice;
        }

        public static void AddMobile<T>(this ViewEngineCollection ves, Func<ControllerContext, bool> isTheRightDevice, string pathToSearch)
            where T : IViewEngine, new()
        {
            ves.Add(new MobileViewEngine(isTheRightDevice, pathToSearch, new T()));
        }

        public static void AddMobile<T>(this ViewEngineCollection ves, string userAgentSubstring, string pathToSearch)
            where T : IViewEngine, new()
        {
            ves.Add(new MobileViewEngine(c => c.UserAgentContains(userAgentSubstring), pathToSearch, new T()));
        }

        public static void AddNokia<T>(this ViewEngineCollection ves) //specific example helper
            where T : IViewEngine, new()
        {
            ves.Add(new MobileViewEngine(c => c.UserAgentContains("Nokia"), "Mobile/Nokia", new T()));
        }

        public static void AddBlackberry<T>(this ViewEngineCollection ves) //specific example helper
            where T : IViewEngine, new()
        {
            ves.Add(new MobileViewEngine(c => c.UserAgentContains("blackberry"), "Mobile/BlackBerry", new T()));
        }
        public static void AddWP7<T>(this ViewEngineCollection ves) //specific example helper
            where T : IViewEngine, new()
        {
            ves.Add(new MobileViewEngine(c => c.UserAgentContains("Windows Phone"), "Mobile/WP7", new T()));
        }

        public static void AddIPhone<T>(this ViewEngineCollection ves) //specific example helper
            where T : IViewEngine, new()
        {
            ves.Add(new MobileViewEngine(c => c.UserAgentContains("iPhone"), "Mobile/iPhone", new T()));
        }

        public static void AddGenericMobile<T>(this ViewEngineCollection ves)
            where T : IViewEngine, new()
        {
            ves.Add(new MobileViewEngine(c => c.IsMobileDevice(), "Mobile", new T()));
        }

        public static void AddGenericTablet<T>(this ViewEngineCollection ves)
            where T : IViewEngine, new()
        {
            ves.Add(new MobileViewEngine(c => c.IsTabletDevice(), "Tablet", new T()));
        }

        public static void AddIPad<T>(this ViewEngineCollection ves) //specific example helper
            where T : IViewEngine, new()
        {
            ves.Add(new MobileViewEngine(c => c.UserAgentContains("iPad"), "Tablet/iPad", new T()));
        }
    }
}