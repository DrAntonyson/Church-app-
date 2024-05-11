using GymApp.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GymApp.ViewModels
{
    public class AssignTrainerVM
    {
        [Required]
        public int TrainerId { get; set; }
        [Required]
        public int TrainerBookingId { get; set; }
    }
}