using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace GymApp.Models
{
    public class Trainer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TrainerId { get; set; }
        [Display(Name = "Trainer Registration Id")]
        public string TrainerRegistrationId { get; set; }
        [Required]
        public string Name { get; set; }
        public string UserId { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }
        public bool IsAssigned { get; set; }
        public bool IsAvailable { get; set; }

        public Trainer()
        {
            this.IsAssigned = false;
            this.IsAvailable = true;
            var guid = Guid.NewGuid().ToString();
            this.TrainerRegistrationId = guid.Substring(0, 6);

        }
    }
}