using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GymApp.ViewModels
{
    [Authorize]
    public class TrainerBookingVM
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="Field required!")]
        [Display(Name ="Email address")]
        public string EmailAddress { get; set; }
        [Required(ErrorMessage ="Field required!")]
        [Display(Name ="Age")]
        public double Weight { get; set; }
        [Required(ErrorMessage ="Field required!")]
        [Display(Name ="Date and Time")]
        public double Height { get; set; }
        [Required(ErrorMessage ="Field required!")]
        public string Goal { get; set; }
        public string Alergy { get; set; }
        [Display(Name ="Consultation Type")]
        public string DietOptions { get; set; }
        [Display(Name ="Priest status")]
        public bool TrainerStatus { get; set; }
        // DietOptions

        public bool Fish { get; set; }
        public bool Vegetables { get; set; }
        public bool Beef { get; set; }
        public bool Vegan { get; set; }
        public bool Chicken { get; set; }

        // Methods
        public string SerializeDietOptions()
        {
            List<string> options = new List<string>();
            if (Fish)
            {
                options.Add("Fish");
            }
            if (Vegetables)
            {
                options.Add("Vegetables");
            }
            if (Beef)
            {
                options.Add("Beef");
            }
            if (Vegan)
            {
                options.Add("Vegan");
            }
            if (Chicken)
            {
                options.Add("Chicken");
            }
            if (options.Count == 0)
            {
                options.Add("N/A");
            }
            return JsonConvert.SerializeObject(options);
        }
        public void DeserializeDietOptions()
        {
            List<string> options = JsonConvert.DeserializeObject<List<string>>(DietOptions);

            foreach (var option in options)
            {
                switch (option)
                {
                    case "Fish":
                        Fish = true;
                        break;
                    case "Vegetables":
                        Vegetables = true;
                        break;
                    case "Beef":
                        Beef = true;
                        break;
                    case "Vegan":
                        Vegan = true;
                        break;
                    case "Chicken":
                        Chicken = true;
                        break;
                    default:
                        // Handle any unknown options here if needed
                        break;
                }
            }
        }

        // Constructor
        public TrainerBookingVM()
        {
            TrainerStatus = false;
        }
    }
}