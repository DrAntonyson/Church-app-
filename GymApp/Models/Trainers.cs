using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GymApp.Models
{
    public class Trainers
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Gender { get; set; }
        public string EmailAddress { get; set; }
        public string Password { get; set; }
        public bool IsBooked { get; set; }
        public int TrainerBookingId { get; set; }
        // Add more properties here
        public List<TrainerBookings> TrainerBooking { get; set; }

    }
}