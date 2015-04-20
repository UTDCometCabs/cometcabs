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
    public class CancelInterestController : BaseController
    {
        private IInterestsService _interestsService;

        public CancelInterestController(
            IInterestsService interestsService)
        {
            _interestsService = interestsService;
        }

        // POST: api/Interests
        public HttpResponseMessage Post(string interestId)
        {
            Interests interest = _interestsService.GetInterest(long.Parse(interestId));

            string result = "[]";

            if (interest != null)
            {
                interest.FlagTime = interest.FlagTime.AddMinutes(-5);
                _interestsService.UpdateInterest(interest);

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
