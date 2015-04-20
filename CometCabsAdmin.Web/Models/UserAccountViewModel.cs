using CometCabsAdmin.Model.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;

namespace CometCabsAdmin.Web.Models
{
    public class UserAccountViewModel
    {
        //private User _user;

        //public UserAccountViewModel(User user)
        //{
        //    _user = user;
        //}
        public int Id { get; set; }

        [Display(Name = "User Name")]
        [Required(ErrorMessage = "Please enter a user name")]
        public string UserName { get; set; }

        [Display(Name = "First Name")]
        [Required(ErrorMessage = "Please enter your first name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "Please enter your last name")]
        public string LastName { get; set; }

        [Display(Name = "Email Address")]
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "Please enter your email address")]
        public string EmailAddress { get; set; }

        public string Address { get; set; }

        [Display(Name = "Role Name")]
        public string RoleName { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        public string PasswordConfirm { get; set; }

        public List<UserTable> UserTable { get; set; }
    }

    [Serializable]
    public class UserTable
    {
        public long Id { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string RoleName { get; set; }
        public string Address { get; set; }
        public SelectListItem SelectedRole { get; set; }
    }
}