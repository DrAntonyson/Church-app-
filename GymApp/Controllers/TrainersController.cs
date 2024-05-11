using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using GymApp.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;

namespace GymApp.Controllers
{
    public class TrainersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Trainers
        public ActionResult Index()
        {
            return View(db.Trainers.ToList());
        }

        // GET: Trainers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Trainer trainer = db.Trainer.Find(id);
            if (trainer == null)
            {
                return HttpNotFound();
            }
            return View(trainer);
        }

        // GET: Trainers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Trainers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "TrainerId,TrainerRegistrationId,Name,UserId,Email,Phone,IsAssigned,IsAvailable")] Trainer trainer)
        {
            if (ModelState.IsValid)
            {
                var Password = "Trainer99";
                var user = new ApplicationUser
                {
                    UserName = trainer.Email,
                    Email = trainer.Email,
                    Name = trainer.Name
                };
                var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
                var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));

                var ExistingUser = UserManager.FindByEmail(trainer.Email);

                if (ExistingUser == null)
                {
                    var result = await UserManager.CreateAsync(user, Password);
                    if (result.Succeeded)
                    {
                        if (!roleManager.RoleExists("Trainer"))
                        {
                            var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole
                            {
                                Name = "Trainer"
                            };
                            roleManager.Create(role);
                            UserManager.AddToRole(user.Id, "Trainer");
                        }
                        else
                        {
                            UserManager.AddToRole(user.Id, "Trainer");
                        }
                        trainer.UserId = user.Id;

                        /*      Members mem = new Members();             This is to add the trainer into the member database aswell 
                              mem.Name = trainer.Name;
                              mem.email = trainer.Email;


                              db.MembersDatabase.Add(mem);
                              db.SaveChanges();
                        */

                        db.Trainer.Add(trainer);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }
            }
            ViewBag.Error = "Model is not valid please fill all the fields";

            return View(trainer);
        }
        public ActionResult assignRole()
        {
            var ListTrainers = db.Trainer.ToList();


            return View(ListTrainers);
        }
        // GET: Trainers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Trainer trainer = db.Trainer.Find(id);
            if (trainer == null)
            {
                return HttpNotFound();
            }
            return View(trainer);
        }

        // POST: Trainers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TrainerId,TrainerRegistrationId,Name,UserId,Email,Phone,IsAssigned,IsAvailable")] Trainer trainer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(trainer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(trainer);
        }

        // GET: Trainers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Trainer trainer = db.Trainer.Find(id);
            if (trainer == null)
            {
                return HttpNotFound();
            }
            return View(trainer);
        }

        // POST: Trainers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Trainer trainer = db.Trainer.Find(id);
            db.Trainer.Remove(trainer);
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
