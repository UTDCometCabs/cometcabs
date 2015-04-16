﻿using CometCabsAdmin.Model.Contracts;
using CometCabsAdmin.Model.Entities;
using CometCabsAdmin.Web.ApiModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Web.Http.Cors;

namespace CometCabsAdmin.Web.ApiControllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class CabActivityController : BaseController
    {
        private ICabService _cabService;
        private IUserService _userService;

        public CabActivityController(
            ICabService cabService
            , IUserService userService)
        {
            _cabService = cabService;
            _userService = userService;
        }

        public HttpResponseMessage Get()
        {
            List<CabActivity> activities = _cabService.GetCabActivity()
                .Where(t => DbFunctions.TruncateTime(t.LoginTime) == DbFunctions.TruncateTime(DateTime.Now))
                .ToList();

            List<CabActivityModel> model = new List<CabActivityModel>();
            string result = "[]";

            foreach (CabActivity activity in activities)
            {
                CabCoordinate current = activity.CabCoordinate
                        .OrderByDescending(m => m.CurrentDateTime)
                        .FirstOrDefault();

                if (current != null)
                {
                    CabActivityModel info = new CabActivityModel
                    {
                        CabCode = activity.Cab.CabCode,
                        Capacity = current.CurrentCapacity,
                        CurrentStatus = current.CurrentStatus,
                        RouteName = activity.Route.RouteName,
                        Latitude = current.Latitude,
                        Longitude = current.Longitude,
                    };

                    model.Add(info);
                }
            }

            HttpResponseMessage response = this.Request.CreateResponse(HttpStatusCode.OK);

            result = JsonConvert.SerializeObject(model);
            response.Content = new StringContent(result, Encoding.UTF8, "application/json");

            return response;
        }

        public HttpResponseMessage Post(string activityId, string currentCapacity, string currentStatus, string latitude, string longitude)
        {
            CabActivity activity = _cabService.GetCabActivity(long.Parse(activityId));
            string result = "[]";

            if (activity != null)
            {
                User user = _userService.GetUser(activity.DriverId);

                CabCoordinate coordinate = new CabCoordinate
                {
                    // CabActivity = activity,
                    CurrentDateTime = DateTime.Now,
                    ActivityId = activity.Id,
                    CurrentCapacity = int.Parse(currentCapacity),
                    CurrentStatus = currentStatus,
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
