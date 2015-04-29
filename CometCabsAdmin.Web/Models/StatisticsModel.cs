using CometCabsAdmin.Model.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CometCabsAdmin.Web.Models
{
    public class StatisticsModel
    {
        public StatisticsModel(ICabService cabService, IRouteService routeService)
        {
            Cabs = cabService.GetCabs().Select(s => s.CabCode).ToList();
            Routes = routeService.GetRoutes().Select(s => s.RouteName).ToList();
        }

        public List<string> Cabs { get; set; }
        public List<string> Routes { get; set; }
        public List<TotalRiderByCabModel> TotalRiderByCab { get; set; }
        public List<TotalRiderByRouteModel> TotalRiderByRoute { get; set; }
        public List<TotalInterestsModel> TotalInterests { get; set; }
    }

    public class TotalRiderByRouteModel
    {
        public string DayOfWeek { get; set; }
        public string RouteName { get; set; }
        public int Capacity { get; set; }
    }

    public class TotalRiderByCabModel
    {
        public int DayOfWeek { get; set; }
        public string CabCode { get; set; }
        public int Capacity { get; set; }
    }

    public class TotalInterestsModel
    {
        public int DayOfWeek { get; set; }
        public int Total { get; set; }
    }
}