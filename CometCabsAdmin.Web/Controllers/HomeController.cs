using CometCabsAdmin.Model;
using CometCabsAdmin.Model.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CometCabsAdmin.Web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private IUserService _userService;

        public HomeController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public ActionResult Index()
        {
            List<User> users = _userService.GetUsers().ToList();
            return View(users);
        }
    }
}