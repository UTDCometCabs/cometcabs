using CometCabsAdmin.Model.Contracts;
using CometCabsAdmin.Model.Entities;
using CometCabsAdmin.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CometCabsAdmin.Web.Controllers
{
    public class CabsController : BaseController
    {
        private ICabService _cabService;

        public CabsController(ICabService cabService)
        {
            _cabService = cabService;
        }

        public ActionResult Index()
        {
            List<Cab> cabs = _cabService.GetCabs().ToList();
            List<CabTable> table = new List<CabTable>();

            foreach (Cab cab in cabs)
            {
                table.Add(new CabTable
                {
                    CabCode = cab.CabCode,
                    MaxCapacity = cab.MaxCapacity.ToString(),
                    Status = string.Format("{0}-duty", cab.OnDutyStatus ? "On" : "Off"),
                });
            }

            CabViewModel model = new CabViewModel
            {
                CabTable = table,
            };

            return View(model);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SaveCab(string id, string cabCode, string cabDesc, string maxCapacity, string onDutyStatus)
        {
            string message = string.Empty;

            if (ModelState.IsValid)
            {
                try
                {
                    Cab cab = new Cab
                    {
                        CabCode = cabCode,
                        CabDesc = cabDesc,
                        MaxCapacity = int.Parse(maxCapacity),
                        OnDutyStatus = bool.Parse(onDutyStatus),
                        CreatedBy = HttpContext.User.Identity.Name,
                        CreateDate = DateTime.Now,
                        IPAddress = IPAddress,

                    };

                    if (!string.IsNullOrWhiteSpace(id))
                    {
                        cab.Id = int.Parse(id);
                        cab.UpdateDate = DateTime.Now;
                        cab.UpdatedBy = HttpContext.User.Identity.Name;

                        _cabService.UpdateCab(cab);
                    }
                    else
                    {
                        _cabService.InsertCab(cab);

                    }
                }
                catch (Exception ex)
                {
                    message = string.Format("Error: {0}, stack trace: {1}", ex.Message, ex.StackTrace);
                }
            }

            return Content(message);
        }
    }
}