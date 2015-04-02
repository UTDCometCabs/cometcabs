using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Newtonsoft.Json;
using CometCabsAdmin.Model.Entities;
using CometCabsAdmin.Model.Contracts;
using CometCabsAdmin.Web.Models;

namespace CometCabsAdmin.Web.Controllers
{
    public class AccountController : BaseController
    {
        private IEncryption _encryption;
        private IUserService _userService;

        public AccountController(
            IUserService userService
            , IEncryption encryption
            )
        {

            _userService = userService;
            _encryption = encryption;
        }

        [HttpGet]
        public ActionResult Index()
        {
            return RedirectToAction("Login");
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        // [CometCabsAuthorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult UserAccount()
        {
            //User user = _userService.GetUsers()

            return View();
        }

        [HttpPost]
        public ActionResult UserAccount([Bind(Exclude = "Id")]UserAccountViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    User user = new User
                    {
                        Username = model.UserName,
                        EmailAddress = model.EmailAddress,
                        Password = _encryption.Encrypt(model.Password),
                        CreatedBy = HttpContext.User.Identity.Name,
                        CreateDate = DateTime.Now,
                        IPAddress = IPAddress,
                        UserProfile = new UserProfile
                        {
                            FirstName = model.FirstName,
                            LastName = model.LastName,
                            Address = model.Address,
                            IPAddress = IPAddress,
                        },
                        UserRole = new UserRoles
                        {
                            RoleName = model.RoleName,
                            CreatedBy = HttpContext.User.Identity.Name,
                            CreateDate = DateTime.Now,
                            IPAddress = IPAddress,
                        }
                    };

                    if (model.Id > 0)
                    {
                        user.Id = model.Id;
                        user.UpdateDate = DateTime.Now;
                        user.UpdatedBy = HttpContext.User.Identity.Name;
                        user.UserProfile.UpdateDate = DateTime.Now;
                        user.UserProfile.UpdatedBy = HttpContext.User.Identity.Name;
                        user.UserRole.UpdateDate = DateTime.Now;
                        user.UserRole.UpdatedBy = HttpContext.User.Identity.Name;

                        _userService.UpdateUser(user);
                    }
                    else
                    {
                        if (!_userService.GetUsers().Any(s => (s.Username == model.UserName)))
                        {
                            user.CreateDate = DateTime.Now;
                            user.CreatedBy = HttpContext.User.Identity.Name;
                            user.UserProfile.CreateDate = DateTime.Now;
                            user.UserProfile.CreatedBy = HttpContext.User.Identity.Name;
                            user.UserRole.CreateDate = DateTime.Now;
                            user.UserRole.CreatedBy = HttpContext.User.Identity.Name;

                            _userService.InsertUser(user);
                        }
                        else
                        {
                            ModelState.AddModelError("Error", string.Format("{0} already exists", model.UserName));
                        }
                    }

                    return RedirectToAction("UserAccount");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("Error", ex.Message);
                }
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                string password = _encryption.Encrypt(model.Password);
                User user = _userService.GetUsers().Where(u => u.Username == model.Username && u.Password == password).FirstOrDefault();

                if (user != null)
                {
                    string[] roles = new string[] { user.UserRole.RoleName };

                    CustomPrincipalSerializeModel serializeModel = new CustomPrincipalSerializeModel();
                    serializeModel.UserId = user.Id;
                    serializeModel.FirstName = user.UserProfile.FirstName;
                    serializeModel.LastName = user.UserProfile.LastName;
                    serializeModel.Roles = roles;

                    string userData = JsonConvert.SerializeObject(serializeModel);
                    FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(1, user.Username, DateTime.Now, DateTime.Now.AddMinutes(15), model.RememberMe, userData);
                    string encTicket = FormsAuthentication.Encrypt(authTicket);
                    HttpCookie faCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);

                    Response.Cookies.Add(faCookie);

                    if (roles.Contains("Admin"))
                    {
                        if (!string.IsNullOrWhiteSpace(model.ReturnUrl))
                        {
                            return Redirect(model.ReturnUrl);
                        }
                        else
                        {
                            return RedirectToAction("Index", "Home");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Administrator priviledge is required to view this page");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Incorrect username and/or password");
                }
            }

            return View(model);
        }

        [AllowAnonymous]
        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Account", null);
        }
    }
}