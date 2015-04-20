using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CometCabsAdmin.Web.ApiModels
{
    public class InterestsModel
    {
        public long InterestId { get; set; }
        public DateTime FlagTime { get; set; }
        public float  Longitude { get; set; }
        public float  Latitude { get; set; }
    }
}