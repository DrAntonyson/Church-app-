using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GymApp.Models;

namespace GymApp.Controllers
{
    public class ReturnsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Returns
        public ActionResult Index()
        {
            var returns = db.Returns.Include(r => r.Order);
            return View(returns.ToList());
        }

        public ActionResult OrderIndex()
        {
            return View(db.orders.ToList());
        }


        // GET: Returns/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Returns returns = db.Returns.Find(id);
            if (returns == null)
            {
                return HttpNotFound();
            }
            ViewBag.Url=returns.imageUrl;
            return View(returns);
        }

        // GET: Returns/Create
        public ActionResult Create(int? id)
        {
            ViewBag.Id = new SelectList(db.orders, "Id", "DeliveryFirstName");
            return View();
        }

        // POST: Returns/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ReturnId,Id,ReturnReason,RequestedBy,IsApproved,imageUrl")] Returns returns,string ImageData)
        {
            if (ModelState.IsValid)
            {
                HttpPostedFileBase fileBase = Request.Files["ImageFile"];
                if (fileBase !=null && fileBase.ContentLength >0 )
                {
                    byte[] fileBytes; 
                    using (BinaryReader reader = new BinaryReader(fileBase.InputStream))
                    {
                        fileBytes= reader.ReadBytes(fileBase.ContentLength);
                    }
                    string base64String = Convert.ToBase64String(fileBytes);
                    returns.imageUrl=base64String;
                }
                returns.IsApproved = false;
                returns.RequestedBy=User.Identity.Name;
                db.Returns.Add(returns);
                db.SaveChanges();
                return RedirectToAction("OrderIndex");

            }
            

            ViewBag.Id = new SelectList(db.orders, "Id", "DeliveryFirstName", returns.Id);
            return View(returns);
        }

        public ActionResult Approve(int? id)
        {
            Returns returns = db.Returns.Find(id);
            returns.IsApproved = true;
            db.SaveChanges();
            return RedirectToAction("Index", new { successMessage = "Done for now" });
        }


        // GET: Returns/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Returns returns = db.Returns.Find(id);
            if (returns == null)
            {
                return HttpNotFound();
            }
            ViewBag.Id = new SelectList(db.orders, "Id", "DeliveryFirstName", returns.Id);
            return View(returns);
        }

        // POST: Returns/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ReturnId,Id,ReturnReason,RequestedBy,IsApproved,imageUrl")] Returns returns)
        {
            if (ModelState.IsValid)
            {
                db.Entry(returns).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Id = new SelectList(db.orders, "Id", "DeliveryFirstName", returns.Id);
            return View(returns);
        }

        // GET: Returns/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Returns returns = db.Returns.Find(id);
            if (returns == null)
            {
                return HttpNotFound();
            }
            return View(returns);
        }

        // POST: Returns/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Returns returns = db.Returns.Find(id);
            db.Returns.Remove(returns);
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
