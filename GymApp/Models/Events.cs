using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GymApp.Models
{
    public class Events
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EventId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime dateTime { get; set; }

        public string Location { get; set; }

        public string Organizer { get; set; }

    }
}