using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CometCabsAdmin.Model.Entities
{
    public class Route : BaseEntity
    {
        public string RouteName { get; set; }
        public string RouteDesc { get; set; }
        public bool IsActive { get; set; }
        public string RouteColor { get; set; }

        public virtual ICollection<RouteCoordinates> RouteCoordinates { get; set; }
    }
}
