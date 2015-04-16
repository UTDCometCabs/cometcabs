using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CometCabsAdmin.Web.ApiControllers
{
    public class BaseController : ApiController
    {
        public string IPAddress
        {
            get
            {
                return ((System.Web.HttpContextWrapper)Request.Properties["MS_HttpContext"]).Request.UserHostAddress;
            }
        }
    }
}
