using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace GymApp.Models
{
    public class BookClass
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        [Display(Name = "typeclass")]
        public classtype classtype { get; set; }
        
        public int classtypeid { get; set; }

        [Display(Name = "Date and Time")]
        public string datepicker { get; set; }

        [Display(Name = "trainerid")]
        public Trainer trainer { get; set; }


        public int? TrainerId { get; set; }

        public string UserBooked { get; set; }
    }
}