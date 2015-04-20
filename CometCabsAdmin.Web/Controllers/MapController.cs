using CometCabsAdmin.Model.Contracts;
using CometCabsAdmin.Model.Entities;
using CometCabsAdmin.Web.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
        public ActionResult GetRoute(string latitude, string longitude)
        {
            JsonResult result = new JsonResult();

            if ((!string.IsNullOrWhiteSpace(latitude)) && (!string.IsNullOrWhiteSpace(longitude)))
            {
                RouteCoordinates coordinate = _routeService.GetRouteCoordinate(float.Parse(latitude), float.Parse(longitude));

                if (coordinate != null)
                {
                    Route route = _routeService.GetRoute(coordinate.RouteId);

                    if (route != null)
                    {
                        result = Json(route);
                    }

                    // result=JsonConvert.DeserializeObject("origin:{D:{0}, k:{1}}, destination:{D:{0}, k:{1}}, waypoints:{
                    // {location:{D:{0}, k:{1}}, stopover: false}, {location:{D:{0}, k:{1}}, stopover: false}}}
                }
            }

            return result;
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
        public ActionResult SaveRoute(string routeId, string routeName, string routeDesc, string routeColor, bool isActive, string routes, string directions)
        {
            Route route = _routeService.GetRoute(long.Parse(routeId));
            SelectListItem color = JsonConvert.DeserializeObject<SelectListItem>(routeColor);
            List<RouteCoordinate> coordinates = JsonConvert.DeserializeObject<List<RouteCoordinate>>(routes);
            List<RouteCoordinates> routeCoordinates = new List<RouteCoordinates>();
            List<RouteDirections> routeDirections = new List<RouteDirections>();
            dynamic dirs = JsonConvert.DeserializeObject(directions);

            foreach (var dir in dirs)
            {
                string name = dir.TagName;
                float latitude = dir.Latitude;
                float longitude = dir.Longitude;
                RouteDirections direction = new RouteDirections
                {
                    TagName = name,
                    Latitude = latitude,
                    Longitude = longitude,
                    CreatedBy = HttpContext.User.Identity.Name,
                    CreateDate = DateTime.Now,
                    IPAddress = IPAddress,
                };

                routeDirections.Add(direction);
            }

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
                    IsActive = isActive,
                    RouteCoordinates = routeCoordinates,
                    RouteDirections = routeDirections,
                    CreatedBy = HttpContext.User.Identity.Name,
                    CreateDate = DateTime.Now,
                    IPAddress = IPAddress,
                };

                _routeService.InsertRoute(route);
            }
            else
            {
                _routeService.DeleteRouteCoordinate(route.Id);
                route.RouteCoordinates = routeCoordinates;
                _routeService.UpdateRoute(route);
            }

            return View("Index", new MapViewModel());
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DeleteRoute(string id)
        {
            Route route = _routeService.GetRoute(long.Parse(id));

            if (route != null)
            {
                _routeService.DeleteRoute(route);
            }

            return View("Index", new MapViewModel());
        }
    }
}