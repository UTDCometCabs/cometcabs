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
            List<Route> routes = _routeService.GetRoutes().ToList();
            List<Cab> cabs = _cabService.GetCabs().ToList();
            string result = "[]";

            dynamic info = new
            {
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
        public HttpResponseMessage Post(string userName, string password, string cabId, string routeId)
        {
            string passwordEncrypted = _encryption.Encrypt(password);
            string result = "[]";

            CometCabsAdmin.Model.Entities.User user = _userService.GetUsers()
                .Where(u => u.Username == userName && u.Password == passwordEncrypted).FirstOrDefault();

            if (user != null)
            {
                long activityId = _cabService.InsertCabActivity(new CabActivity
                 {
                     LoginTime = DateTime.Now,
                     DriverId = user.Id,
                     CabId = long.Parse(cabId),
                     RouteId = long.Parse(routeId),
                     CreatedBy = user.Username,
                     CreateDate = DateTime.Now,
                     IPAddress = IPAddress,

                 });

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