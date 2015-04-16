using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CometCabsAdmin.Model.Entities
{
    public class RouteDirections : BaseEntity, IEnumerable<RouteDirections>
    {
        public float Latitude { get; set; }
        public float Longitude { get; set; }
        public long RouteId { get; set; }
        public string TagName { get; set; }

        [ForeignKey(name: "RouteId")]
        public virtual Route Route { get; set; }

        #region IEnumerable<RouteDirections> Members

        public IEnumerator<RouteDirections> GetEnumerator()
        {
            return new List<RouteDirections>().GetEnumerator();
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