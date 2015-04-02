using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CometCabsAdmin.Model.Entities
{
    public class RouteCoordinates : BaseEntity, IEnumerable<RouteCoordinates>
    {
        public float Latitude { get; set; }
        public float Longitude { get; set; }
        public long RouteId { get; set; }

        [ForeignKey(name: "RouteId")]
        public virtual Route Route { get; set; }

        #region IEnumerable<RouteCoordinates> Members

        public IEnumerator<RouteCoordinates> GetEnumerator()
        {
            return new List<RouteCoordinates>().GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)this.GetEnumerator();
        }

        #endregion
    }
}
