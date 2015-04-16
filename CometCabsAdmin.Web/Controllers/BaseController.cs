using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CometCabsAdmin.Web.Controllers
{
    public class BaseController : Controller
    {
        public string IPAddress
        {
            get
            {
                return Request.ServerVariables["REMOTE_ADDR"];
            }
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
        }
    }
}