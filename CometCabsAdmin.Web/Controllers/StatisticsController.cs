using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using CometCabsAdmin.Model.Contracts;
using CometCabsAdmin.Web.Models;

namespace CometCabsAdmin.Web.Controllers
{
    public class StatisticsController : Controller
    {
        private ICabService _cabService;
        private IRouteService _routeService;
        private IInterestsService _interestsService;

        public StatisticsController(ICabService cabService
            , IRouteService routeService
            , IInterestsService interestsService)
        {
            _cabService = cabService;
            _routeService = routeService;
            _interestsService = interestsService;
        }

        // GET: Statistics
        public ActionResult Index()
        {
            return View();
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult ViewTotalRider(string week)
        {
            StatisticsModel model = new StatisticsModel(_cabService, _routeService);
            List<TotalRiderByCabModel> riderModel = GetRiderTotal(DateTime.Parse(week));

            model.TotalRiderByCab = riderModel;

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult ViewTotalInterests(string week)
        {
            StatisticsModel model = new StatisticsModel(_cabService, _routeService);
            List<TotalInterestsModel> interestsModel = GetInterestsTotal(DateTime.Parse(week));

            model.TotalInterests = interestsModel;

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        private List<TotalRiderByCabModel> GetRiderTotal(DateTime week)
        {
            var cal = System.Globalization.DateTimeFormatInfo.CurrentInfo.Calendar;
            int weekNumber = cal.GetWeekOfYear(week, CalendarWeekRule.FirstDay, DayOfWeek.Sunday);

            List<TotalRiderByCabModel> coordinates = _cabService.GetCabActivity()
                .Where(c => SqlFunctions.DatePart("week", c.LoginTime) == weekNumber)
                .OrderBy(c => c.CabId)
                .ThenBy(c => c.LoginTime)
                .GroupBy(c => new { DayOfWeek = SqlFunctions.DatePart("weekday", c.LoginTime).Value, CabCode = c.Cab.CabCode })
                .Select(c => new TotalRiderByCabModel
                {
                    CabCode = c.Key.CabCode,
                    DayOfWeek = c.Key.DayOfWeek - 1,
                    Capacity = c.Sum(d => d.TotalCapacity),
                })
                .ToList();

            return coordinates;
        }

        private List<TotalInterestsModel> GetInterestsTotal(DateTime week)
        {
            var cal = System.Globalization.DateTimeFormatInfo.CurrentInfo.Calendar;
            int weekNumber = cal.GetWeekOfYear(week, CalendarWeekRule.FirstDay, DayOfWeek.Sunday);

            List<TotalInterestsModel> interests = _interestsService.GetInterests()
                .Where(c => SqlFunctions.DatePart("week", c.FlagTime) == weekNumber)
                .OrderBy(c => c.FlagTime)
                .GroupBy(c => SqlFunctions.DatePart("weekday", c.FlagTime).Value)
                .Select(c => new TotalInterestsModel
                {
                    DayOfWeek = c.Key - 1,
                    Total = c.Count(),
                })
                .ToList();

            return interests;
        }
    }
}