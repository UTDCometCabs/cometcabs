using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CometCabsAdmin.Web.Controllers
{
    public class ErrorController : Controller
    {
        public ActionResult Index()
        {
            return View("An error has occured");
        }

        public ActionResult AccessDenied()
        {
            return View("Access denied.");
        }
    }
}