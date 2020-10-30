using IdentityServer3.EntityFramework.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace SSOWeb.Models
{
    public class ClientViewModels
    {
        public static IEnumerable<SelectListItem> SelectedClientScopes(IEnumerable<Scope> lstScope, IEnumerable<string> selected )
        {
            if (lstScope == null)
                return null;
            if (selected == null)
                return lstScope.Select(x => new SelectListItem() { Text = x.Name, Value = x.Id.ToString(), Selected = false });
            return lstScope.Select(x => new SelectListItem() { Text = x.Name, Value = x.Id.ToString(), Selected = selected.Contains(x.Name) });
        }
    }
}
