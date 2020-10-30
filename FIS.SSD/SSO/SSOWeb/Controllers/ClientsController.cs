using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using IdentityServer3.EntityFramework.Entities;
using SSOWeb.Areas.Admin.Controllers;
using Microsoft.AspNet.Identity.Owin;
using SSD.SSO.Identity;
using SSD.SSO;
using SSOWeb.Models;
using SSOWeb.Base;

namespace SSOWeb.Controllers
{
    [UGAuthorize(Key = "ClientsController", Name = "Quản lý Clients")]
    public class ClientsController : BaseController
    {
        private ClientManager _clientManager;
        //Ioc
        public ClientsController()
        {
        }
        public ClientsController(ClientManager clientManager)
        {
            ClientManager = clientManager;
        }
        //Ioc Impl
        public ClientManager ClientManager
        {
            get
            {
                return _clientManager ?? new ClientManager(HttpContext.GetOwinContext().Get<ApplicationDbContext>());
            }
            private set
            {
                _clientManager = value;
            }
        }
       
        // GET: Admin/Clients
        [UGAuthorize(Key = "ClientsController.Index", Name = "Danh sách Clients")]
        public ActionResult Index()
        {
            return View(ClientManager.ListClients());
        }

        // GET: Admin/Clients/Details/5
        [UGAuthorize(Key = "ClientsController.Details", Name = "Chi tiết Client")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Client client = ClientManager.FindById(id);
            if (client == null)
            {
                return HttpNotFound();
            }
            return View(client);
        }
        [UGFollowAuthorize(FollowKey = "ClientsController.Create")]
        public ActionResult CreateResourceOwner()
        {
            Client client = ClientManager.GetClientDefaultResourceOwner();
            var clientScopes = client.AllowedScopes.Select(x => x.Scope);
            ViewBag.Scopes = ClientViewModels.SelectedClientScopes(ClientManager.Scopes, clientScopes);//.Select(x => new SelectListItem() { Text = x.Name, Value = x.Id.ToString(), Selected = clientScopes.Contains(x.Name) });
            ViewBag.ClientSecret = ClientManager.NewSecret;
            return View(client);
        }

        // POST: Admin/Clients/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ValidateAntiForgeryToken]
        [UGFollowAuthorize(FollowKey = "ClientsController.Create")]
        public ActionResult CreateResourceOwner([Bind(Include = "Id,Enabled,ClientId,ClientName,ClientUri,LogoUri,RequireConsent,AllowRememberConsent,Flow,AllowClientCredentialsOnly,AllowAccessToAllScopes,IdentityTokenLifetime,AccessTokenLifetime,AuthorizationCodeLifetime,AbsoluteRefreshTokenLifetime,SlidingRefreshTokenLifetime,RefreshTokenUsage,UpdateAccessTokenOnRefresh,RefreshTokenExpiration,AccessTokenType,EnableLocalLogin,IncludeJwtId,AlwaysSendClientClaims,PrefixClientClaims,AllowAccessToAllGrantTypes")] Client client,
            string ClientSecret,
            params string[] selectedScopes)
        {
            if (string.IsNullOrWhiteSpace(ClientSecret))
            {
                ModelState.AddModelError("ClientSecret", "Trường bắt buộc phải nhập!");
            }
            if (ModelState.IsValid)
            {
                ClientManager.Create(client, ClientSecret,null,null, selectedScopes);
                return RedirectToAction("Index");
            }
            ViewBag.ClientSecret = ClientManager.NewSecret;
            ViewBag.Scopes = ClientViewModels.SelectedClientScopes(ClientManager.Scopes, selectedScopes);
            return View(client);
        }
        // GET: Admin/Clients/Create
        [UGAuthorize(Key = "ClientsController.Create", Name = "Tạo mới Client")]
        public ActionResult Create()
        {
            Client client = ClientManager.GetClientDefaultImplicit();
            var clientScopes = client.AllowedScopes.Select(x => x.Scope);
            ViewBag.Scopes = ClientViewModels.SelectedClientScopes(ClientManager.Scopes, clientScopes);//.Select(x => new SelectListItem() { Text = x.Name, Value = x.Id.ToString(), Selected = clientScopes.Contains(x.Name) });
            ViewBag.ClientSecret = ClientManager.NewSecret;
            return View(client);
        }

