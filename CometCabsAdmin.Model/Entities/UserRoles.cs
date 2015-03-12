using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace CometCabsAdmin.Model.Entities
{
    public class UserRoles : BaseEntity
    {
        // public long UserId { get; set; }
        public string RoleName { get; set; }        
        
        public virtual User User { get; set; }       
    }
}
