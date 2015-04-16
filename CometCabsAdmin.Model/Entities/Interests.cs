using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CometCabsAdmin.Model.Entities
{
    public class Interests : BaseEntity
    {
        public DateTime FlagTime { get; set; }
        public float Longitude { get; set; }
        public float Latitude { get; set; }
        public Nullable<DateTime> PickedUpTime { get; set; }
    }
}
