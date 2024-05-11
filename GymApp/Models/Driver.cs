using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GymApp.Models
{
    public class Driver
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public int AssignedOrderId { get; set; }
        public string UserId { get; set; }
        public bool IsAvailable { get; set; }

        public Driver()
        {
            this.IsAvailable = true;
        }
    }
}