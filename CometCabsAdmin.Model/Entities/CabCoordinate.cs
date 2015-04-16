using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CometCabsAdmin.Model.Entities
{
    public class CabCoordinate : BaseEntity
    {
        public long ActivityId { get; set; }
        public DateTime CurrentDateTime { get; set; }
        public float Longitude { get; set; }
        public float Latitude { get; set; }
        public int CurrentCapacity { get; set; }
        public string CurrentStatus { get; set; }

        [ForeignKey(name: "ActivityId")]
        public virtual CabActivity CabActivity { get; set; }
    }
}