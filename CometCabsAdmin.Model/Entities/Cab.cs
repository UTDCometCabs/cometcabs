using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CometCabsAdmin.Model.Entities
{
    public class Cab : BaseEntity
    {
        public string CabCode { get; set; }
        public string CabDesc { get; set; }
        public int Capacity { get; set; }
    }
}
