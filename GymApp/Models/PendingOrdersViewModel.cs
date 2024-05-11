using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GymApp.Models
{
    public class PendingOrdersViewModel
    {
        public IEnumerable<Order> Orders { get; set; }
        public double? TraveledDistance { get; set; }
    }
}