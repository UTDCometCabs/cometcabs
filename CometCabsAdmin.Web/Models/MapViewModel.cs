using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Drawing;
using System.ComponentModel.DataAnnotations;
using CometCabsAdmin.Model.Entities;

namespace CometCabsAdmin.Web.Models
{
    public class MapViewModel
    {
        public long RouteId { get; set; }

        [Display(Name = "Route Name")]
        [Required(ErrorMessage = "Please enter route name")]
        public string RouteName { get; set; }

        [Display(Name = "Description")]
        public string RouteDesc { get; set; }

        [Display(Name = "Active Route")]
        public bool IsActive { get; set; }

        [Display(Name = "Choose color")]
        public string RouteColor { get; set; }

        public List<List<RouteCoordinate>> RouteCoordinates { get; set; }

        public List<Route> RouteTable { get; set; }
        
    }

    [Serializable]
    public class RouteCoordinate
    {
        public long RouteId { get; set; }
        public string k { get; set; }
        public string D { get; set; }
    }
}