using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CometCabsAdmin.Web.ApiModels
{
    public class RouteModel
    {
        public string Name { get; set; }
        public string Color { get; set; }
        public List<Path> Path { get; set; }
    }

    public class Path
    {
        public float Longitude { get; set; }
        public float Latitude { get; set; }
    }
}