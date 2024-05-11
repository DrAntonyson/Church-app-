using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GymApp.Models
{
    public class TrainerBookings
    {
        [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string EmailAddress { get; set; }
        public double Weight { get; set; }
        public double Height { get; set; }
        public string Goal { get; set; }
        public string Alergy { get; set; }
        public string DietOptions { get; set; }
        public bool TrainerStatus { get; set; }
        public int TrainerId { get; set; }
        public List<Trainers> Trainer { get; set; }
    }
}