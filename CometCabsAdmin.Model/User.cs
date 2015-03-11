using System.ComponentModel.DataAnnotations;

namespace CometCabsAdmin.Model
{
    public class User : BaseEntity
    {
        [Display(Name="User Name")]
        public string Username { get; set; }

        public string EmailAddress { get; set; }

        [Display(Name="Password")]
        public string Password { get; set; }

        public virtual UserProfile UserProfile { get; set; }
    }
}
