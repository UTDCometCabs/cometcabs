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
    public class LogoffController : BaseController
    {
        private ICabService _cabService;
        private IUserService _userService;

        public LogoffController(
            ICabService cabService
            , IUserService userService)
        {
            _cabService = cabService;
            _userService = userService;
        }

        // POST: api/Interests
        public HttpResponseMessage Post(string activityId, string latitude, string longitude)
        {
            CabActivity activity = _cabService.GetCabActivity(long.Parse(activityId));

            string result = "[]";

            if (activity != null)
            {
                User user = _userService.GetUser(activity.DriverId);
                Cab cab = _cabService.GetCab(activity.CabId);

                if (cab != null)
                {
                    cab.OnDutyStatus = "off-duty";
                    _cabService.UpdateCab(cab);
                }

                CabCoordinate coordinate = new CabCoordinate
                {
                    // CabActivity = activity,
                    CurrentDateTime = DateTime.Now,
                    ActivityId = activity.Id,
                    CurrentCapacity = activity.CabCoordinate.OrderByDescending(c=>c.CurrentDateTime).FirstOrDefault().CurrentCapacity,
                    CurrentStatus = activity.Cab.OnDutyStatus,
                    Latitude = float.Parse(latitude),
                    Longitude = float.Parse(longitude),
                    CreatedBy = user.Username,
                    CreateDate = DateTime.Now,
                    IPAddress = IPAddress,
                };

                _cabService.InsertCabCoordinate(coordinate);

                dynamic info = new
                {
                    success = true,
                };

                result = JsonConvert.SerializeObject(info);
            }

            HttpResponseMessage response = this.Request.CreateResponse(HttpStatusCode.OK);

            response.Content = new StringContent(result, Encoding.UTF8, "application/json");

            return response;
        }
    }
}
