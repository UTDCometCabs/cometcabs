using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using CometCabsAdmin.Model.Contracts;
using CometCabsAdmin.Model.Entities;
using CometCabsAdmin.Model.DataServices;
using CometCabsAdmin.Model.Common;
using Microsoft.Practices.Unity;
using System.Configuration;
using CometCabsAdmin.Dal;
using System.Web.Script.Services;
using System.ServiceModel.Activation;
using Newtonsoft.Json;

namespace CometCabsAdmin.WebServices
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
    public class CometCabsServices : ICometCabsServices
    {
        private IEncryption _encryption;
        private IUserService _userService;
        private IRouteService _routeService;

        public CometCabsServices(IEncryption encryption
            , IUserService userService
            , IRouteService routeService)
        {
            _userService = userService;
            _routeService = routeService;
            _encryption = encryption;
        }

        #region ICometCabsServices Members

        public bool Login(string userName, string password)
        {
            string pwd = _encryption.Encrypt(password);

            User result = _userService.GetUsers()
                .SingleOrDefault(s => (s.Username.Equals(userName)) && (s.Password.Equals(pwd)));

            return result != null;
        }

        public string GetRouteData()
        {
            List<RouteData> routeData = new List<RouteData>();

            List<Route> routes = _routeService.GetRoutes()
                // .Where(s=>s.IsActive)
                .ToList();

            foreach (Route route in routes)
            {
                RouteData data = new RouteData
                {
                    RouteName = route.RouteName,
                    RouteColor = route.RouteColor,
                    Coordinates = new List<Coordinate>(),
                };

                foreach (RouteCoordinates coordinate in route.RouteCoordinates)
                {
                    Coordinate latLng = new Coordinate
                    {
                        D = coordinate.Longitude.ToString(),
                        k = coordinate.Latitude.ToString(),
                    };

                    data.Coordinates.Add(latLng);
                }

                routeData.Add(data);
            }

            return JsonConvert.SerializeObject(routeData);
        }

        #endregion
    }
}