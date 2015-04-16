using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CometCabsAdmin.Web.Models
{
    public class CabViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Cab Code")]
        [Required(ErrorMessage = "Please enter a code for the cab")]
        public string CabCode { get; set; }

        [Display(Name = "Cab Description")]
        [Required(ErrorMessage = "Please enter cab description")]
        public string CabDesc { get; set; }

        [Display(Name = "Maximum Capacity")]
        public int MaxCapacity { get; set; }

        [Display(Name = "On-duty")]
        public bool OnDutyStatus { get; set; }

        public List<CabTable> CabTable { get; set; }
    }

    [Serializable]
    public class CabTable
    {
        public string CabCode { get; set; }
        public string MaxCapacity { get; set; }
        public string Status { get; set; }
    }
}