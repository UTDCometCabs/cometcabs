using System.ComponentModel.DataAnnotations;

namespace CometCabsAdmin.Web.Models
{
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "User name")]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me on this computer")]
        public bool RememberMe { get; set; }
    }
}