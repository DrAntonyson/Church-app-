using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using GymApp.Models;

namespace GymApp.Controllers
{
    public class BookClassesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: BookClasses
        public ActionResult Index()
        {
            var bookClassdatabase = db.BookClasses.Include(b => b.classtype).Include(b => b.trainer);
            var thisuser = User.Identity.Name;

            if (User.IsInRole("Admin"))
            {
                return View(bookClassdatabase.ToList());
            }
            var selection = bookClassdatabase.Where(dd => dd.UserBooked == null).ToList();

            return View("ReadOnlyIndex", selection);
        }

        public ActionResult userBookings()
        {
            var bookClassdatabase = db.BookClasses.Include(b => b.classtype).Include(b => b.trainer);

            var thisuser = User.Identity.Name;

            var selection = bookClassdatabase.Where(dd => dd.UserBooked == thisuser).ToList();

            return View(selection);

        }


        // GET: BookClasses/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BookClass bookClass = db.BookClasses.Find(id);
            if (bookClass == null)
            {
                return HttpNotFound();
            }
            return View(bookClass);
        }

        public ActionResult Book(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BookClass bookClass = db.BookClasses.Where(dd => dd.id == id).Include(ss => ss.classtype).Include(ee => ee.trainer).SingleOrDefault();

            if (bookClass == null)
            {
                return HttpNotFound();
            }
            ViewBag.classtypeid = new SelectList(db.classtypes, "id", "Reason", bookClass.classtypeid);
            ViewBag.TrainerId = new SelectList(db.Trainer, "TrainerId", "TrainerRegistrationId", bookClass.TrainerId);
            return View(bookClass);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Book([Bind(Include = "id,classtypeid,datepicker,TrainerId,UserBooked")] BookClass bookClass)
        {

            var fromdb = db.BookClasses.Where(ss => ss.id == bookClass.id).SingleOrDefault();

            string thisuser = User.Identity.Name;
            bookClass.id = fromdb.id;
            bookClass.TrainerId = fromdb.TrainerId;
            bookClass.UserBooked = thisuser;
            bookClass.classtypeid = fromdb.classtypeid;
            bookClass.datepicker = fromdb.datepicker;

            var pDate = fromdb.datepicker;
            string pame = thisuser;
            string pName = pame.Substring(0, pame.IndexOf('@'));

            string subjectinfo = $" Hello {pName}!";
            string Mstring = "🎉 Welcome to our gym, a place where fitness meets fun and dedication paves the way to your goals! Your fitness adventure begins here, and we're excited to be a part of it! Let's make every workout count and celebrate every milestone together. Here's to new challenges, stronger bodies, and the incredible camaraderie that flourishes within these walls. Welcome to the family! 💪";
            var fromtrainerdb = db.Trainer.Where(dd => dd.TrainerId == fromdb.TrainerId).SingleOrDefault();
            string trainerName = fromtrainerdb.Name;

            string bodyinfo = $"{Mstring}.The trainer which will be taking you will be {trainerName} at the following  date and time: {pDate}.We urge you to abide by the times as we have other sessions planned";


            if (ModelState.IsValid)
            {
                db.BookClasses.Remove(fromdb);
                db.BookClasses.Add(bookClass);
                db.SaveChanges();

                WebMail.Send(thisuser, subjectinfo, bodyinfo, null, null, null, true, null, null, null, null, null, null);


                return RedirectToAction("userBookings");
            }
            ViewBag.classtypeid = new SelectList(db.classtypes, "id", "typeclass", bookClass.classtypeid);
            ViewBag.TrainerId = new SelectList(db.Trainer, "TrainerId", "Name", bookClass.TrainerId);

            return View(bookClass);
        }


        // GET: BookClasses/Create
        public ActionResult Create()
        {
            ViewBag.classtypeid = new SelectList(db.classtypes, "id", "typeclass");
            ViewBag.TrainerId = new SelectList(db.Trainer, "TrainerId", "Name");
            return View();
        }

        // POST: BookClasses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,classtypeid,datepicker,TrainerId,UserBooked")] BookClass bookClass)
        {
            if (ModelState.IsValid)
            {
                db.BookClasses.Add(bookClass);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.classtypeid = new SelectList(db.classtypes, "id", "typeclass", bookClass.classtypeid);
            ViewBag.TrainerId = new SelectList(db.Trainer, "TrainerId", "Name", bookClass.TrainerId);
            return View(bookClass);
        }

        // GET: BookClasses/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BookClass bookClass = db.BookClasses.Find(id);
            if (bookClass == null)
            {
                return HttpNotFound();
            }
            ViewBag.classtypeid = new SelectList(db.classtypes, "id", "typeclass", bookClass.classtypeid);
            ViewBag.TrainerId = new SelectList(db.Trainer, "TrainerId", "Name", bookClass.TrainerId);
            return View(bookClass);
        }

        // POST: BookClasses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,classtypeid,datepicker,TrainerId,UserBooked")] BookClass bookClass)
        {
            if (ModelState.IsValid)
            {
                db.Entry(bookClass).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.classtypeid = new SelectList(db.classtypes, "id", "Reason", bookClass.classtypeid);
            ViewBag.TrainerId = new SelectList(db.Trainer, "TrainerId", "Pastor", bookClass.TrainerId);
            return View(bookClass);
        }

        // GET: BookClasses/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BookClass bookClass = db.BookClasses.Find(id);
            if (bookClass == null)
            {
                return HttpNotFound();
            }
            return View(bookClass);
        }

        // POST: BookClasses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BookClass bookClass = db.BookClasses.Find(id);
            db.BookClasses.Remove(bookClass);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
