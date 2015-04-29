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
            MapViewModel model = new MapViewModel();
            JsonResult result = new JsonResult();

            if ((!string.IsNullOrWhiteSpace(latitude)) && (!string.IsNullOrWhiteSpace(longitude)))
            {
                RouteCoordinates coordinate = _routeService.GetRouteCoordinate(float.Parse(latitude), float.Parse(longitude));

                if (coordinate != null)
                {
                    Route route = _routeService.GetRoute(coordinate.RouteId);
                    List<RouteDirections> directions = _routeService.GetRouteDirections(coordinate.RouteId).ToList();

                    if (route != null)
                    {
                        model.RouteId = route.Id;
                        model.RouteName = route.RouteName;
                        model.RouteDesc = route.RouteDesc;
                        model.IsActive = route.IsActive;
                        model.RouteColor = new SelectListItem { Value = route.RouteColor };
                        model.RouteDirections = new List<RouteDirection>();

                        foreach (RouteDirections direction in directions)
                        {
                            RouteDirection routeDirection = new RouteDirection
                            {
                                RouteId = direction.RouteId,
                                Latitude = direction.Latitude,
                                Longitude = direction.Longitude,
                                TagName = direction.TagName,
                            };

                            model.RouteDirections.Add(routeDirection);
                        }

                        result = Json(JsonConvert.SerializeObject(model));
                    }
                }
            }

            return result;
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SaveRoute(string routeId, string routeName, string routeDesc, string routeColor, bool isActive, string routes, string directions)
        {
            long id = long.Parse(routeId);
            
            SelectListItem color = JsonConvert.DeserializeObject<SelectListItem>(routeColor);
            List<RouteCoordinate> coordinates = JsonConvert.DeserializeObject<List<RouteCoordinate>>(routes);

            List<RouteCoordinates> routeCoordinates = _routeService.GetRouteCoordinates(id).ToList();
            List<RouteDirections> routeDirections = _routeService.GetRouteDirections(id).ToList();

            foreach (RouteCoordinates coordinate in routeCoordinates)
            {
                _routeService.DeleteRouteCoordinate(coordinate);
            }

            routeCoordinates.Clear();

            foreach (RouteDirections direction in routeDirections)
            {
                _routeService.DeleteRouteDirections(direction);
            }

            routeDirections.Clear();

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

            Route route = _routeService.GetRoute(id);

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
                route.RouteName = routeName;
                route.RouteDesc = routeDesc;
                route.RouteColor = color.Value;
                route.IsActive = isActive;
                route.RouteCoordinates = routeCoordinates;
                route.RouteDirections = routeDirections;
                route.UpdatedBy = HttpContext.User.Identity.Name;
                route.UpdateDate = DateTime.Now;

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