using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Reflection.Emit;
using System.Security.Claims;
using System.Threading.Tasks;
using GymApp.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace GymApp.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string Subscription { get; set; }
        public float Account { get; set; }
        public string Name { get; set; }
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public DbSet<Products> Products { get; set; }
        public DbSet<Driver> Drivers { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<InvoiceModel> invoiceModel { get; set; }
        public DbSet<Order> orders { get; set; }
        public DbSet<ContactModel> contactModels { get; set; }
        public DbSet<BlogModel> blogModels { get; set; }
        public DbSet<TrainerBookings> TrainerBookings { get; set; }
        public DbSet<Trainers> Trainers { get; set; }
        public DbSet<Events> Events { get; set; }
        public DbSet<ReturnItem> ReturnItems { get; set; }
        public DbSet<Returns>Returns { get; set; }  

        public System.Data.Entity.DbSet<GymApp.Models.BookClass> BookClasses { get; set; }

        public System.Data.Entity.DbSet<GymApp.Models.classtype> classtypes { get; set; }

        public System.Data.Entity.DbSet<GymApp.Models.Trainer> Trainer { get; set; }



        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<TrainerBookings>()
        //        .HasRequired(tb => tb.Trainer)
        //        .WithOptional(t => t.TrainerBooking)
        //        .Map(m => m.MapKey("TrainerId")); // Map foreign key

        //    // Rest of your model configuration
        //}

    }
}