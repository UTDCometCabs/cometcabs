using CometCabsAdmin.Model.Contracts;
using CometCabsAdmin.Model.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Web.Http.Cors;

namespace CometCabsAdmin.Web.ApiControllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class CabCapacityController : BaseController
    {
        private ICabService _cabService;

        public CabCapacityController(ICabService cabService)
        {
            _cabService = cabService;
        }

        // POST api/<controller>
        public HttpResponseMessage Post(string activityId, string capacity)
        {
            string result = "[]";

            long id = 0;
            int numberOfRider = 0;

            if (long.TryParse(activityId, out id))
            {
                CabActivity cabActivity = _cabService.GetCabActivity(id);

                if (cabActivity != null)
                {
                    if (int.TryParse(capacity, out numberOfRider))
                    {
                        cabActivity.TotalCapacity += numberOfRider;
                        cabActivity.UpdateDate = DateTime.Now;
                        cabActivity.UpdatedBy = cabActivity.Driver.Username;

                        _cabService.UpdateCabActivity(cabActivity);

                        dynamic info = new
                        {
                            success = true,
                        };

                        result = JsonConvert.SerializeObject(info);
                    }
                }
            }

            HttpResponseMessage response = this.Request.CreateResponse(HttpStatusCode.OK);

            response.Content = new StringContent(result, Encoding.UTF8, "application/json");

            return response;
        }
    }
}