        // POST: Admin/Clients/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ValidateAntiForgeryToken]
        [UGFollowAuthorize(FollowKey = "ClientsController.Create")]
        public ActionResult Create([Bind(Include = "Id,Enabled,ClientId,ClientName,ClientUri,LogoUri,RequireConsent,AllowRememberConsent,Flow,AllowClientCredentialsOnly,AllowAccessToAllScopes,IdentityTokenLifetime,AccessTokenLifetime,AuthorizationCodeLifetime,AbsoluteRefreshTokenLifetime,SlidingRefreshTokenLifetime,RefreshTokenUsage,UpdateAccessTokenOnRefresh,RefreshTokenExpiration,AccessTokenType,EnableLocalLogin,IncludeJwtId,AlwaysSendClientClaims,PrefixClientClaims,AllowAccessToAllGrantTypes")] Client client,
            string ClientSecret,
            string RedirectUri,
            string PostLogoutRedirectUri,
            params string[] selectedScopes)
        {
            if (string.IsNullOrWhiteSpace(ClientSecret))
            {
                ModelState.AddModelError("ClientSecret", "Trường bắt buộc phải nhập!");
            }
            if (string.IsNullOrWhiteSpace(RedirectUri))
            {
                ModelState.AddModelError("RedirectUri", "Trường bắt buộc phải nhập!");
            }
            if (string.IsNullOrWhiteSpace(PostLogoutRedirectUri))
            {
                ModelState.AddModelError("PostLogoutRedirectUri", "Trường bắt buộc phải nhập!");
            }
            if (ModelState.IsValid)
            {
                ClientManager.Create(client, ClientSecret, RedirectUri, PostLogoutRedirectUri, selectedScopes);
                return RedirectToAction("Index");
            }
            ViewBag.ClientSecret = ClientManager.NewSecret;
            ViewBag.Scopes = ClientViewModels.SelectedClientScopes(ClientManager.Scopes, selectedScopes);
            return View(client);
        }

        // GET: Admin/Clients/Edit/5
        [UGAuthorize(Key = "ClientsController.Edit", Name = "Sửa Client")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Client client = ClientManager.FindById(id);
            if (client == null)
            {
                return HttpNotFound();
            }
            var clientScopes = client.AllowedScopes.Select(x => x.Scope);
            ViewBag.Scopes = ClientManager.Scopes.Select(x => new SelectListItem() { Text = x.Name, Value = x.Id.ToString(), Selected = clientScopes.Contains(x.Name) });
            ViewBag.ClientSecret = client.ClientSecrets.First().Description;
            ViewBag.RedirectUris = client.RedirectUris.Select(x=>x.Uri).ToList();
            ViewBag.PostLogoutRedirectUris = client.PostLogoutRedirectUris.Select(x => x.Uri).ToList();

            return View(client);
        }

