using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CometCabsAdmin.Web.Controllers
{
    [CometCabsAuthorize(Roles="Admin")]
    public class MapController : Controller
    {
        private float _longitude;
        private float _latitude;

        // GET: Map
        public ActionResult Index()
        {
            return View();
        }
    }
}