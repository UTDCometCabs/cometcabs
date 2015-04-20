using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CometCabsAdmin.Web.ApiModels
{
    public class CabActivityModel
    {
        public string CabCode { get; set; }
        public long ActivityId { get; set; }
        public string ActivityName { get; set; }
        public string RouteName { get; set; }
        public int Capacity { get; set; }
        public int MaxCapacity { get; set; }
        public string CurrentStatus { get; set; }
        public float Longitude { get; set; }
        public float Latitude { get; set; }
    }
}