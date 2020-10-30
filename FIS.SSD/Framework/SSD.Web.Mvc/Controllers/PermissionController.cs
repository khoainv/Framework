using SSD.Web.Mvc.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Data.Entity;
using System;
using SSD.Web.Identity;
using SSD.Web.Models;
using System.Reflection;
using System.IO;
using SSD.Framework;
using SSD.Web.Security;
using SSD.Framework.Security;

namespace SSD.Web.Mvc.Controllers
{
    [AdminAuthorize(Groups = "Admin")]
    public class PermissionController : PermissionControllerBase
    {
        protected override void SetManager()
        {
            _perManager = HttpContext.GetOwinContext().Get<PermissionManager>();
        }
        public PermissionController() : base(null)
        {
        }
        public PermissionController(PermissionManager perManager) : base(perManager)//, grpManager)
        {
        }
    }
    public abstract class PermissionControllerBase : Controller
    {
        protected abstract void SetManager();
        protected PermissionManagerBase _perManager;
        public PermissionControllerBase(PermissionManagerBase perManager)
        {
            _perManager = perManager;
        }
        public PermissionManagerBase PermissionManager
        {
            get
            {
                if (_perManager == null)
                    SetManager();
                return _perManager;
            }
        }
        protected List<Permission> Permissions
        {
            get
            {
                List<Permission> lst = new List<Permission>();

                //Host Assembly
                Assembly mscorlib = System.Web.Compilation.BuildManager.GetGlobalAsaxType().BaseType.Assembly;
                lst.AddRange(GetListPermissions(mscorlib));

                //Assembly.GetExecutingAssembly() with Current Dll Exc
                if (!string.IsNullOrWhiteSpace(UGConstants.UGControllerAssembly))
                {
                    var lstFile = UGConstants.UGControllerAssembly.Split(new char[] { ';' });
                    //get assemblies in directory.
                    string folder = Path.Combine(HttpContext.Server.MapPath("~/"), "bin");
                    var files = Directory.GetFiles(folder, "*.dll");
                    //load each assembly.
                    foreach (string file in files)
                    {
                        var fileName = Path.GetFileName(file);
                        if(lstFile.Contains(fileName))
                        {
                            var assembly = Assembly.LoadFile(file);
                            lst.AddRange(GetListPermissions(assembly));
                        }
                    }
                }

                return lst.Distinct().ToList();
            }
        }
        private List<Permission> GetListPermissions(Assembly mscorlib)
        {
            List<Permission> lst = new List<Permission>();
            foreach (Type type in mscorlib.GetTypes())
            {
                if (typeof(UGControllerBase).IsAssignableFrom(type))//type.BaseType.BaseType != null && type.BaseType.BaseType.Equals(typeof(UGControllerBase)))
                {
                    var lstObjAttr = type.GetCustomAttributes(typeof(UGPermissionAttribute), false);
                    //if (lstObjAttr.Length > 0)
                    //{
                    foreach (var md in type.GetMethods())
                    {
                        var lstAttAction = md.GetCustomAttributes(typeof(UGPermissionAttribute), false);
                        if (lstAttAction.Length > 0)
                        {
                            lst.Add(
                                new Permission()
                                {
                                    ControllerKey = lstObjAttr.Length > 0 ? (lstObjAttr.First() as UGPermissionAttribute).Key : type.Name,
                                    AcctionKey = (lstAttAction.First() as UGPermissionAttribute).Key,
                                    Description = (lstAttAction.First() as UGPermissionAttribute).Name
                                }
                                );
                        }
                    }
                    //}

                }
            }
            return lst;
        }
        public async Task<ActionResult> Index()
        {
            ListPermissionViewModel model = new ListPermissionViewModel();
            model.PermissionList = await PermissionManager.Permissions.OrderBy(x=>x.ControllerKey).ToListAsync();
            return View(model);
        }
        [HttpPost]
        public ActionResult UpdatePermission()
        {
            PermissionManager.UpdatePermission(Permissions);
            return RedirectToAction("Index");
        }
        // GET: Admin/Groups/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var per = await PermissionManager.FindByIdAsync(id);
            if (per == null)
            {
                return HttpNotFound();
            }

            ViewBag.Users = await PermissionManager.FindUserGroupByIdAsync(id);
            ViewBag.Groups = await PermissionManager.FindGroupByIdAsync(id);
            return View(per);
        }

    }
}
