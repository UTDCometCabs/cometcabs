using CometCabsAdmin.Model.Contracts;
using CometCabsAdmin.Model.Entities;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;

namespace CometCabsAdmin.Web.ApiControllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class LoginController : BaseController
    {
        private IUserService _userService;
        private IRouteService _routeService;
        private ICabService _cabService;
        private IEncryption _encryption;

        public LoginController(IUserService userService
            , IRouteService routeService
            , ICabService cabService
            , IEncryption encryption)
        {
            _userService = userService;
            _routeService = routeService;
            _cabService = cabService;
            _encryption = encryption;
        }

        // GET api/<controller>
        public HttpResponseMessage Get()
        {
            List<User> users = _userService.GetUsers()
                .Where(s => s.UserRole.RoleName.ToLower().Equals("driver"))
                .ToList();

            List<CabActivity> cabActivity = _cabService.GetCabActivity()
                .Where(s => s.Cab.OnDutyStatus.ToLower().Equals("on-duty"))
                .OrderByDescending(s => s.Id)
                .ToList();

            int indexUsers = 0;
            int indexActivity = 0;

            while (indexUsers < users.Count())
            {
                bool notFound = true;

                while ((indexActivity < cabActivity.Count()) && (notFound))
                {
                    notFound = users[indexUsers].Id != cabActivity[indexActivity].DriverId;

                    if (!notFound)
                    {
                        users.RemoveAt(indexUsers);
                        indexUsers = 0;
                    }

                    indexActivity++;
                }

                indexActivity = 0;
                indexUsers++;
            }

            List<Route> routes = _routeService.GetRoutes().ToList();
            List<Cab> cabs = _cabService.GetCabs()
                .Where(s => s.OnDutyStatus.ToLower().Equals("off-duty"))
                .ToList();

            string result = "[]";

            dynamic info = new
            {
                Users = users.Select(t => new
                {
                    UserId = t.Id,
                    UserName = t.Username,
                }),
                Cabs = cabs.Select(t => new
                {
                    CabId = t.Id,
                    CabCode = t.CabCode,
                }),
                Routes = routes.Select(t => new
                {
                    RouteId = t.Id,
                    RouteName = t.RouteName,
                })

            };

            result = JsonConvert.SerializeObject(info);

            HttpResponseMessage response = this.Request.CreateResponse(HttpStatusCode.OK);

            response.Content = new StringContent(result, Encoding.UTF8, "application/json");

            return response;
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        public HttpResponseMessage Post(string userId, string password, string cabId, string routeId, string longitude, string latitude)
        {
            string passwordEncrypted = _encryption.Encrypt(password);
            string result = "[]";
            long lgUserId = long.Parse(userId);

            CometCabsAdmin.Model.Entities.User user = _userService.GetUsers()
                .Where(u => u.Id == lgUserId && u.Password == passwordEncrypted && u.UserRole.RoleName.ToLower() == "driver").FirstOrDefault();

            if (user != null)
            {
                long lgCabId = long.Parse(cabId);
                Cab cab = _cabService.GetCab(lgCabId);

                if (cab != null)
                {
                    cab.OnDutyStatus = "on-duty";
                    _cabService.UpdateCab(cab);
                }

                List<CabCoordinate> cabCoordinate = new List<CabCoordinate>();
                CabActivity cabActivity = new CabActivity
                {
                    LoginTime = DateTime.Now,
                    DriverId = user.Id,
                    CabId = lgCabId,
                    RouteId = long.Parse(routeId),
                    CreatedBy = user.Username,
                    CreateDate = DateTime.Now,
                    IPAddress = IPAddress,
                };

                cabCoordinate.Add(new CabCoordinate
                {
                    Latitude = float.Parse(latitude),
                    Longitude = float.Parse(longitude),
                    CurrentStatus = cab.OnDutyStatus,
                    CurrentDateTime = DateTime.Now,
                    CurrentCapacity = 0,
                    CreatedBy = user.Username,
                    CreateDate = DateTime.Now,
                    IPAddress = IPAddress,
                });

                cabActivity.CabCoordinate = cabCoordinate;

                long activityId = _cabService.InsertCabActivity(cabActivity);

                dynamic info = new
                {
                    UserName = user.Username,
                    RoleName = user.UserRole.RoleName,
                    ActivityId = activityId.ToString(),
                };

                result = JsonConvert.SerializeObject(info);
            }

            HttpResponseMessage response = this.Request.CreateResponse(HttpStatusCode.OK);

            response.Content = new StringContent(result, Encoding.UTF8, "application/json");

            return response;
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}