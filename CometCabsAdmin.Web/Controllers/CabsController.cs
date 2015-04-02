using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CometCabsAdmin.Web.Controllers
{
    public class CabsController : Controller
    {
        // GET: Cabs
        public ActionResult Index()
        {
            return View();
        }
    }
}