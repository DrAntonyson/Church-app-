using GymApp.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GymApp.Controllers
{
    public class VehicleController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        // GET: Vehicle
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult AddVehicle()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddVehicle(Vehicle model, HttpPostedFileBase file)
        {
            //check if a file was uploaded
            if (file != null && file.ContentLength > 0)
            {
                //get file extension
                string ext = file.ContentType.ToLower();
                //verify extension
                if (ext != "image/jpg" &&
                    ext != "image/jpeg" &&
                    ext != "image/pjpeg" &&
                    ext != "image/gif" &&
                    ext != "image/x-png" &&
                    ext != "image/png")
                {
                    ModelState.AddModelError("", "Wrong Image format !");
                    return View(model);
                }
            }

            using (var db = new ApplicationDbContext())
            {
                // Check if the number plate already exists
                if (db.Vehicles.Any(v => v.NumberPlate == model.NumberPlate))
                {
                    ModelState.AddModelError("", "Number Plate already exists!");
                    return View(model);
                }

                Vehicle g = new Vehicle()
                {
                    VehicleName = model.VehicleName,
                    VehicleColor = model.VehicleColor,
                    NumberPlate = model.NumberPlate,
                    Image = file.FileName,
                    Status = "Available",
                    TraveledDistance = 0,
                    StatusNum = 1,
                };

                db.Vehicles.Add(g);
                db.SaveChanges();

                int id = g.Id;

                #region add image

                //create necessary directories
                var dir = new DirectoryInfo(string.Format("{0}Images\\Uploads", Server.MapPath(@"\")));
                var pathString1 = Path.Combine(dir.ToString(), "Vehicles");
                var pathString2 = Path.Combine(dir.ToString(), "Vehicles\\" + id.ToString());

                if (!Directory.Exists(pathString1))
                {
                    Directory.CreateDirectory(pathString1);
                }

                if (!Directory.Exists(pathString2))
                {
                    Directory.CreateDirectory(pathString2);
                }

                //initilize image name
                string imageName = file.FileName;

                var path = string.Format("{0}\\{1}", pathString2, imageName);

                //save original image
                file.SaveAs(path);

                #endregion

                return Redirect("/Vehicle/VehicleList");
            }
        }
        public ActionResult VehicleList()
        {
            return View(db.Vehicles.ToList());
        }
        public ActionResult ManageVehicles()
        {
            return View();
        }
    }
}