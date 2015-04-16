using System.Collections.Generic;

namespace CometCabsAdmin.Model.Entities
{
    public class User : BaseEntity
    {
        // public User()
        // {
        //     UserRoles = new HashSet<UserRoles>();
        // }

        public string Username { get; set; }
        public string EmailAddress { get; set; }
        public string Password { get; set; }

        public virtual UserProfile UserProfile { get; set; }
        // public virtual ICollection<UserRoles> UserRoles { get; set; }
        public virtual UserRoles UserRole { get; set; }

        public virtual ICollection<CabActivity> CabActivity { get; set; }
    }
}
