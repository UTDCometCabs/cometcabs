using CometCabsAdmin.Model.Contracts;
using CometCabsAdmin.Model.DataServices;
using Microsoft.Practices.Unity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace CometCabsAdmin.Web
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.EnableCors();

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                  name: "DefaultApi",
                  routeTemplate: "api/{controller}/{id}",
                  defaults: new { id = RouteParameter.Optional }
             );

            // Ignore Null values on JSON Returns
            config.Formatters.JsonFormatter.SerializerSettings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
            config.Formatters.JsonFormatter.SerializerSettings = new JsonSerializerSettings { DefaultValueHandling = DefaultValueHandling.Ignore };
            config.EnsureInitialized();
        }
    }
}
