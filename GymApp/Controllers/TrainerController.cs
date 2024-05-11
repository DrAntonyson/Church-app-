using GymApp.Models;
using GymApp.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace GymApp.Controllers
{
    public class TrainerController : Controller
    {
        readonly ApplicationDbContext _db = new ApplicationDbContext();
        // GET: Trainer
        public ActionResult BookTrainer()
        {
            var user = _db.TrainerBookings.FirstOrDefault(tb => tb.EmailAddress == User.Identity.Name);
            if (user != null)
            {
                return RedirectToAction("Booking", new { user.Id });
            }
            return View(new TrainerBookingVM());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult BookTrainer(TrainerBookingVM model)
        {
            if (ModelState.IsValid)
            {
                TrainerBookings trainerBooking = new TrainerBookings()
                {
                    EmailAddress = model.EmailAddress,
                    Weight = model.Weight,
                    Height = model.Height,
                    Goal = model.Goal,
                    Alergy = model.Alergy,
                    DietOptions = model.SerializeDietOptions()
                };
                _db.TrainerBookings.Add(trainerBooking);
                _db.SaveChanges();
                return RedirectToAction("Booking", new { trainerBooking.Id });
            }
            return View(model);
        }

        public ActionResult Booking(int Id)
        {
            TrainerBookings booking = _db.TrainerBookings.Find(Id);
            if (booking != null)
            {
                return View(booking);
            }
            return HttpNotFound("Booking not found!");
        }

        public ActionResult Edit(int Id)
        {
            TrainerBookings trainerBooking = _db.TrainerBookings.Find(Id);
            if (trainerBooking != null)
            {
                TrainerBookingVM trainerBookingVM = new TrainerBookingVM()
                {
                    Id = trainerBooking.Id,
                    EmailAddress = trainerBooking.EmailAddress,
                    Weight = trainerBooking.Weight,
                    Height = trainerBooking.Height,
                    Goal = trainerBooking.Goal,
                    Alergy = trainerBooking.Alergy,
                    DietOptions = trainerBooking.DietOptions
                };
                trainerBookingVM.DeserializeDietOptions();
                return View(trainerBookingVM);
            }
            return HttpNotFound("Booking not found!");
        }
        [HttpPost]
        public ActionResult Edit(TrainerBookingVM model)
        {
            if (ModelState.IsValid)
            {
                TrainerBookings trainerBooking = _db.TrainerBookings.Find(model.Id);
                if (trainerBooking != null)
                {
                    trainerBooking.EmailAddress = model.EmailAddress;
                    trainerBooking.Weight = model.Weight;
                    trainerBooking.Height = model.Height;
                    trainerBooking.Goal = model.Goal;
                    trainerBooking.Alergy = model.Alergy;
                    trainerBooking.DietOptions = model.SerializeDietOptions();
                    _db.Entry(trainerBooking).State = System.Data.Entity.EntityState.Modified;
                    _db.SaveChanges();
                    return RedirectToAction("Booking", new { model.Id });
                }
                ModelState.AddModelError("", "Booking not found!");
            }
            return View(model);
        }

        public ActionResult Cancel(int Id)
        {
            TrainerBookings trainerBooking = _db.TrainerBookings.Find(Id);
            if (trainerBooking != null)
            {
                if (trainerBooking.TrainerId != 0)
                {
                    Models.Trainers trainer = _db.Trainers.Find(trainerBooking.TrainerId);
                    trainer.IsBooked = false;
                    trainer.TrainerBookingId = 0;
                    trainer.TrainerBooking = new List<TrainerBookings>();
                    _db.Entry(trainer).State = System.Data.Entity.EntityState.Modified;
                }
                _db.TrainerBookings.Remove(trainerBooking);
                _db.SaveChanges();
                return RedirectToAction("CancelSuccess");
            }
            return HttpNotFound("Booking not found!");
        }

        public ActionResult CancelSuccess()
        {
            return View();
        }






        public ActionResult TrainerBookings()
        {
            var trainerBookings = _db.TrainerBookings.ToList();
            foreach (var booking in trainerBookings)
            {
                var trainer = _db.Trainers.Find(booking.TrainerId);
                if (trainer != null)
                {
                    booking.Trainer = new List<Trainers>();
                    booking.Trainer.Add(trainer);
                }
            }
            ViewBag.AvailableTrainers = _db.Trainers.Where(t=>t.IsBooked == false).ToList();
            return View(trainerBookings);
        }
        public ActionResult AddTrainer()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddTrainer(Trainers model)
        {
            if (ModelState.IsValid)
            {
                _db.Trainers.Add(model);
                _db.SaveChanges();
                return RedirectToAction("Trainers");
            }
            return View();
        }
        public ActionResult Trainers()
        {
            return View(_db.Trainers.ToList());
        }
        public ActionResult DeleteTrainer(int Id)
        {
            Trainers trainer = _db.Trainers.Find(Id);
            if (trainer != null)
            {
                _db.Trainers.Remove(trainer);
                _db.SaveChanges();
                return RedirectToAction("Trainers");
            }
            return HttpNotFound();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AssignTrainer(AssignTrainerVM model)
        {
            if (ModelState.IsValid)
            {
                Models.Trainers trainer = _db.Trainers.Find(model.TrainerId);
                Models.TrainerBookings trainerBooking = _db.TrainerBookings.Find(model.TrainerBookingId);

                trainer.IsBooked = true;
                trainer.TrainerBookingId = trainerBooking.Id;
                trainer.TrainerBooking = new List<TrainerBookings>();
                trainer.TrainerBooking.Add(trainerBooking);

                trainerBooking.TrainerStatus = true;
                trainerBooking.TrainerId = trainer.Id;
                trainerBooking.Trainer = new List<Trainers>();
                trainerBooking.Trainer.Add(trainer);

                _db.Entry(trainer).State = System.Data.Entity.EntityState.Modified;
                _db.Entry(trainerBooking).State = System.Data.Entity.EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("TrainerBookings");
            }
            return HttpNotFound("Something went wrong!");
        }
    }
}