using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace GymApp.Models
{
    public class Vehicle
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Display(Name = "Vehicle Name")]
        public string VehicleName { get; set; }
        [Display(Name = "Vehicle Color")]
        public string VehicleColor { get; set; }
        [Display(Name = "Number Plate")]
        public string NumberPlate { get; set; }
        [Display(Name = "Travel Distance")]
        public double TraveledDistance { get; set; }
        public string Image { get; set; }
        [Display(Name = "Status")]
        public string Status { get; set; }
        public int StatusNum { get; set; }
    }
}