using GymApp.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using System.Net;

namespace GymApp.Controllers
{
    public class DriverController : Controller
    {
        // GET: Driver
        readonly ApplicationDbContext _db = new ApplicationDbContext();
        readonly ApplicationDbContext db = new ApplicationDbContext();
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public DriverController()
        {

        }

        public DriverController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        public ActionResult Index()
        {
            return View(_db.Drivers.ToList());
        }

        public ActionResult NewDriver()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> NewDriver(Driver driver)
        {

            if (ModelState.IsValid)
            {
                var Password = "Admin@111";
                var user = new ApplicationUser { UserName = driver.Email, Email = driver.Email, Name = driver.Name };
                var result = await UserManager.CreateAsync(user, Password);

                var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(_db));

                if (result.Succeeded)
                {
                    Driver _driver = new Driver();
                    _driver.Email = user.Email;
                    _driver.UserId = user.Id;
                    _driver.Name = driver.Name;

                    _db.Drivers.Add(_driver);
                    _db.SaveChanges();
                    if (!roleManager.RoleExists("Driver"))
                    {
                        var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole
                        {
                            Name = "Driver"
                        };
                        roleManager.Create(role);
                        UserManager.AddToRole(user.Id, "Driver");
                    }
                    else
                    {
                        UserManager.AddToRole(user.Id, "Driver");
                    }
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError("", "User Already Exist.");
            }
            return View();
        }
        public ActionResult AssignDriverToOrder(int? id)
        {
            Order _order = _db.orders.Find(id);
            if (_order == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var Drivers = _db.Drivers.Where(d => d.IsAvailable).ToList();
            ViewBag.Drivers = new SelectList(Drivers, "Id", "Name");

            var availableVehicles = _db.Vehicles.Where(v => v.Status == "Available").ToList();
            ViewBag.Vehicles = new SelectList(availableVehicles, "Id", "VehicleName");

            return View(_order);
        }

        [HttpPost]
        public ActionResult AssignDriverToOrder(int id, Order order)
        {
            int _driverId = Convert.ToInt32(order.AssignedDriver);
            int vehicleId = Convert.ToInt32(order.AssignedVehicle);
            Order _order = _db.orders.Find(id);
            if (_order == null)
            {
                return HttpNotFound();
            }

            Vehicle _vehicle = _db.Vehicles.FirstOrDefault(v => v.Id == vehicleId);
            if (_vehicle != null)
            {
                _vehicle.Status = "Assigned";
                _db.SaveChanges();
            }

            _order.AssignedVehicle = vehicleId;

            if (_vehicle != null)
            {
                _order.Status = "Vehicle with ID " + vehicleId + " has been assigned";
            }
            else
            {
                _order.Status = "Vehicle assigned";
            }
            Driver _driver = _db.Drivers.Where(d => d.Id == _driverId).FirstOrDefault();
            _order.AssignedDriver = _driver.UserId;

            if (_driver != null)
            {
                _order.Status = _driver.Name + " has been assigned";
            }
            else
            {
                _order.Status = "Driver assigned";
            }

            _db.SaveChanges();

            return RedirectToAction("ListOfOrders", "Admin");
        }
        public ActionResult PendingOrders()
        {
            var UserId = User.Identity.GetUserId();
            var orders = _db.orders.Where(o => o.IsDelivered == false && o.AssignedDriver == UserId)
                .ToList();

            foreach (var order in orders)
            {
                var vehicle = _db.Vehicles.FirstOrDefault(v => v.Id == order.AssignedVehicle);
                if (vehicle != null)
                {
                    order.NumberPlate = vehicle.NumberPlate;
                }
            }

            return View(new PendingOrdersViewModel { Orders = orders });
        }

        public ActionResult UpdateDistance(int orderId, double distance)
        {
            var order = db.orders.FirstOrDefault(o => o.Id == orderId);

            if (order != null)
            {
                var vehicle = db.Vehicles.FirstOrDefault(v => v.Id == order.AssignedVehicle);

                if (vehicle != null)
                {
                    // Convert the distance from meters to kilometers
                    double distanceInKilometers = distance / 1000;

                    // Round the distance to one decimal place
                    double roundedDistance = Math.Round(distanceInKilometers, 1);

                    // Set the value in Session
                    Session["FinalDistance"] = roundedDistance;

                    // Return a response indicating that distance is updated but not saved yet
                    return Json(new { success = true, message = "Distance updated. Click 'Confirm Delivery' to save changes." });
                }
            }

            return Json(new { success = false, message = "Failed to update distance" });
        }
        
        public ActionResult MyDeliveries()
        {
            var UserId = User.Identity.GetUserId();
            return View(_db.orders.Where(o => o.IsDelivered && o.AssignedDriver == UserId).ToList());
        }
        [HttpGet]
        public ActionResult OrderPickup(int Id)
        {
            TempData["OrderPickupId"] = Id;
            Order _order = _db.orders.Find(Id);
            if (_order == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return View(_order);
        }
        [HttpPost]
        public ActionResult OrderPickup()
        {
            int Id = (int)TempData["OrderPickupId"];
            Order _order = _db.orders.Find(Id);
            if (_order == null)
            {
                return HttpNotFound();
            }
            _order.IsPreparing = true;
            _order.Status = "Order picked";
            _db.SaveChanges();
            return RedirectToAction("PendingOrders");
        }

        public ActionResult ShipOrder(int? id)
        {
            Order _order = _db.orders.Find(id);
            if (_order == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return View(_order);
        }
        [HttpPost]
        public ActionResult ShipOrder(int id)
        {
            Order _order = _db.orders.Find(id);
            if (_order == null)
            {
                return HttpNotFound();
            }
            _order.IsArriving = true;
            _order.Status = "Order Is Arriving";
            _db.SaveChanges();
            return RedirectToAction("PendingOrders");
        }

        public ActionResult ConfirmDelivery(int? id)
        {
            Order _order = _db.orders.Find(id);
            if (_order == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return View(_order);
        }

        [HttpPost]
        public ActionResult ConfirmDelivery(int id)
        {
            Order _order = _db.orders.Find(id);
            if (_order == null)
            {
                return HttpNotFound();
            }

            _order.IsDelivered = true;
            _order.Status = "Order Is Delivered";

            // Update the status of the vehicle assigned to this order
            Vehicle _vehicle = db.Vehicles.FirstOrDefault(v => v.Id == _order.AssignedVehicle);
            if (_vehicle != null)
            {
                _vehicle.Status = "Available";
                var finalDistance = (double)Session["FinalDistance"];
                _vehicle.TraveledDistance += finalDistance;
                db.SaveChanges();
            }

            _db.SaveChanges();
            return RedirectToAction("PendingOrders");
        }



        public ActionResult ManageDrivers()
        {
            return View();
        }
    }
}