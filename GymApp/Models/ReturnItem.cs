using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GymApp.Models
{
    public class ReturnItem
    {
        
        public int ReturnId { get; set; }
        public int Id { get; set; }
        public Order Order { get; set; }
        public string ReturnReason { get; set; }
        public string RequestedBy { get; set; }
        public bool IsApproved { get; set; }

    }
}
