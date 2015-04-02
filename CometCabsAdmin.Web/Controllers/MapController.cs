using CometCabsAdmin.Model.Contracts;
using CometCabsAdmin.Model.Entities;
using CometCabsAdmin.Web.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace CometCabsAdmin.Web.Controllers
{
    [CometCabsAuthorize(Roles = "Admin")]
    public class MapController : BaseController
    {
        private IRouteService _routeService;

        public MapController(
            IRouteService routeService
            )
        {
            _routeService = routeService;
        }

        // GET: Map
        public ActionResult Index()
        {
            List<Route> routes = _routeService.GetRoutes().ToList();
            List<List<RouteCoordinate>> coordinates = new List<List<RouteCoordinate>>();

            foreach (Route route in routes)
            {
                List<RouteCoordinate> coordinate = new List<RouteCoordinate>();

                foreach (RouteCoordinates routeCoordinate in route.RouteCoordinates)
                {
                    coordinate.Add(new RouteCoordinate
                    {
                        RouteId = routeCoordinate.RouteId,
                        D = routeCoordinate.Longitude.ToString(),
                        k = routeCoordinate.Latitude.ToString(),
                    });
                }

                coordinates.Add(coordinate);
            }

            MapViewModel model = new MapViewModel
            {
                RouteTable = routes,
                RouteCoordinates = coordinates,
            };

            return View(model);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult GetForm(Nullable<long> id = null)
        {
            MapViewModel model = new MapViewModel();

            if (id.HasValue)
            {
                Route route = _routeService.GetRoute(id.Value);

                if (route != null)
                {
                    model.RouteName = route.RouteName;
                    model.RouteDesc = route.RouteDesc;
                    model.IsActive = route.IsActive;
                    model.RouteColor = route.RouteColor;

                    string coordinateString = string.Empty;

                    foreach (RouteCoordinates coordinate in route.RouteCoordinates)
                    {
                        coordinateString = string.Concat(coordinateString, string.Format("{0},{1}|", coordinate.Latitude.ToString(), coordinate.Longitude.ToString()));
                    }

                    // model.RouteCoordinates = coordinateString.Substring(0, coordinateString.Length - 1);
                }
            }

            return PartialView("_routeForm", model);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SaveRoute(string routeId, string routeName, string routeDesc, string routeColor, string routes)
        {
            Route route = _routeService.GetRoute(long.Parse(routeId));
            SelectListItem color = JsonConvert.DeserializeObject<SelectListItem>(routeColor);
            List<RouteCoordinate> coordinates = JsonConvert.DeserializeObject<List<RouteCoordinate>>(routes);
            List<RouteCoordinates> routeCoordinates = new List<RouteCoordinates>();

            foreach (RouteCoordinate routeCoordinate in coordinates)
            {
                routeCoordinates.Add(new RouteCoordinates
                {
                    Latitude = float.Parse(routeCoordinate.k),
                    Longitude = float.Parse(routeCoordinate.D),
                    CreatedBy = HttpContext.User.Identity.Name,
                    CreateDate = DateTime.Now,
                    IPAddress = IPAddress,
                });
            }

            if (route == null)
            {
                route = new Route
                {
                    RouteName = routeName,
                    RouteDesc = routeDesc,
                    RouteColor = color.Value,
                    RouteCoordinates = routeCoordinates,
                    CreatedBy = HttpContext.User.Identity.Name,
                    CreateDate = DateTime.Now,
                    IPAddress = IPAddress,
                };

                _routeService.InsertRoute(route);
            }
            else
            {
                _routeService.DeleteRouteCoordinate(route);
                route.RouteCoordinates = routeCoordinates;

                _routeService.UpdateRoute(route);
            }

            return View("Index", new MapViewModel());
        }
    }
}