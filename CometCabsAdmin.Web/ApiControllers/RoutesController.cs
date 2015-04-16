using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using CometCabsAdmin.Model.Contracts;
using CometCabsAdmin.Model.Entities;
using Newtonsoft.Json;
using System.Web.Http.Cors;
using System.Net.Http;
using System.Text;
using System.Net;
using CometCabsAdmin.Web.ApiModels;

namespace CometCabsAdmin.Web.ApiControllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class RoutesController : BaseController
    {
        private IRouteService _routeService;

        public RoutesController(IRouteService routeService)
        {
            _routeService = routeService;
        }

        // GET api/<controller>
        public HttpResponseMessage Get()
        {
            List<Route> routes = _routeService.GetRoutes().ToList();
            List<RouteModel> model = new List<RouteModel>();

            foreach (Route route in routes)
            {
                RouteModel routeModel = new RouteModel
                {
                    Name = route.RouteName,
                    Color = route.RouteColor,
                };

                List<Path> coordinate = new List<Path>();

                foreach (RouteCoordinates routeCoordinate in route.RouteCoordinates)
                {
                    coordinate.Add(new Path
                    {
                        Longitude = routeCoordinate.Longitude,
                        Latitude = routeCoordinate.Latitude,
                    });
                }

                routeModel.Path = coordinate;

                model.Add(routeModel);
            }

            HttpResponseMessage response = this.Request.CreateResponse(HttpStatusCode.OK);

            response.Content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

            return response;
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        public HttpResponseMessage Post()
        {
            HttpResponseMessage response = this.Request.CreateResponse(HttpStatusCode.OK);

            response.Content = new StringContent(JsonConvert.SerializeObject("[]"), Encoding.UTF8, "application/json");

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