﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Newtonsoft.Json;
using CometCabsAdmin.Model.Entities;
using CometCabsAdmin.Model.Contracts;
using CometCabsAdmin.Web.Models;
//using Microsoft.Practices.Unity;

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
            return RedirectToAction("UserAccount");
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpGet]
        public ActionResult UserAccount()
        {
            List<User> users = _userService.GetUsers().ToList();
            List<UserTable> table = new List<UserTable>();

            foreach (User user in users)
            {
                table.Add(new UserTable
                {
                    Id = user.Id,
                    UserName = user.Username,
                    FullName = user.UserProfile.NameLastFirst,
                    FirstName = user.UserProfile.FirstName,
                    LastName = user.UserProfile.LastName,
                    EmailAddress = user.EmailAddress,
                    RoleName = user.UserRole.RoleName,
                    Address = user.UserProfile.Address,
                    SelectedRole = new SelectListItem
                    {
                        Value = user.UserRole.RoleName,
                        Text = user.UserRole.RoleName == "Admin" ? "Administrator" : user.UserRole.RoleName,
                    },
                });
            }

            UserAccountViewModel model = new UserAccountViewModel
            {
                UserTable = table,
            };

            return View(model);
        }

        [HttpPost]
        public ActionResult SaveUser(string id, string userName, string firstName, string lastName, string emailAddress, string role, string password1, string password2, string address)
        {
            string message = string.Empty;

            if (ModelState.IsValid)
            {
                try
                {
                    SelectListItem selectedRole = JsonConvert.DeserializeObject<SelectListItem>(role);

                    if (password1.SequenceEqual(password2))
                    {
                        long userId = 0;

                        if (!string.IsNullOrWhiteSpace(id) && long.TryParse(id, out userId))
                        {
                            User user = _userService.GetUser(userId);

                            if (user == null)
                            {
                                if (!_userService.GetUsers().Any(s => (s.Username == userName)))
                                {
                                    user = new User
                                   {
                                       Username = userName,
                                       EmailAddress = emailAddress,
                                       Password = _encryption.Encrypt(password2),
                                       CreatedBy = HttpContext.User.Identity.Name,
                                       CreateDate = DateTime.Now,
                                       IPAddress = IPAddress,
                                       UserProfile = new UserProfile
                                       {
                                           FirstName = firstName,
                                           LastName = lastName,
                                           Address = address,
                                           CreatedBy = HttpContext.User.Identity.Name,
                                           CreateDate = DateTime.Now,
                                           IPAddress = IPAddress,
                                       },
                                       UserRole = new UserRoles
                                       {
                                           RoleName = selectedRole.Value,
                                           CreatedBy = HttpContext.User.Identity.Name,
                                           CreateDate = DateTime.Now,
                                           IPAddress = IPAddress,
                                       }
                                   };

                                    _userService.InsertUser(user);
                                }
                                else
                                {
                                    message = string.Format("{0} already exists", userName);
                                }
                            }
                            else
                            {
                                user.Username = userName;
                                user.EmailAddress = emailAddress;
                                user.UserProfile.FirstName = firstName;
                                user.UserProfile.LastName = lastName;
                                user.UserProfile.Address = address;
                                user.UserProfile.IPAddress = IPAddress;
                                user.UserRole.RoleName = selectedRole.Value;
                                user.UserRole.CreatedBy = HttpContext.User.Identity.Name;
                                user.UserRole.CreateDate = DateTime.Now;
                                user.UserRole.IPAddress = IPAddress;
                                user.UpdateDate = DateTime.Now;
                                user.UpdatedBy = HttpContext.User.Identity.Name;
                                user.UserProfile.UpdateDate = DateTime.Now;
                                user.UserProfile.UpdatedBy = HttpContext.User.Identity.Name;
                                user.UserRole.UpdateDate = DateTime.Now;
                                user.UserRole.UpdatedBy = HttpContext.User.Identity.Name;

                                _userService.UpdateUser(user);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    message = string.Format("Error: {0}, stack trace: {1}", ex.Message, ex.StackTrace);
                }
            }

            return Content(message);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DeleteUser(string id)
        {
            User user = _userService.GetUser(long.Parse(id));

            if (user != null)
            {
                _userService.DeleteUser(user);
            }

            return View("Index", new MapViewModel());
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateAntiForgeryToken]
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