using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using CometCabsAdmin.Model.Contracts;
using CometCabsAdmin.Model.Entities;

namespace CometCabsAdmin.Web.Controllers
{
    [CometCabsAuthorize(Roles="Admin")]
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