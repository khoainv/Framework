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
using SSD.Framework;

namespace SSD.Web.Mvc.Controllers
{
    [AdminAuthorize(Groups = "Admin")]
    public class IoTClientsController : IoTClientsControllerBase
    {
        protected override void SetManager()
        {
            _iotManager = HttpContext.GetOwinContext().Get<IoTClientManager>();
        }
        public IoTClientsController() : base(null)
        {
        }
        public IoTClientsController(IoTClientManager grpManager) : base(grpManager)
        {
        }
    }
    public abstract class IoTClientsControllerBase : Controller
    {
        protected abstract void SetManager();
        protected IoTClientManagerBase _iotManager;
        public IoTClientsControllerBase(IoTClientManagerBase iotManager)
        {
            _iotManager = iotManager;
        }
        public IoTClientManagerBase IoTClientManager
        {
            get
            {
                if (_iotManager == null)
                    SetManager();
                return _iotManager;
            }
        }
        // GET: Admin/IoTClients
        public ActionResult Index(int? page)
        {
            int pageno = 0;
            pageno = page == null ? 1 : page.Value;
            int pageSize = UGConstants.DefaultPageSize;
            var query = IoTClientManager.IoTClients.Where(x => !x.Deleted).OrderBy(x => x.ClientId);
            Pager<IoTClient> pager = new Pager<IoTClient>(query, pageno, pageSize);
            return View(pager);
        }

        // GET: Admin/IoTClients/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            IoTClient client = await IoTClientManager.FindByIdAsync(id);
            if (client == null)
            {
                return HttpNotFound();
            }
            return View(client);
        }

        // GET: Admin/IoTClients/Create
        public ActionResult Create()
        {
            ViewBag.ClientSecret = IoTClientManager.NewSecret;
            return View();
        }

        // POST: Admin/IoTClients/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ClientSecret,ClientName,Description,DeviceType,Enable")] IoTClient client)
        {
            if (ModelState.IsValid)
            {
                await IoTClientManager.CreateAsync(client);
                return RedirectToAction("Index");
            }

            return View(client);
        }
        // GET: Admin/IoTClients/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            IoTClient client = await IoTClientManager.FindByIdAsync(id);
            if (client == null)
            {
                return HttpNotFound();
            }
            return View(client);
        }

        // POST: Admin/IoTClients/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ClientId,ClientSecret,ClientName,Description,DeviceType,Enable")] IoTClient client
            ,params string[] SelectedPermision)
        {
            IoTClient oldgrp = await IoTClientManager.FindByIdAsync(client.ClientId);
            if (oldgrp == null)
            {
                return HttpNotFound();
            }
            if (ModelState.IsValid)
            {
                IoTClientManager.Detach(oldgrp);
                var result = await IoTClientManager.UpdateAsync(client);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", result.Errors.First());
                }

            }
            return View(client);
        }

        // GET: Admin/IoTClients/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            IoTClient client = await IoTClientManager.FindByIdAsync(id);
            if (client == null)
            {
                return HttpNotFound();
            }
            return View(client);
        }

        // POST: Admin/IoTClients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            IoTClient client = await IoTClientManager.FindByIdAsync(id);
            if (client == null)
            {
                return HttpNotFound();
            }
            if (ModelState.IsValid)
            {
                client.Deleted = true;
                IoTClientManager.Detach(client);
                var result = await IoTClientManager.UpdateAsync(client);
                return RedirectToAction("Index");
            }
            return View(client);
        }
    }
}
