using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace CometCabsAdmin.Web
{
    public class CometCabsAuthorizeAttribute : AuthorizeAttribute
    {
        protected virtual CustomPrincipal CurrentUser
        {
            get
            {
                return HttpContext.Current.User as CustomPrincipal;
            }
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.Request.IsAuthenticated)
            {
                string authorizedUsers = ConfigurationManager.AppSettings["UsersConfigKey"];
                string authorizedRoles = ConfigurationManager.AppSettings["RolesConfigKey"];

                base.Users = string.IsNullOrEmpty(base.Users) ? authorizedUsers : base.Users;
                base.Roles = string.IsNullOrEmpty(base.Roles) ? authorizedRoles : base.Roles;

                if (!String.IsNullOrEmpty(base.Roles))
                {
                    if (!CurrentUser.IsInRole(base.Roles))
                    {
                        filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Error", action = "AccessDenied" }));
                    }
                }

                if (!String.IsNullOrEmpty(base.Users))
                {
                    if (!base.Users.Contains(CurrentUser.UserId.ToString()))
                    {

                        filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Error", action = "AccessDenied" }));
                    }
                }
            }
            else
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(
                    new { 
                        controller = "Account", 
                        action = "Login", 
                        returnUrl = HttpContext.Current.Request.Url.AbsoluteUri  
                    }));
            }
        }
    }
}
