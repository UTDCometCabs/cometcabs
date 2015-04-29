using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CometCabsAdmin.Model.Entities
{
    public class CabActivity : BaseEntity
    {
        public long CabId { get; set; }
        public long DriverId { get; set; }
        public long RouteId { get; set; }
        public DateTime LoginTime { get; set; }
        public int TotalCapacity { get; set; }

        [ForeignKey(name: "CabId")]
        public virtual Cab Cab { get; set; }

        [ForeignKey(name: "DriverId")]
        public virtual User Driver { get; set; }

        [ForeignKey(name: "RouteId")]
        public virtual Route Route { get; set; }

        public virtual ICollection<CabCoordinate> CabCoordinate { get; set; }
    }
}
