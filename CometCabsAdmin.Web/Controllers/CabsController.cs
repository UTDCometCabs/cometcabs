using CometCabsAdmin.Model.Contracts;
using CometCabsAdmin.Model.Entities;
using CometCabsAdmin.Web.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CometCabsAdmin.Web.Controllers
{
    [CometCabsAuthorize(Roles = "Admin")]
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
                    Id = cab.Id,
                    CabCode = cab.CabCode,
                    CabDesc = cab.CabDesc,
                    MaxCapacity = cab.MaxCapacity.ToString(),
                    Status = cab.OnDutyStatus,
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
            SelectListItem status = JsonConvert.DeserializeObject<SelectListItem>(onDutyStatus);

            if (ModelState.IsValid)
            {
                try
                {
                    long cabId = 0;

                    if (!string.IsNullOrWhiteSpace(id) && long.TryParse(id, out cabId))
                    {
                        Cab cab = _cabService.GetCab(cabId);

                        if (cab == null)
                        {
                            cab = new Cab
                            {
                                CabCode = cabCode,
                                CabDesc = cabDesc,
                                MaxCapacity = int.Parse(maxCapacity),
                                OnDutyStatus = status.Value,
                                CreatedBy = HttpContext.User.Identity.Name,
                                CreateDate = DateTime.Now,
                                IPAddress = IPAddress,
                            };

                            _cabService.InsertCab(cab);
                        }
                        else
                        {
                            cab.CabCode = cabCode;
                            cab.CabDesc = cabDesc;
                            cab.MaxCapacity = int.Parse(maxCapacity);
                            cab.OnDutyStatus = status.Value;
                            cab.UpdateDate = DateTime.Now;
                            cab.UpdatedBy = HttpContext.User.Identity.Name;
                            cab.IPAddress = IPAddress;

                            _cabService.UpdateCab(cab);

                            // update cab activity status
                            CabCoordinate cabCoordinate = _cabService.GetCabCoordinates()
                                .Where(c => c.CabActivity.CabId.Equals(cabId))
                                .OrderByDescending(c => c.Id)
                                .FirstOrDefault();

                            if (cabCoordinate != null)
                            {
                                cabCoordinate.CurrentStatus = status.Value;
                                cabCoordinate.UpdateDate = DateTime.Now;
                                cabCoordinate.UpdatedBy = HttpContext.User.Identity.Name;
                                cabCoordinate.IPAddress = IPAddress;

                                _cabService.UpdateCabCoordinate(cabCoordinate);
                            }
                        }
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