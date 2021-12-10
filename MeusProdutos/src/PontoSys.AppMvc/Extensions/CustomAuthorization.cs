using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace PontoSys.AppMvc.Extensions
{
    public class CustomAuthorization
    {
        public static bool ValidarClaimsUsuario(string name, string value)
        {
            var identity = (ClaimsIdentity)HttpContext.Current.User.Identity;
            var claim = identity.Claims.FirstOrDefault(c => c.Type == name);
            return claim != null && claim.Value.Contains(value);
        }
    }
    public class ClaimAttribute : AuthorizeAttribute
    {
        private readonly string _claimName;
        private readonly string _claimValue;
        public ClaimAttribute(string claimName, string claimValue)
        {
            _claimName = claimName;
            _claimValue = claimValue;   
        }
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            return CustomAuthorization.ValidarClaimsUsuario(_claimName, _claimValue);   
        }
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.Request.IsAuthenticated)
            {
                filterContext.Result = new HttpStatusCodeResult((int)HttpStatusCode.Forbidden);
            }
            else
            {
                base.HandleUnauthorizedRequest(filterContext);
            }
        }
    }
}