using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SSD.Web.UI.Ribbon;
using System.Web.Helpers;
using SSD.Web.UI.Captcha;
using SSD.Web.UI.FileManager;

namespace MvcAppTest.Controllers
{
    public class BaseController : Controller
    {
        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
            if (items.Count == 0)
            {
                //items.Clear();
                //RibbonItemMap.Clear();
                AddHome();
                AddLogistics();
                AddSalesMarketing();
                AddCRM();
                AddContent();
            }
            Dictionary<List<MenuRibbon>, List<MenuItem>> d = new Dictionary<List<MenuRibbon>, List<MenuItem>>();
            d.Add(MenuRibbon(), MenuItems());
            ViewData["RibbonMenu"] = d;
        }

        #region Build Data
        public static List<MenuItem> MenuItems()
        {
            List<MenuItem> sysMenu = new List<MenuItem>()
            {
                new MenuItem
                {
                    ID=1,
                    Name = "Email Accounts",
                    URLOrDiv = "/Home/Index/",
                    Icon = "/Content/ribbon/images/ecommerce/orb_Email_Accounts.png",
                    DisplayOrder=0
                },
                new MenuItem
                {
                    ID=2,
                    Name = "Payment methods",
                    URLOrDiv = "/Home/Index/",
                    Icon = "/Content/ribbon/images/ecommerce/orb_Payment_methods.png",
                    DisplayOrder=1
                },
                new MenuItem
                {
                    ID=3,
                    Name = "Access control list",
                    URLOrDiv = "/Home/Index/",
                    Icon = "/Content/ribbon/images/ecommerce/orb_Access_control_list.png",
                    DisplayOrder=2
                },
                new MenuItem
                {
                    ID=4,
                    Name = "Measures",
                    URLOrDiv = "/Home/Index/",
                    Icon = "/Content/ribbon/images/ecommerce/orb_Measures.png",
                    DisplayOrder=3
                },
                new MenuItem
                {
                    ID=5,
                    ParentID=4,
                    Name = "Weights",
                    URLOrDiv = "/Home/Index/",
                    Icon = "/Content/ribbon/images/ecommerce/orb_Measures_Weights.png",
                    DisplayOrder=0
                },
                new MenuItem
                {
                    ID=6,
                    ParentID=4,
                    Name = "Dimensions",
                    URLOrDiv = "/Home/Index/",
                    Icon = "/Content/ribbon/images/ecommerce/orb_Measures_Dimensions.png",
                    DisplayOrder=1
                },
                new MenuItem
                {
                    ID=7,
                    Name = "Tax & Shipping",
                    URLOrDiv = "/Home/Index/",
                    Icon = "/Content/ribbon/images/ecommerce/orb_tax_shipping.png",
                    DisplayOrder=4
                },
                new MenuItem
                {
                    ID=8,
                    ParentID=7,
                    Name = "Tax providers",
                    URLOrDiv = "/Home/Index/",
                    Icon = "/Content/ribbon/images/ecommerce/orb_tax.png",
                    DisplayOrder=0
                },
                new MenuItem
                {
                    ID=9,
                    ParentID=7,
                    Name = "Tax categories",
                    URLOrDiv = "/Home/Index/",
                    Icon = "/Content/ribbon/images/ecommerce/orb_tax_category.png",
                    DisplayOrder=1
                },
                new MenuItem
                {
                    ID=10,
                    ParentID=7,
                    Name = "Shipping methods",
                    URLOrDiv = "/Home/Index/",
                    Icon = "/Content/ribbon/images/ecommerce/orb_shipping.png",
                    DisplayOrder=2
                },
                new MenuItem
                {
                    ID=11,
                    ParentID=7,
                    Name = "Shipping method restrictions",
                    URLOrDiv = "/Home/Index/",
                    Icon = "/Content/ribbon/images/ecommerce/orb_shipping.png",
                    DisplayOrder=3
                },
                new MenuItem
                {
                    ID=12,
                    ParentID=7,
                    Name = "Shipping rate computation methods",
                    URLOrDiv = "/Home/Index/",
                    Icon = "/Content/ribbon/images/ecommerce/orb_shipping_rate.png",
                    DisplayOrder=4
                },
                new MenuItem
                {
                    ID=13,
                    Name = "Providers",
                    URLOrDiv = "/Home/Index/",
                    Icon = "/Content/ribbon/images/ecommerce/orb_provider.png",
                    DisplayOrder=5
                },
                new MenuItem
                {
                    ID=14,
                    ParentID=13,
                    Name = "SMS providers",
                    URLOrDiv = "/Home/Index/",
                    Icon = "/Content/ribbon/images/ecommerce/orb_provider_sms.png",
                    DisplayOrder=0
                },
                new MenuItem
                {
                    ID=15,
                    ParentID=13,
                    Name = "Live chat providers",
                    URLOrDiv = "/Home/Index/",
                    Icon = "/Content/ribbon/images/ecommerce/orb_provider_chat.png",
                    DisplayOrder=1
                },
                new MenuItem
                {
                    ID=16,
                    ParentID=13,
                    Name = "Plugins",
                    URLOrDiv = "/Home/Index/",
                    Icon = "/Content/ribbon/images/ecommerce/orb_provider_plugin.png",
                    DisplayOrder=2
                },
                new MenuItem
                {
                    ID=17,
                    Name = "Location",
                    URLOrDiv = "/Home/Index/",
                    Icon = "/Content/ribbon/images/ecommerce/orb_location.png",
                    DisplayOrder=6
                },
                new MenuItem
                {
                    ID=18,
                    ParentID=17,
                    Name = "Countries",
                    URLOrDiv = "/Home/Index/",
                    Icon = "/Content/ribbon/images/ecommerce/orb_location_country.png",
                    DisplayOrder=0
                },
                new MenuItem
                {
                    ID=19,
                    ParentID=17,
                    Name = "Languages",
                    URLOrDiv = "/Home/Index/",
                    Icon = "/Content/ribbon/images/ecommerce/orb_location_language.png",
                    DisplayOrder=1
                },
                new MenuItem
                {
                    ID=20,
                    ParentID=17,
                    Name = "Currencies",
                    URLOrDiv = "/Home/Index/",
                    Icon = "/Content/ribbon/images/ecommerce/orb_location_currency.png",
                    DisplayOrder=2
                },
                new MenuItem
                {
                    ID=21,
                    Name = "Activity Log",
                    URLOrDiv = "/Home/Index/",
                    Icon = "/Content/ribbon/images/ecommerce/orb_log.png",
                    DisplayOrder=7
                },
                new MenuItem
                {
                    ID=22,
                    ParentID=21,
                    Name = "Activity Types",
                    URLOrDiv = "/Home/Index/",
                    Icon = "/Content/ribbon/images/ecommerce/orb_log_type.png",
                    DisplayOrder=0
                },
                new MenuItem
                {
                    ID=23,
                    ParentID=21,
                    Name = "Activity Log",
                    URLOrDiv = "/Home/Index/",
                    Icon = "/Content/ribbon/images/ecommerce/orb_log.png",
                    DisplayOrder=1
                },
                new MenuItem
                {
                    ID=24,
                    Name = "Settings",
                    URLOrDiv = "/Home/Index/",
                    Icon = "/Content/ribbon/images/ecommerce/orb_setting.png",
                    DisplayOrder=8
                },
                new MenuItem
                {
                    ID=25,
                    ParentID=24,
                    Name = "Catalog settings",
                    URLOrDiv = "/Home/Index/",
                    Icon = "/Content/ribbon/images/ecommerce/orb_setting_catalog.png",
                    DisplayOrder=0
                },
                new MenuItem
                {
                    ID=26,
                    ParentID=24,
                    Name = "Customer settings",
                    URLOrDiv = "/Home/Index/",
                    Icon = "/Content/ribbon/images/ecommerce/orb_setting_customer.png",
                    DisplayOrder=1
                },
                new MenuItem
                {
                    ID=27,
                    ParentID=24,
                    Name = "Shopping cart settings",
                    URLOrDiv = "/Home/Index/",
                    Icon = "/Content/ribbon/images/ecommerce/orb_setting_shoppingcard.png",
                    DisplayOrder=2
                },
                new MenuItem
                {
                    ID=28,
                    ParentID=24,
                    Name = "Order settings",
                    URLOrDiv = "/Home/Index/",
                    Icon = "/Content/ribbon/images/ecommerce/orb_setting_order.png",
                    DisplayOrder=3
                },
                new MenuItem
                {
                    ID=29,
                    ParentID=24,
                    Name = "Media settings",
                    URLOrDiv = "/Home/Index/",
                    Icon = "/Content/ribbon/images/ecommerce/orb_setting_media.png",
                    DisplayOrder=4
                },
                new MenuItem
                {
                    ID=30,
                    ParentID=24,
                    Name = "Tax settings",
                    URLOrDiv = "/Home/Index/",
                    Icon = "/Content/ribbon/images/ecommerce/orb_setting_tax.png",
                    DisplayOrder=5
                },
                new MenuItem
                {
                    ID=31,
                    ParentID=24,
                    Name = "Shipping settings",
                    URLOrDiv = "/Home/Index/",
                    Icon = "/Content/ribbon/images/ecommerce/orb_setting_shipping.png",
                    DisplayOrder=6
                },
                new MenuItem
                {
                    ID=32,
                    ParentID=24,
                    Name = "Reward points",
                    URLOrDiv = "/Home/Index/",
                    Icon = "/Content/ribbon/images/ecommerce/orb_setting_Reward_points.png",
                    DisplayOrder=7
                },
                new MenuItem
                {
                    ID=33,
                    ParentID=24,
                    Name = "General settings",
                    URLOrDiv = "/Home/Index/",
                    Icon = "/Content/ribbon/images/ecommerce/orb_setting_General.png",
                    DisplayOrder=8
                },
                new MenuItem
                {
                    ID=34,
                    ParentID=24,
                    Name = "All settings (advanced)",
                    URLOrDiv = "/Home/Index/",
                    Icon = "/Content/ribbon/images/ecommerce/orb_setting_advanced.png",
                    DisplayOrder=9
                },
                new MenuItem
                {
                    ID=35,
                    Name = "System",
                    URLOrDiv = "/Home/Index/",
                    Icon = "/Content/ribbon/images/ecommerce/orb_system.png",
                    DisplayOrder=9
                },
                new MenuItem
                {
                    ID=36,
                    ParentID=35,
                    Name = "Log",
                    URLOrDiv = "/Home/Index/",
                    Icon = "/Content/ribbon/images/ecommerce/orb_log.png",
                    DisplayOrder=0
                },
                new MenuItem
                {
                    ID=37,
                    ParentID=35,
                    Name = "Message Queue",
                    URLOrDiv = "/Home/Index/",
                    Icon = "/Content/ribbon/images/ecommerce/orb_system_message.png",
                    DisplayOrder=1
                },
                new MenuItem
                {
                    ID=38,
                    ParentID=35,
                    Name = "Maintenance",
                    URLOrDiv = "/Home/Index/",
                    Icon = "/Content/ribbon/images/ecommerce/orb_system_maintenace.png",
                    DisplayOrder=2
                },
                new MenuItem
                {
                    ID=39,
                    ParentID=35,
                    Name = "Warnings",
                    URLOrDiv = "/Home/Index/",
                    Icon = "/Content/ribbon/images/ecommerce/orb_system_warning.png",
                    DisplayOrder=3
                },
                new MenuItem
                {
                    ID=40,
                    ParentID=35,
                    Name = "System information",
                    URLOrDiv = "/Home/Index/",
                    Icon = "/Content/ribbon/images/ecommerce/orb_system_Info.png",
                    DisplayOrder=4
                },
                new MenuItem
                {
                    ID=41,
                    Name = "Exit",
                    URLOrDiv = "/Home/Index/",
                    Icon = "/Content/ribbon/images/icon_exit.png",
                    DisplayOrder=10
                },
                new MenuItem
                {
                    ID=42,
                    Name = "User Manager",
                    URLOrDiv = "/Home/Index/",
                    Icon = "/Content/ribbon/images/ecommerce/orb_users_manager.png",
                    DisplayOrder=4
                },
                new MenuItem
                {
                    ID=43,
                    Name = "Media Manager",
                    URLOrDiv = "/Home/Index/",
                    Icon = "/Content/ribbon/images/ecommerce/orb_media_manager.png",
                    DisplayOrder=4
                },
            };

            return sysMenu;
        }
        public List<MenuRibbon> MenuRibbon()
        {
            //Build Data
            List<MenuRibbon> menu = new List<MenuRibbon>()
            {
                #region Home
                new MenuRibbon
                {
                    ID=1,
                    Name = "Home",
                    Type=0,//Tab
                    DisplayOrder=0
                },
                new MenuRibbon
                {
                    ID=2,
                    ParentID = 1,
                    Name = "Clipboard",
                    Type= LevelType.Group,// 1,//Group
                    DisplayOrder=0,//(new int[] {2}).Contains(p.ID)
                    Items = (from p in items join s in RibbonItemMap on p.ID equals s.MenuItemID
                            where s.MenuRibbonID == 2 orderby s.DisplayOrder select p).ToList()
                },
                new MenuRibbon
                {
                    ID=3,
                    ParentID = 2,
                    Type=LevelType.ItemList,//ItemList
                    DisplayOrder=1,
                    Items = (from p in items join s in RibbonItemMap on p.ID equals s.MenuItemID
                            where s.MenuRibbonID == 3 orderby s.DisplayOrder select p).ToList()
                },
                //================
                new MenuRibbon
                {
                    ID=4,
                    ParentID = 1,
                    Name = "Insert",
                    Type=LevelType.Group,//Group
                    DisplayOrder=1,
                    Items = (from p in items join s in RibbonItemMap on p.ID equals s.MenuItemID
                            where s.MenuRibbonID == 4 orderby s.DisplayOrder select p).ToList()
                },
                //=============
                new MenuRibbon
                {
                    ID=5,
                    ParentID = 1,
                    Name = "Editing",
                    Type=LevelType.Group,//Group
                    DisplayOrder=2
                },
                new MenuRibbon
                {
                    ID=6,
                    ParentID = 5,
                    Type=LevelType.ItemList,//ItemList
                    DisplayOrder=0,
                    Items = (from p in items join s in RibbonItemMap on p.ID equals s.MenuItemID
                            where s.MenuRibbonID == 6 orderby s.DisplayOrder select p).ToList()
                },
                //====================
                new MenuRibbon
                {
                    ID=7,
                    ParentID = 1,
                    Name = "Theme",
                    Type=LevelType.Group,//Group
                    DisplayOrder=3
                },
                new MenuRibbon
                {
                    ID=8,
                    ParentID = 7,
                    Type=LevelType.ItemList,//ItemList
                    DisplayOrder=0,
                    Items = (from p in items join s in RibbonItemMap on p.ID equals s.MenuItemID
                            where s.MenuRibbonID == 8 orderby s.DisplayOrder select p).ToList()
                },
                new MenuRibbon
                {
                    ID=9,
                    ParentID = 8,
                    Name = "Ribbon theme",
                    Icon="/Content/ribbon/images/icon_small_theme.png",
                    Type=LevelType.ItemMenu,//ItemMenu
                    DisplayOrder=0,
                    Items = (from p in items join s in RibbonItemMap on p.ID equals s.MenuItemID
                            where s.MenuRibbonID == 9 orderby s.DisplayOrder select p).ToList()
                },
                #endregion
                //====================
                #region Logistics
                new MenuRibbon
                {
                    ID=10,
                    Name = "Logistics",
                    Type=LevelType.Tab,//Tab
                    DisplayOrder=1
                },
                new MenuRibbon
                {
                    ID=11,
                    ParentID = 10,
                    Name = "Products",
                    Type= LevelType.Group,
                    DisplayOrder=0,
                    Items = (from p in items join s in RibbonItemMap on p.ID equals s.MenuItemID
                            where s.MenuRibbonID == 11 orderby s.DisplayOrder select p).ToList()
                },
                new MenuRibbon
                {
                    ID=12,
                    ParentID = 11,
                    Type= LevelType.ItemList,
                    DisplayOrder=0,
                    Items = (from p in items join s in RibbonItemMap on p.ID equals s.MenuItemID
                            where s.MenuRibbonID == 12 orderby s.DisplayOrder select p).ToList()
                },
                new MenuRibbon
                {
                    ID=13,
                    ParentID = 10,
                    Name = "Categories",
                    Type= LevelType.Group,
                    DisplayOrder=1,
                    Items = (from p in items join s in RibbonItemMap on p.ID equals s.MenuItemID
                            where s.MenuRibbonID == 13 orderby s.DisplayOrder select p).ToList()
                },
                new MenuRibbon
                {
                    ID=14,
                    ParentID = 13,
                    Type= LevelType.ItemList,
                    DisplayOrder=0,
                    Items = (from p in items join s in RibbonItemMap on p.ID equals s.MenuItemID
                            where s.MenuRibbonID == 14 orderby s.DisplayOrder select p).ToList()
                },
                new MenuRibbon
                {
                    ID=15,
                    ParentID = 10,
                    Name = "Manufacturers",
                    Type= LevelType.Group,
                    DisplayOrder=2,
                    Items = (from p in items join s in RibbonItemMap on p.ID equals s.MenuItemID
                            where s.MenuRibbonID == 15 orderby s.DisplayOrder select p).ToList()
                },
                new MenuRibbon
                {
                    ID=16,
                    ParentID = 10,
                    Name = "Attributes",
                    Type= LevelType.Group,
                    DisplayOrder=3,
                    Items = (from p in items join s in RibbonItemMap on p.ID equals s.MenuItemID
                            where s.MenuRibbonID == 16 orderby s.DisplayOrder select p).ToList()
                },
                new MenuRibbon
                {
                    ID=17,
                    ParentID = 10,
                    Name = "Reports",
                    Type= LevelType.Group,
                    DisplayOrder=4,
                    Items = (from p in items join s in RibbonItemMap on p.ID equals s.MenuItemID
                            where s.MenuRibbonID == 17 orderby s.DisplayOrder select p).ToList()
                },
                new MenuRibbon
                {
                    ID=18,
                    ParentID = 10,
                    Name = "Export/Import",
                    Type= LevelType.Group,
                    DisplayOrder=5,
                    Items = (from p in items join s in RibbonItemMap on p.ID equals s.MenuItemID
                            where s.MenuRibbonID == 18 orderby s.DisplayOrder select p).ToList()
                },
                new MenuRibbon
                {
                    ID=19,
                    ParentID = 18,
                    Type= LevelType.ItemList,
                    DisplayOrder=0,
                    Items = (from p in items join s in RibbonItemMap on p.ID equals s.MenuItemID
                            where s.MenuRibbonID == 19 orderby s.DisplayOrder select p).ToList()
                },
                new MenuRibbon
                {
                    ID=20,
                    ParentID = 18,
                    Type= LevelType.ItemList,
                    DisplayOrder=1,
                    Items = (from p in items join s in RibbonItemMap on p.ID equals s.MenuItemID
                            where s.MenuRibbonID == 20 orderby s.DisplayOrder select p).ToList()
                },
                #endregion

                #region Sale & Marketing
                new MenuRibbon
                {
                    ID=21,
                    Name = "Sale & Marketing",
                    Type=LevelType.Tab,//Tab
                    DisplayOrder=2
                },
                new MenuRibbon
                {
                    ID=22,
                    ParentID = 21,
                    Name = "Marketing",
                    Type= LevelType.Group,
                    DisplayOrder=0
                },
                new MenuRibbon
                {
                    ID=23,
                    ParentID = 22,
                    Type= LevelType.ItemList,
                    DisplayOrder=0,
                    Items = (from p in items join s in RibbonItemMap on p.ID equals s.MenuItemID
                            where s.MenuRibbonID == 23 orderby s.DisplayOrder select p).ToList()
                },
                new MenuRibbon
                {
                    ID=24,
                    ParentID = 22,
                    Type= LevelType.ItemList,
                    DisplayOrder=1,
                    Items = (from p in items join s in RibbonItemMap on p.ID equals s.MenuItemID
                            where s.MenuRibbonID == 24 orderby s.DisplayOrder select p).ToList()
                },
                new MenuRibbon
                {
                    ID=25,
                    ParentID = 21,
                    Name = "Sales",
                    Type= LevelType.Group,
                    DisplayOrder=1,
                    Items = (from p in items join s in RibbonItemMap on p.ID equals s.MenuItemID
                            where s.MenuRibbonID == 25 orderby s.DisplayOrder select p).ToList()
                },
                new MenuRibbon
                {
                    ID=26,
                    ParentID = 25,
                    Type= LevelType.ItemList,
                    DisplayOrder=0,
                    Items = (from p in items join s in RibbonItemMap on p.ID equals s.MenuItemID
                            where s.MenuRibbonID == 26 orderby s.DisplayOrder select p).ToList()
                },
                new MenuRibbon
                {
                    ID=27,
                    ParentID = 25,
                    Type= LevelType.ItemList,
                    DisplayOrder=1,
                    Items = (from p in items join s in RibbonItemMap on p.ID equals s.MenuItemID
                            where s.MenuRibbonID == 27 orderby s.DisplayOrder select p).ToList()
                }
                ,
                new MenuRibbon
                {
                    ID=28,
                    ParentID = 21,
                    Name = "Reports",
                    Type= LevelType.Group,
                    DisplayOrder=2,
                    Items = (from p in items join s in RibbonItemMap on p.ID equals s.MenuItemID
                            where s.MenuRibbonID == 28 orderby s.DisplayOrder select p).ToList()
                },
                new MenuRibbon
                {
                    ID=29,
                    ParentID = 28,
                    Type= LevelType.ItemList,
                    DisplayOrder=0,
                    Items = (from p in items join s in RibbonItemMap on p.ID equals s.MenuItemID
                            where s.MenuRibbonID == 29 orderby s.DisplayOrder select p).ToList()
                },
                #endregion

                #region CRM
                new MenuRibbon
                {
                    ID=30,
                    Name = "CRM",
                    Type=LevelType.Tab,//Tab
                    DisplayOrder=3
                },
                new MenuRibbon
                {
                    ID=31,
                    ParentID = 30,
                    Name = "Customers",
                    Type= LevelType.Group,
                    DisplayOrder=0,
                    Items = (from p in items join s in RibbonItemMap on p.ID equals s.MenuItemID
                            where s.MenuRibbonID == 31 orderby s.DisplayOrder select p).ToList()
                },
                new MenuRibbon
                {
                    ID=32,
                    ParentID = 31,
                    Type= LevelType.ItemList,
                    DisplayOrder=0,
                    Items = (from p in items join s in RibbonItemMap on p.ID equals s.MenuItemID
                            where s.MenuRibbonID == 23 orderby s.DisplayOrder select p).ToList()
                },
                new MenuRibbon
                {
                    ID=33,
                    ParentID = 30,
                    Name = "After Sales",
                    Type= LevelType.Group,
                    DisplayOrder=1,
                    Items = (from p in items join s in RibbonItemMap on p.ID equals s.MenuItemID
                            where s.MenuRibbonID == 33 orderby s.DisplayOrder select p).ToList()
                },
                new MenuRibbon
                {
                    ID=34,
                    ParentID = 30,
                    Name = "Contacts",
                    Type= LevelType.Group,
                    DisplayOrder=2,
                    Items = (from p in items join s in RibbonItemMap on p.ID equals s.MenuItemID
                            where s.MenuRibbonID == 34 orderby s.DisplayOrder select p).ToList()
                },
                new MenuRibbon
                {
                    ID=35,
                    ParentID = 30,
                    Name = "Contracts",
                    Type= LevelType.Group,
                    DisplayOrder=3,
                    Items = (from p in items join s in RibbonItemMap on p.ID equals s.MenuItemID
                            where s.MenuRibbonID == 35 orderby s.DisplayOrder select p).ToList()
                },
                #endregion

                #region Content
                new MenuRibbon
                {
                    ID=36,
                    Name = "Content",
                    Type=LevelType.Tab,//Tab
                    DisplayOrder=3
                },
                new MenuRibbon
                {
                    ID=37,
                    ParentID = 36,
                    Name = "Menus",
                    Type= LevelType.Group,
                    DisplayOrder=0,
                    Items = (from p in items join s in RibbonItemMap on p.ID equals s.MenuItemID
                            where s.MenuRibbonID == 37 orderby s.DisplayOrder select p).ToList()
                },
                new MenuRibbon
                {
                    ID=38,
                    ParentID = 37,
                    Type= LevelType.ItemList,
                    DisplayOrder=0,
                    Items = (from p in items join s in RibbonItemMap on p.ID equals s.MenuItemID
                            where s.MenuRibbonID == 38 orderby s.DisplayOrder select p).ToList()
                },
                new MenuRibbon
                {
                    ID=39,
                    ParentID = 36,
                    Name = "News",
                    Type= LevelType.Group,
                    DisplayOrder=1,
                    Items = (from p in items join s in RibbonItemMap on p.ID equals s.MenuItemID
                            where s.MenuRibbonID == 39 orderby s.DisplayOrder select p).ToList()
                },
                new MenuRibbon
                {
                    ID=40,
                    ParentID = 39,
                    Type= LevelType.ItemList,
                    DisplayOrder=0,
                    Items = (from p in items join s in RibbonItemMap on p.ID equals s.MenuItemID
                            where s.MenuRibbonID == 40 orderby s.DisplayOrder select p).ToList()
                },
                new MenuRibbon
                {
                    ID=41,
                    ParentID = 36,
                    Name = "Blogs",
                    Type= LevelType.Group,
                    DisplayOrder=2,
                    Items = (from p in items join s in RibbonItemMap on p.ID equals s.MenuItemID
                            where s.MenuRibbonID == 41 orderby s.DisplayOrder select p).ToList()
                },
                new MenuRibbon
                {
                    ID=42,
                    ParentID = 41,
                    Type= LevelType.ItemList,
                    DisplayOrder=0,
                    Items = (from p in items join s in RibbonItemMap on p.ID equals s.MenuItemID
                            where s.MenuRibbonID == 42 orderby s.DisplayOrder select p).ToList()
                },
                new MenuRibbon
                {
                    ID=43,
                    ParentID = 36,
                    Name = "Forums",
                    Type= LevelType.Group,
                    DisplayOrder=3,
                    Items = (from p in items join s in RibbonItemMap on p.ID equals s.MenuItemID
                            where s.MenuRibbonID == 43 orderby s.DisplayOrder select p).ToList()
                },
                new MenuRibbon
                {
                    ID=44,
                    ParentID = 43,
                    Type= LevelType.ItemList,
                    DisplayOrder=0,
                    Items = (from p in items join s in RibbonItemMap on p.ID equals s.MenuItemID
                            where s.MenuRibbonID == 44 orderby s.DisplayOrder select p).ToList()
                },
                new MenuRibbon
                {
                    ID=45,
                    ParentID = 36,
                    Name = "Surveys",
                    Type= LevelType.Group,
                    DisplayOrder=4,
                    Items = (from p in items join s in RibbonItemMap on p.ID equals s.MenuItemID
                            where s.MenuRibbonID == 45 orderby s.DisplayOrder select p).ToList()
                },
                new MenuRibbon
                {
                    ID=46,
                    ParentID = 45,
                    Type= LevelType.ItemList,
                    DisplayOrder=0,
                    Items = (from p in items join s in RibbonItemMap on p.ID equals s.MenuItemID
                            where s.MenuRibbonID == 46 orderby s.DisplayOrder select p).ToList()
                },
                new MenuRibbon
                {
                    ID=47,
                    ParentID = 36,
                    Name = "Contacts",
                    Type= LevelType.Group,
                    DisplayOrder=4,
                    Items = (from p in items join s in RibbonItemMap on p.ID equals s.MenuItemID
                            where s.MenuRibbonID == 47 orderby s.DisplayOrder select p).ToList()
                },
                new MenuRibbon
                {
                    ID=48,
                    ParentID = 47,
                    Type= LevelType.ItemList,
                    DisplayOrder=0,
                    Items = (from p in items join s in RibbonItemMap on p.ID equals s.MenuItemID
                            where s.MenuRibbonID == 48 orderby s.DisplayOrder select p).ToList()
                }
                #endregion
            };
            return menu;
        }
        public static List<RibbonItem> RibbonItemMap = new List<RibbonItem>();
        public static List<MenuItem> items = new List<MenuItem>();
        public static void AddHome()
        {
            items.AddRange(
            new MenuItem[]{
                new MenuItem
                        {
                            ID=1,
                            Name = "Home",
                            Icon="/Content/ribbon/images/icon_paste.png",
                            DisplayOrder=0,
                            URLOrDiv = "/Home/Index"
                        },
                        new MenuItem
                        {
                            ID=2,
                            Name = "File",
                            Icon="/Content/ribbon/images/icon_small_cut.png",
                            DisplayOrder=1,
                            URLOrDiv = "/Home/FileManager"
                        },
                        new MenuItem
                        {
                            ID=3,
                            Name = "Map",
                            Icon="/Content/ribbon/images/icon_small_copy.png",
                            DisplayOrder=2,
                            URLOrDiv = "/Map/Index"
                        },
                        new MenuItem
                        {
                            ID=4,
                            Name = "Ajax",
                            URLOrDiv="/Home/AjaxRibbon/",
                            Icon="/Content/ribbon/images/icon_datetime.png",
                            DisplayOrder=3,
                            ActionType = ActionType.Ajax,
                            Shortcut="Alt + A",
                            EnableActionOnViews = new List<string>()
                            {
                                {"/home/index/"},
                                {"/home/IntergrateTest/"}
                            },
                            //Paras= new Dictionary<string,string>()
                            //{
                            //    {"SimleText",""},
                            //    {"Description",""}
                            //},
                            AreaContainerID = "abcde12345"
                            //MCEEditorFields = new List<string>(){"Description"}
                        },
                        new MenuItem
                        {
                            ID=5,
                            Name = "Dialog",
                            URLOrDiv="popupabcde12345",
                            Icon="/Content/ribbon/images/icon_picture.png",
                            DisplayOrder=4,
                            ActionType = ActionType.Dialog,
                            Shortcut="Alt + D",
                            EnableActionOnViews = new List<string>()
                            {
                                {"/home/index/"},
                                {"/home/IntergrateTest/"}
                            },
                            AreaContainerID = "abcde12345",
                            Paras= new Dictionary<string,string>()
                            {
                                {"SimleText",""},
                                {"Description",""}
                            },
                            DefineMapPopupControlID= new Dictionary<string,string>()
                            {
                                {"SimleText","popupSimleText"},
                                {"Description","popupDescription"}
                            }
                            //MCEEditorFields = new List<string>(){"Description"}
                        },
                        new MenuItem
                        {
                            ID=6,
                            Name = "Lookup",
                            URLOrDiv="popupabcde12345",
                            Icon="/Content/ribbon/images/icon_paint.png",
                            DisplayOrder=5,
                            ActionType = ActionType.Lookup,
                            Shortcut="Alt + L",
                            EnableActionOnViews = new List<string>()
                            {
                                {"/home/index/"},
                                {"/home/IntergrateTest/"}
                            },
                            AreaContainerID = "abcde12345",
                            Paras= new Dictionary<string,string>()
                            {
                                {"SimleText","default1"},
                                {"Description","default2"}
                            },
                            DefineMapPopupControlID= new Dictionary<string,string>()
                            {
                                {"SimleText","popupSimleText"},
                                {"Description","popupDescription"},
                                {"popupSimleText","SimleText"}
                            },
                            GetValueAfterAction= new Dictionary<string,string>()
                            {
                                {"popupSimleText","val1"},
                                {"popupDescription","val2"}
                            }
                            //MCEEditorFields = new List<string>(){"Description"}
                        },
                        new MenuItem
                        {
                            ID=7,
                            Name = "Rating",
                            Icon="/Content/ribbon/images/icon_small_find.png",
                            DisplayOrder=6,
                            URLOrDiv = "/Rating/Index"
                        },
                        new MenuItem
                        {
                            ID=8,
                            Name = "SlideShow",
                            Icon="/Content/ribbon/images/icon_small_replace.png",
                            DisplayOrder=7,
                            URLOrDiv = "/SlideShow/Index"
                        },
                        new MenuItem
                        {
                            ID=9,
                            Name = "Intergrate",
                            Icon="/Content/ribbon/images/icon_small_selectall.png",
                            DisplayOrder=8,
                            URLOrDiv = "/Home/IntergrateTest"
                        },
                        new MenuItem
                        {
                            ID=10,
                            Name = "Mask input",
                            Icon="/Content/ribbon/images/icon_small_selectall.png",
                            DisplayOrder=9,
                            URLOrDiv="/Home/MaskInput/"
                        },
                        new MenuItem
                        {
                            ID=11,
                            Name = "Windows 7",
                            DisplayOrder=10,
                            ActionType = ActionType.Ajax
                        },
                        new MenuItem
                        {
                            ID=12,
                            Name = "Simple",
                            DisplayOrder=11
                        }
            }
            );

            RibbonItemMap.AddRange(
                new RibbonItem[]{
                new RibbonItem(){
                    MenuItemID = 1,
                    MenuRibbonID=2
                },
                new RibbonItem(){
                    MenuItemID = 2,
                    MenuRibbonID=3,
                    DisplayOrder=0
                },
                new RibbonItem(){
                    MenuItemID = 3,
                    MenuRibbonID=3,
                    DisplayOrder=1
                },
                new RibbonItem(){
                    MenuItemID = 4,
                    MenuRibbonID=4,
                    DisplayOrder=0
                },
                new RibbonItem(){
                    MenuItemID = 5,
                    MenuRibbonID=4,
                    DisplayOrder=1
                },
                new RibbonItem(){
                    MenuItemID = 6,
                    MenuRibbonID=4,
                    DisplayOrder=2
                },
                new RibbonItem(){
                    MenuItemID = 7,
                    MenuRibbonID=6,
                    DisplayOrder=0
                },
                new RibbonItem(){
                    MenuItemID = 8,
                    MenuRibbonID=6,
                    DisplayOrder=1
                },
                new RibbonItem(){
                    MenuItemID = 9,
                    MenuRibbonID=6,
                    DisplayOrder=2
                },
                new RibbonItem(){
                    MenuItemID = 10,
                    MenuRibbonID=8
                },
                new RibbonItem(){
                    MenuItemID = 11,
                    MenuRibbonID=9,
                    DisplayOrder=0
                },
                new RibbonItem(){
                    MenuItemID = 12,
                    MenuRibbonID=9,
                    DisplayOrder=1
                }
            }
                );
        }
        public static void AddLogistics()
        {
            items.AddRange(
            new MenuItem[]{
                        new MenuItem
                        {
                            ID=13,
                            Name = "Add",
                            URLOrDiv = "/Home/Index/",
                            Icon="/Content/ribbon/images/ecommerce/logistics_product_add.png",
                            DisplayOrder=12
                        },
                        new MenuItem
                        {
                            ID=14,
                            Name = "Manage",
                            URLOrDiv = "/Home/Index/",
                            Icon="/Content/ribbon/images/ecommerce/logistics_product_list.png",
                            DisplayOrder=13
                        },
                        new MenuItem
                        {
                            ID=15,
                            Name = "Low stock",
                            URLOrDiv = "/Home/Index/",
                            Icon="/Content/ribbon/images/ecommerce/logistics_product_lowstock.png",
                            DisplayOrder=14
                        },
                        new MenuItem
                        {
                            ID=16,
                            Name = "Reviews",
                            URLOrDiv = "/Home/Index/",
                            Icon="/Content/ribbon/images/ecommerce/logistics_product_reviews.png",
                            DisplayOrder=15
                        },
                        new MenuItem
                        {
                            ID=17,
                            Name = "Tags",
                            URLOrDiv = "/Home/Index/",
                            Icon="/Content/ribbon/images/ecommerce/logistics_product_tags.png",
                            DisplayOrder=16
                        },
                        new MenuItem
                        {
                            ID=18,
                            Name = "Add",
                            URLOrDiv = "/Home/Index/",
                            Icon="/Content/ribbon/images/ecommerce/logistics_category_add.png",
                            DisplayOrder=17
                        },
                        new MenuItem
                        {
                            ID=19,
                            Name = "List",
                            URLOrDiv = "/Home/Index/",
                            Icon="/Content/ribbon/images/ecommerce/logistics_category_list.png",
                            DisplayOrder=18
                        },
                        new MenuItem
                        {
                            ID=20,
                            Name = "Tree",
                            URLOrDiv = "/Home/Index/",
                            Icon="/Content/ribbon/images/ecommerce/logistics_category_tree.png",
                            DisplayOrder=19
                        },
                        new MenuItem
                        {
                            ID=21,
                            Name = "Add",
                            URLOrDiv = "/Home/Index/",
                            Icon="/Content/ribbon/images/ecommerce/logistics_manufacturers_add.png",
                            DisplayOrder=20
                        },
                        new MenuItem
                        {
                            ID=22,
                            Name = "Manager",
                            URLOrDiv = "/Home/Index/",
                            Icon="/Content/ribbon/images/ecommerce/logistics_manufacturers_list.png",
                            DisplayOrder=21
                        },
                        new MenuItem
                        {
                            ID=23,
                            Name = "Product",
                            URLOrDiv = "/Home/Index/",
                            Icon="/Content/ribbon/images/ecommerce/logistics_Attributes_Product.png",
                            DisplayOrder=22
                        },
                        new MenuItem
                        {
                            ID=24,
                            Name = "Checkout",
                            URLOrDiv = "/Home/Index/",
                            Icon="/Content/ribbon/images/ecommerce/logistics_Attributes_Checkout.png",
                            DisplayOrder=23
                        },
                        new MenuItem
                        {
                            ID=25,
                            Name = "Specification",
                            URLOrDiv = "/Home/Index/",
                            Icon="/Content/ribbon/images/ecommerce/logistics_Attributes_Specification.png",
                            DisplayOrder=24
                        },
                        new MenuItem
                        {
                            ID=26,
                            Name = "Products",
                            URLOrDiv = "/Home/Index/",
                            Icon="/Content/ribbon/images/ecommerce/logistics_report_product.png",
                            DisplayOrder=25
                        },
                        new MenuItem
                        {
                            ID=27,
                            Name = "Products",
                            URLOrDiv = "/Home/Index/",
                            Icon="/Content/ribbon/images/ecommerce/logistics_export_product.png",
                            DisplayOrder=26
                        },
                        new MenuItem
                        {
                            ID=28,
                            Name = "Categories",
                            URLOrDiv = "/Home/Index/",
                            Icon="/Content/ribbon/images/ecommerce/logistics_export_category.png",
                            DisplayOrder=27
                        },
                        new MenuItem
                        {
                            ID=29,
                            Name = "Manufacturers",
                            URLOrDiv = "/Home/Index/",
                            Icon="/Content/ribbon/images/ecommerce/logistics_export_m.png",
                            DisplayOrder=28
                        },
                        new MenuItem
                        {
                            ID=30,
                            Name = "Products",
                            URLOrDiv = "/Home/Index/",
                            Icon="/Content/ribbon/images/ecommerce/logistics_import_product.png",
                            DisplayOrder=29
                        },
                        new MenuItem
                        {
                            ID=31,
                            Name = "Categories",
                            URLOrDiv = "/Home/Index/",
                            Icon="/Content/ribbon/images/ecommerce/logistics_import_category.png",
                            DisplayOrder=30
                        },
                        new MenuItem
                        {
                            ID=32,
                            Name = "Manufacturers",
                            URLOrDiv = "/Home/Index/",
                            Icon="/Content/ribbon/images/ecommerce/logistics_import_m.png",
                            DisplayOrder=31
                        }
            }
            );

            RibbonItemMap.AddRange(
                new RibbonItem[]{
                new RibbonItem(){
                    MenuItemID = 13,
                    MenuRibbonID=11,
                    DisplayOrder=0
                },
                new RibbonItem(){
                    MenuItemID = 14,
                    MenuRibbonID=11,
                    DisplayOrder=1
                },
                new RibbonItem(){
                    MenuItemID = 15,
                    MenuRibbonID=12,
                    DisplayOrder=0
                },
                new RibbonItem(){
                    MenuItemID = 16,
                    MenuRibbonID=12,
                    DisplayOrder=1
                }
                ,
                new RibbonItem(){
                    MenuItemID = 17,
                    MenuRibbonID=12,
                    DisplayOrder=2
                },
                new RibbonItem(){
                    MenuItemID = 18,
                    MenuRibbonID=13,
                    DisplayOrder=0
                },
                new RibbonItem(){
                    MenuItemID = 19,
                    MenuRibbonID=14,
                    DisplayOrder=0
                }
                ,
                new RibbonItem(){
                    MenuItemID = 20,
                    MenuRibbonID=14,
                    DisplayOrder=1
                },
                new RibbonItem(){
                    MenuItemID = 21,
                    MenuRibbonID=15,
                    DisplayOrder=0
                },
                new RibbonItem(){
                    MenuItemID = 22,
                    MenuRibbonID=15,
                    DisplayOrder=1
                }
                ,
                new RibbonItem(){
                    MenuItemID = 23,
                    MenuRibbonID=16,
                    DisplayOrder=0
                },
                new RibbonItem(){
                    MenuItemID = 24,
                    MenuRibbonID=16,
                    DisplayOrder=1
                },
                new RibbonItem(){
                    MenuItemID = 25,
                    MenuRibbonID=16,
                    DisplayOrder=2
                }
                ,
                new RibbonItem(){
                    MenuItemID = 26,
                    MenuRibbonID=17,
                    DisplayOrder=0
                },
                new RibbonItem(){
                    MenuItemID = 27,
                    MenuRibbonID=19,
                    DisplayOrder=0
                },
                new RibbonItem(){
                    MenuItemID = 28,
                    MenuRibbonID=19,
                    DisplayOrder=1
                }
                ,
                new RibbonItem(){
                    MenuItemID = 29,
                    MenuRibbonID=19,
                    DisplayOrder=2
                },
                new RibbonItem(){
                    MenuItemID = 30,
                    MenuRibbonID=20,
                    DisplayOrder=0
                },
                new RibbonItem(){
                    MenuItemID = 31,
                    MenuRibbonID=20,
                    DisplayOrder=1
                }
                ,
                new RibbonItem(){
                    MenuItemID = 32,
                    MenuRibbonID=20,
                    DisplayOrder=2
                }
            }
                );
        }
        public static void AddSalesMarketing()
        {
            items.AddRange(
            new MenuItem[]{
                new MenuItem
                        {
                            ID=33,
                            Name = "Affiliates",
                            URLOrDiv = "/Home/Index/",
                            Icon="/Content/ribbon/images/ecommerce/marketing_Affiliates.png",
                            DisplayOrder=32
                        },
                        new MenuItem
                        {
                            ID=34,
                            Name = "Newsletter",
                            URLOrDiv = "/Home/Index/",
                            Icon="/Content/ribbon/images/ecommerce/marketing_Newsletters.png",
                            DisplayOrder=33
                        },
                        new MenuItem
                        {
                            ID=35,
                            Name = "Campaigns",
                            URLOrDiv = "/Home/Index/",
                            Icon="/Content/ribbon/images/ecommerce/marketing_campaign.png",
                            DisplayOrder=34
                        },
                        new MenuItem
                        {
                            ID=36,
                            Name = "Discounts",
                            URLOrDiv = "/Home/Index/",
                            Icon="/Content/ribbon/images/ecommerce/marketing_discount.png",
                            DisplayOrder=35
                        },
                        new MenuItem
                        {
                            ID=37,
                            Name = "Promotion",
                            URLOrDiv = "/Home/Index/",
                            Icon="/Content/ribbon/images/ecommerce/marketing_promationfeeds.png",
                            DisplayOrder=36
                        },
                        new MenuItem
                        {
                            ID=38,
                            Name = "Orders",
                            URLOrDiv = "/Home/Index/",
                            Icon="/Content/ribbon/images/ecommerce/sales_orders.png",
                            DisplayOrder=37
                        },
                        new MenuItem
                        {
                            ID=39,
                            Name = "Return requests",
                            URLOrDiv = "/Home/Index/",
                            Icon="/Content/ribbon/images/ecommerce/sales_Return_requests.png",
                            DisplayOrder=38
                        },
                        new MenuItem
                        {
                            ID=40,
                            Name = "Gift cards",
                            URLOrDiv = "/Home/Index/",
                            Icon="/Content/ribbon/images/ecommerce/sales_giftcard.png",
                            DisplayOrder=39
                        },
                        new MenuItem
                        {
                            ID=41,
                            Name = "Bestsellers",
                            URLOrDiv = "/Home/Index/",
                            Icon="/Content/ribbon/images/ecommerce/sales_bestseller.png",
                            DisplayOrder=40
                        },
                        new MenuItem
                        {
                            ID=42,
                            Name = "Recurring payments",
                            URLOrDiv = "/Home/Index/",
                            Icon="/Content/ribbon/images/ecommerce/sales_Recurring_payments.png",
                            DisplayOrder=41
                        },
                        new MenuItem
                        {
                            ID=43,
                            Name = "Current shopping carts",
                            URLOrDiv = "/Home/Index/",
                            Icon="/Content/ribbon/images/ecommerce/sales_Current_shopping_carts.png",
                            DisplayOrder=42
                        },
                        new MenuItem
                        {
                            ID=44,
                            Name = "Marketing",
                            URLOrDiv = "/Home/Index/",
                            Icon="/Content/ribbon/images/ecommerce/marketing_report.png",
                            DisplayOrder=37
                        },
                        new MenuItem
                        {
                            ID=45,
                            Name = "Sales reports",
                            URLOrDiv = "/Home/Index/",
                            Icon="/Content/ribbon/images/ecommerce/sales_reports.png",
                            DisplayOrder=38
                        }
            }
            );

            RibbonItemMap.AddRange(
                new RibbonItem[]{
                new RibbonItem(){
                    MenuItemID = 33,
                    MenuRibbonID=23,
                    DisplayOrder=0
                },
                new RibbonItem(){
                    MenuItemID = 34,
                    MenuRibbonID=23,
                    DisplayOrder=1
                },
                new RibbonItem(){
                    MenuItemID = 35,
                    MenuRibbonID=23,
                    DisplayOrder=2
                },
                new RibbonItem(){
                    MenuItemID = 36,
                    MenuRibbonID=24,
                    DisplayOrder=0
                },
                new RibbonItem(){
                    MenuItemID = 37,
                    MenuRibbonID=24,
                    DisplayOrder=1
                },
                new RibbonItem(){
                    MenuItemID = 38,
                    MenuRibbonID=25,
                    DisplayOrder=0
                },
                new RibbonItem(){
                    MenuItemID = 39,
                    MenuRibbonID=26,
                    DisplayOrder=0
                },
                new RibbonItem(){
                    MenuItemID = 40,
                    MenuRibbonID=26,
                    DisplayOrder=1
                },
                new RibbonItem(){
                    MenuItemID = 41,
                    MenuRibbonID=26,
                    DisplayOrder=2
                },
                new RibbonItem(){
                    MenuItemID = 42,
                    MenuRibbonID=27,
                    DisplayOrder=0
                },
                new RibbonItem(){
                    MenuItemID = 43,
                    MenuRibbonID=27,
                    DisplayOrder=1
                },
                new RibbonItem(){
                    MenuItemID = 44,
                    MenuRibbonID=28,
                    DisplayOrder=0
                },
                new RibbonItem(){
                    MenuItemID = 45,
                    MenuRibbonID=29,
                    DisplayOrder=0
                }
            }
                );
        }
        public static void AddCRM()
        {
            items.AddRange(
            new MenuItem[]{
                new MenuItem
                        {
                            ID=46,
                            Name = "List",
                            URLOrDiv = "/Home/Index/",
                            Icon="/Content/ribbon/images/ecommerce/crm_Customers_List.png",
                            DisplayOrder=45
                        },
                        new MenuItem
                        {
                            ID=47,
                            Name = "Roles",
                            URLOrDiv = "/Home/Index/",
                            Icon="/Content/ribbon/images/ecommerce/crm_Customers_role.png",
                            DisplayOrder=46
                        },
                        new MenuItem
                        {
                            ID=48,
                            Name = "Online",
                            URLOrDiv = "/Home/Index/",
                            Icon="/Content/ribbon/images/ecommerce/crm_Customers_online.png",
                            DisplayOrder=47
                        },
                        new MenuItem
                        {
                            ID=49,
                            Name = "Reports",
                            URLOrDiv = "/Home/Index/",
                            Icon="/Content/ribbon/images/ecommerce/crm_Customers_report.png",
                            DisplayOrder=48
                        },
                        new MenuItem
                        {
                            ID=50,
                            Name = "Feedback",
                            URLOrDiv = "/Home/Index/",
                            Icon="/Content/ribbon/images/ecommerce/crm_customers_Feedback.png",
                            DisplayOrder=49
                        },
                        new MenuItem
                        {
                            ID=51,
                            Name = "Case study",
                            URLOrDiv = "/Home/Index/",
                            Icon="/Content/ribbon/images/ecommerce/crm_casestudy.png",
                            DisplayOrder=50
                        },
                        new MenuItem
                        {
                            ID=52,
                            Name = "Solutions",
                            URLOrDiv = "/Home/Index/",
                            Icon="/Content/ribbon/images/ecommerce/crm_casestudy_solution.png",
                            DisplayOrder=51
                        },
                        new MenuItem
                        {
                            ID=53,
                            Name = "Add",
                            URLOrDiv = "/Home/Index/",
                            Icon="/Content/ribbon/images/ecommerce/crm_contact_add.png",
                            DisplayOrder=52
                        },
                        new MenuItem
                        {
                            ID=54,
                            Name = "List",
                            URLOrDiv = "/Home/Index/",
                            Icon="/Content/ribbon/images/ecommerce/crm_contacts.png",
                            DisplayOrder=53
                        },
                        new MenuItem
                        {
                            ID=55,
                            Name = "Add",
                            URLOrDiv = "/Home/Index/",
                            Icon="/Content/ribbon/images/ecommerce/crm_contract_add.png",
                            DisplayOrder=54
                        },
                        new MenuItem
                        {
                            ID=56,
                            Name = "List",
                            URLOrDiv = "/Home/Index/",
                            Icon="/Content/ribbon/images/ecommerce/crm_contracts.png",
                            DisplayOrder=55
                        }
            }
            );

            RibbonItemMap.AddRange(
                new RibbonItem[]{
                new RibbonItem(){
                    MenuItemID = 46,
                    MenuRibbonID=31,
                    DisplayOrder=0
                },
                new RibbonItem(){
                    MenuItemID = 47,
                    MenuRibbonID=32,
                    DisplayOrder=0
                },
                new RibbonItem(){
                    MenuItemID = 48,
                    MenuRibbonID=32,
                    DisplayOrder=1
                },
                new RibbonItem(){
                    MenuItemID = 49,
                    MenuRibbonID=32,
                    DisplayOrder=2
                },
                new RibbonItem(){
                    MenuItemID = 50,
                    MenuRibbonID=33,
                    DisplayOrder=0
                },
                new RibbonItem(){
                    MenuItemID = 51,
                    MenuRibbonID=33,
                    DisplayOrder=1
                },
                new RibbonItem(){
                    MenuItemID = 52,
                    MenuRibbonID=33,
                    DisplayOrder=2
                },
                new RibbonItem(){
                    MenuItemID = 53,
                    MenuRibbonID=34,
                    DisplayOrder=0
                },
                new RibbonItem(){
                    MenuItemID = 54,
                    MenuRibbonID=34,
                    DisplayOrder=1
                },
                new RibbonItem(){
                    MenuItemID = 55,
                    MenuRibbonID=35,
                    DisplayOrder=0
                },
                new RibbonItem(){
                    MenuItemID = 56,
                    MenuRibbonID=35,
                    DisplayOrder=1
                }
            }
                );
        }
        public static void AddContent()
        {
            items.AddRange(
            new MenuItem[]{
                new MenuItem
                        {
                            ID=57,
                            Name = "Add",
                            URLOrDiv = "/Home/Index/",
                            Icon="/Content/ribbon/images/ecommerce/Menu_add.png",
                            DisplayOrder=56
                        },
                        new MenuItem
                        {
                            ID=58,
                            Name = "Ribbon",
                            URLOrDiv = "/Home/Index/",
                            Icon="/Content/ribbon/images/ecommerce/Menu_list.png",
                            DisplayOrder=57
                        },
                        new MenuItem
                        {
                            ID=59,
                            Name = "Items",
                            URLOrDiv = "/Home/Index/",
                            Icon="/Content/ribbon/images/ecommerce/Menu_Items.png",
                            DisplayOrder=58
                        },
                        new MenuItem
                        {
                            ID=60,
                            Name = "Add",
                            URLOrDiv = "/Home/Index/",
                            Icon="/Content/ribbon/images/ecommerce/News_add.png",
                            DisplayOrder=59
                        },
                        new MenuItem
                        {
                            ID=61,
                            Name = "List",
                            URLOrDiv = "/Home/Index/",
                            Icon="/Content/ribbon/images/ecommerce/News_List.png",
                            DisplayOrder=60
                        },
                        new MenuItem
                        {
                            ID=62,
                            Name = "Comments",
                            URLOrDiv = "/Home/Index/",
                            Icon="/Content/ribbon/images/ecommerce/News_comments.png",
                            DisplayOrder=61
                        },
                        new MenuItem
                        {
                            ID=63,
                            Name = "Post",
                            URLOrDiv = "/Home/Index/",
                            Icon="/Content/ribbon/images/ecommerce/Blog_add.png",
                            DisplayOrder=62
                        },
                        new MenuItem
                        {
                            ID=64,
                            Name = "List",
                            URLOrDiv = "/Home/Index/",
                            Icon="/Content/ribbon/images/ecommerce/Blog_list.png",
                            DisplayOrder=63
                        },
                        new MenuItem
                        {
                            ID=65,
                            Name = "Comments",
                            URLOrDiv = "/Home/Index/",
                            Icon="/Content/ribbon/images/ecommerce/Blog_comments.png",
                            DisplayOrder=64
                        },
                        new MenuItem
                        {
                            ID=66,
                            Name = "Add",
                            URLOrDiv = "/Home/Index/",
                            Icon="/Content/ribbon/images/ecommerce/forum_add.png",
                            DisplayOrder=65
                        },
                        new MenuItem
                        {
                            ID=67,
                            Name = "List",
                            URLOrDiv = "/Home/Index/",
                            Icon="/Content/ribbon/images/ecommerce/forum_list.png",
                            DisplayOrder=66
                        },
                        new MenuItem
                        {
                            ID=68,
                            Name = "Group",
                            URLOrDiv = "/Home/Index/",
                            Icon="/Content/ribbon/images/ecommerce/forum_group.png",
                            DisplayOrder=67
                        },
                        new MenuItem
                        {
                            ID=69,
                            Name = "Add",
                            URLOrDiv = "/Home/Index/",
                            Icon="/Content/ribbon/images/ecommerce/surveys_add.png",
                            DisplayOrder=68
                        },
                        new MenuItem
                        {
                            ID=70,
                            Name = "Build Q/A",
                            URLOrDiv = "/Home/Index/",
                            Icon="/Content/ribbon/images/ecommerce/surveys_qa.png",
                            DisplayOrder=69
                        },
                        new MenuItem
                        {
                            ID=71,
                            Name = "Build Survey",
                            URLOrDiv = "/Home/Index/",
                            Icon="/Content/ribbon/images/ecommerce/surveys_list.png",
                            DisplayOrder=70
                        },
                        new MenuItem
                        {
                            ID=72,
                            Name = "Setting",
                            URLOrDiv = "/Home/Index/",
                            Icon="/Content/ribbon/images/ecommerce/menu_setting.png",
                            DisplayOrder=71
                        },
                        new MenuItem
                        {
                            ID=73,
                            Name = "Setting",
                            URLOrDiv = "/Home/Index/",
                            Icon="/Content/ribbon/images/ecommerce/news_setting.png",
                            DisplayOrder=72
                        },
                        new MenuItem
                        {
                            ID=74,
                            Name = "Setting",
                            URLOrDiv = "/Home/Index/",
                            Icon="/Content/ribbon/images/ecommerce/blog_setting.png",
                            DisplayOrder=73
                        },
                        new MenuItem
                        {
                            ID=75,
                            Name = "Setting",
                            URLOrDiv = "/Home/Index/",
                            Icon="/Content/ribbon/images/ecommerce/forum_setting.png",
                            DisplayOrder=74
                        },
                        new MenuItem
                        {
                            ID=76,
                            Name = "Setting",
                            URLOrDiv = "/Home/Index/",
                            Icon="/Content/ribbon/images/ecommerce/surveys_setting.png",
                            DisplayOrder=75
                        },
                        new MenuItem
                        {
                            ID=77,
                            Name = "Add",
                            URLOrDiv = "/Home/Index/",
                            Icon="/Content/ribbon/images/ecommerce/content_contact_add.png",
                            DisplayOrder=76
                        },
                        new MenuItem
                        {
                            ID=78,
                            Name = "List",
                            URLOrDiv = "/Home/Index/",
                            Icon="/Content/ribbon/images/ecommerce/content_contact_list.png",
                            DisplayOrder=77
                        },
                        new MenuItem
                        {
                            ID=79,
                            Name = "Categories",
                            URLOrDiv = "/Home/Index/",
                            Icon="/Content/ribbon/images/ecommerce/content_contact_category.png",
                            DisplayOrder=78
                        }
            }
            );

            RibbonItemMap.AddRange(
                new RibbonItem[]{
                new RibbonItem(){
                    MenuItemID = 57,
                    MenuRibbonID=37,
                    DisplayOrder=0
                },
                new RibbonItem(){
                    MenuItemID = 58,
                    MenuRibbonID=38,
                    DisplayOrder=0
                },
                new RibbonItem(){
                    MenuItemID = 59,
                    MenuRibbonID=38,
                    DisplayOrder=1
                },
                new RibbonItem(){
                    MenuItemID = 72,
                    MenuRibbonID=38,
                    DisplayOrder=2
                },
                new RibbonItem(){
                    MenuItemID = 60,
                    MenuRibbonID=39,
                    DisplayOrder=0
                },
                new RibbonItem(){
                    MenuItemID = 61,
                    MenuRibbonID=40,
                    DisplayOrder=0
                },
                new RibbonItem(){
                    MenuItemID = 62,
                    MenuRibbonID=40,
                    DisplayOrder=1
                },
                new RibbonItem(){
                    MenuItemID = 73,
                    MenuRibbonID=40,
                    DisplayOrder=2
                },
                new RibbonItem(){
                    MenuItemID = 63,
                    MenuRibbonID=41,
                    DisplayOrder=0
                },
                new RibbonItem(){
                    MenuItemID = 64,
                    MenuRibbonID=42,
                    DisplayOrder=0
                },
                new RibbonItem(){
                    MenuItemID = 65,
                    MenuRibbonID=42,
                    DisplayOrder=1
                },
                new RibbonItem(){
                    MenuItemID = 74,
                    MenuRibbonID=42,
                    DisplayOrder=2
                },
                new RibbonItem(){
                    MenuItemID = 66,
                    MenuRibbonID=43,
                    DisplayOrder=0
                },
                new RibbonItem(){
                    MenuItemID = 67,
                    MenuRibbonID=44,
                    DisplayOrder=0
                },
                new RibbonItem(){
                    MenuItemID = 68,
                    MenuRibbonID=44,
                    DisplayOrder=1
                },
                new RibbonItem(){
                    MenuItemID = 75,
                    MenuRibbonID=44,
                    DisplayOrder=2
                },
                new RibbonItem(){
                    MenuItemID = 69,
                    MenuRibbonID=45,
                    DisplayOrder=0
                },
                new RibbonItem(){
                    MenuItemID = 70,
                    MenuRibbonID=46,
                    DisplayOrder=0
                },
                new RibbonItem(){
                    MenuItemID = 71,
                    MenuRibbonID=46,
                    DisplayOrder=1
                },
                new RibbonItem(){
                    MenuItemID = 76,
                    MenuRibbonID=46,
                    DisplayOrder=2
                },
                new RibbonItem(){
                    MenuItemID = 77,
                    MenuRibbonID=47,
                    DisplayOrder=0
                },
                new RibbonItem(){
                    MenuItemID = 78,
                    MenuRibbonID=48,
                    DisplayOrder=0
                },
                new RibbonItem(){
                    MenuItemID = 79,
                    MenuRibbonID=48,
                    DisplayOrder=1
                }
            }
                );
        }
        #endregion
    }
}
