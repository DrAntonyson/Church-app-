using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GymApp.Models;

namespace GymApp.Controllers
{
    public class classtypesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: classtypes
        public ActionResult Index()
        {
            return View(db.classtypes.ToList());
        }

        // GET: classtypes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            classtype classtype = db.classtypes.Find(id);
            if (classtype == null)
            {
                return HttpNotFound();
            }
            return View(classtype);
        }

        // GET: classtypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: classtypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,typeclass")] classtype classtype)
        {
            if (ModelState.IsValid)
            {
                db.classtypes.Add(classtype);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(classtype);
        }

        // GET: classtypes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            classtype classtype = db.classtypes.Find(id);
            if (classtype == null)
            {
                return HttpNotFound();
            }
            return View(classtype);
        }

        // POST: classtypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,typeclass")] classtype classtype)
        {
            if (ModelState.IsValid)
            {
                db.Entry(classtype).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(classtype);
        }

        // GET: classtypes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            classtype classtype = db.classtypes.Find(id);
            if (classtype == null)
            {
                return HttpNotFound();
            }
            return View(classtype);
        }

        // POST: classtypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            classtype classtype = db.classtypes.Find(id);
            db.classtypes.Remove(classtype);
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
