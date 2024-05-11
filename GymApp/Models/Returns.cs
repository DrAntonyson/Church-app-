using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GymApp.Models
{
    public class Returns
    {

        [DatabaseGenerated(DatabaseGeneratedOption.Identity), Key]
        public int ReturnId { get; set; }
        public int Id { get; set; }
        public Order Order { get; set; }
        public string ReturnReason { get; set; }
        public string RequestedBy { get; set; }
        public bool IsApproved { get; set; }
        public string imageUrl { get; set; }
    }
}