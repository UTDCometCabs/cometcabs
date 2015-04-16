using CometCabsAdmin.Model.Contracts;
using CometCabsAdmin.Model.Entities;
using CometCabsAdmin.Web.ApiModels;
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
    public class InterestsController : BaseController
    {
        private IInterestsService _interestsService;

        public InterestsController(IInterestsService interestsService)
        {
            _interestsService = interestsService;
        }

        // GET: api/Interests
        public HttpResponseMessage Get()
        {
            HttpResponseMessage response = this.Request.CreateResponse(HttpStatusCode.OK);

            DateTime date = DateTime.Now.AddMinutes(-5);

            List<Interests> interests = _interestsService.GetInterests()
                .Where(t => DateTime.Compare(t.FlagTime, date) > 0)
                .ToList();

            List<InterestsModel> models = new List<InterestsModel>();

            foreach (Interests interest in interests)
            {
                InterestsModel model = new InterestsModel
                {
                    FlagTime = interest.FlagTime,
                    Longitude = interest.Longitude,
                    Latitude = interest.Latitude,
                };

                models.Add(model);
            }

            string result = JsonConvert.SerializeObject(models);
            response.Content = new StringContent(result, Encoding.UTF8, "application/json");

            return response;
        }

        // GET: api/Interests/5
        public HttpResponseMessage Get(int id)
        {
            HttpResponseMessage response = this.Request.CreateResponse(HttpStatusCode.OK);

            string result = JsonConvert.SerializeObject("[{ result: unimplemented }]");
            response.Content = new StringContent(result, Encoding.UTF8, "application/json");

            return response;
        }

        // POST: api/Interests
        public HttpResponseMessage Post(string flagTime, string longitude, string latitude)
        {
            HttpResponseMessage response = this.Request.CreateResponse(HttpStatusCode.OK);

            Interests interest = new Interests
            {
                FlagTime = DateTime.Parse(flagTime),
                Longitude = float.Parse(longitude),
                Latitude = float.Parse(latitude),
                CreatedBy = "rider's app",
                CreateDate = DateTime.Now,
                IPAddress = IPAddress,
            };

            long id = _interestsService.InsertInterest(interest);

            string result = JsonConvert.SerializeObject(string.Format("{{ interestId: {0} }}", id));
            response.Content = new StringContent(result, Encoding.UTF8, "application/json");

            return response;
        }

        // PUT: api/Interests/5
        public HttpResponseMessage Put(int id, [FromBody]string value)
        {
            HttpResponseMessage response = this.Request.CreateResponse(HttpStatusCode.OK);

            string result = JsonConvert.SerializeObject("[{ result: unimplemented }]");
            response.Content = new StringContent(result, Encoding.UTF8, "application/json");

            return response;
        }

        // DELETE: api/Interests/5
        public HttpResponseMessage Delete(int id)
        {
            HttpResponseMessage response = this.Request.CreateResponse(HttpStatusCode.OK);

            string result = JsonConvert.SerializeObject("[{ result: unimplemented }]");
            response.Content = new StringContent(result, Encoding.UTF8, "application/json");

            return response;
        }
    }
}
