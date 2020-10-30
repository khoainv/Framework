using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.IO;
using SSD.Framework;

namespace SSD.Web.Extensions
{
    public static class OwinEnvironmentExtensions
    {
        public static string GetSignOutMessageId(this IDictionary<string, object> env)
        {
            if (env == null) throw new ArgumentNullException("env");

            var ctx = new OwinContext(env);
            return ctx.Request.Query.Get(UGConstants.Authentication.SignoutId);
        }
    }
}