        // POST: Admin/Clients/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ValidateAntiForgeryToken]
        [UGFollowAuthorize(FollowKey = "ClientsController.Edit")]
        public ActionResult Edit([Bind(Include = "Id,Enabled,ClientId,ClientName,ClientUri,LogoUri,RequireConsent,AllowRememberConsent,Flow,AllowClientCredentialsOnly,AllowAccessToAllScopes,IdentityTokenLifetime,AccessTokenLifetime,AuthorizationCodeLifetime,AbsoluteRefreshTokenLifetime,SlidingRefreshTokenLifetime,RefreshTokenUsage,UpdateAccessTokenOnRefresh,RefreshTokenExpiration,AccessTokenType,EnableLocalLogin,IncludeJwtId,AlwaysSendClientClaims,PrefixClientClaims,AllowAccessToAllGrantTypes")] Client client,
            string ClientSecret,
            [Bind(Include = "selectedScopes")]  string[] selectedScopes,
            [Bind(Include = "RedirectUris")]  string[] RedirectUris,
            [Bind(Include = "PostLogoutRedirectUris")]  string[] PostLogoutRedirectUris)
        {
            if(client.ClientId== SSOISConstants.LocalClientId)
            {
                ModelState.AddModelError("ClientId", "Không được sửa Local Client. Liên hệ quản trị để biết thêm chi tiết!");
            }
            if (ModelState.IsValid)
            {
                var lstUriRed = RedirectUris.Distinct().Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();
                var lstUriPost = PostLogoutRedirectUris.Distinct().Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();

                var clientDB = ClientManager.FindById(client.Id);
                clientDB.Enabled = client.Enabled;
                clientDB.ClientId = client.ClientId;
                clientDB.ClientName = client.ClientName;
                clientDB.ClientUri = client.ClientUri;
                clientDB.LogoUri = client.LogoUri;
                clientDB.RequireConsent = client.RequireConsent;
                clientDB.AllowRememberConsent = client.AllowRememberConsent;
                clientDB.Flow = client.Flow;
                clientDB.AllowClientCredentialsOnly = client.AllowClientCredentialsOnly;
                clientDB.AllowAccessToAllScopes = client.AllowAccessToAllScopes;
                clientDB.IdentityTokenLifetime = client.IdentityTokenLifetime;
                clientDB.AccessTokenLifetime = client.AccessTokenLifetime;
                clientDB.AuthorizationCodeLifetime = client.AuthorizationCodeLifetime;
                clientDB.AbsoluteRefreshTokenLifetime = client.AbsoluteRefreshTokenLifetime;
                clientDB.SlidingRefreshTokenLifetime = client.SlidingRefreshTokenLifetime;
                clientDB.RefreshTokenUsage = client.RefreshTokenUsage;
                clientDB.UpdateAccessTokenOnRefresh = client.UpdateAccessTokenOnRefresh;
                clientDB.RefreshTokenExpiration = client.RefreshTokenExpiration;
                clientDB.AccessTokenType = client.AccessTokenType;
                clientDB.EnableLocalLogin = client.EnableLocalLogin;
                clientDB.IncludeJwtId = client.IncludeJwtId;
                clientDB.AlwaysSendClientClaims = client.AlwaysSendClientClaims;
                clientDB.PrefixClientClaims = client.PrefixClientClaims;
                clientDB.AllowAccessToAllGrantTypes = client.AllowAccessToAllGrantTypes;

                ClientManager.Detach(clientDB);
                ClientManager.Update(clientDB, ClientSecret, selectedScopes, lstUriRed, lstUriPost);

                return RedirectToAction("Index");
            }

            ViewBag.Scopes = ClientViewModels.SelectedClientScopes(ClientManager.Scopes, selectedScopes);
            ViewBag.ClientSecret = ClientSecret;
            ViewBag.RedirectUris = RedirectUris;
            ViewBag.PostLogoutRedirectUris = PostLogoutRedirectUris;

            return View(client);
        }

        // GET: Admin/Clients/Delete/5
        [UGAuthorize(Key = "ClientsController.Delete", Name = "Xóa Client")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Client client = ClientManager.FindById(id);
            if (client == null)
            {
                return HttpNotFound();
            }
            return View(client);
        }

        // POST: Admin/Clients/Delete/5
        [HttpPost, ValidateAntiForgeryToken, ActionName("Delete")]
        [UGFollowAuthorize(FollowKey = "ClientsController.Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Client client = ClientManager.FindById(id);
            if (client.ClientId == SSOISConstants.LocalClientId)
            {
                ModelState.AddModelError("ClientId", "Không được xóa Local Client. Liên hệ quản trị để biết thêm chi tiết!");
                return View(client);
            }

            ClientManager.Delete(client);
            return RedirectToAction("Index");
        }

    }
}
