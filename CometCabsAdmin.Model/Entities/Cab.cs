using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CometCabsAdmin.Model.Entities
{
    public class Cab : BaseEntity
    {
        public string CabCode { get; set; }
        public string CabDesc { get; set; }
        public int MaxCapacity { get; set; }
        public string OnDutyStatus { get; set; }

        public virtual ICollection<CabActivity> CabActivity { get; set; }
    }
}
