using GymApp.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Odbc;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;
using System.Web.Mvc;

namespace GymApp.Controllers
{
    public class HomePageTestController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            if (User.IsInRole("Admin"))
            {
                return View("~/Views/Admin/Index.cshtml", "_LayoutAdminPage");
            }
            else if (User.IsInRole("Driver"))
            {
                return RedirectToAction("PendingOrders", "Driver");
            }
            else
            {
                if (TempData.ContainsKey("OrderPlaced"))
                {
                    // Get the success message from TempData
                    string successMessage = TempData["OrderPlaced"] as string;
                    // Pass the success message to the view
                    ViewBag.OrderPlacedMessage = successMessage;
                    TempData.Remove("OrderPlaced");
                }
                ViewBag.Title = "Home";
                List<Products> products = db.Products.ToList<Products>();
                return View(products);
            }
           
        }
        public ActionResult Membership()
        {
            ViewBag.Title = "Membership";
            return View();
        }
        [HttpGet]
        public ActionResult MonthlyMembership()
        {
            if (Request.IsAuthenticated)
            {
                ViewBag.Title = "MonthlyMembership";
                return View();
            }
            return RedirectToAction("login", "Account");
        }
        [HttpPost]
        public ActionResult MonthlyMembership(ApplicationUser user)
        {
            string userId = User.Identity.GetUserId();
            user = db.Users.Find(userId);
            user.Subscription = "MonthlyMembership";

            db.SaveChanges();
            return RedirectToAction("Index", "HomePageTest");
        }
        [HttpGet]
        public ActionResult PremiumYearly()
        {
            if (Request.IsAuthenticated)
            {
                ViewBag.Title = "PremiumYearly";
                return View();
            }
            return RedirectToAction("login", "Account");
        }
        [HttpPost]
        public ActionResult PremiumYearly(ApplicationUser user)
        {
            string userId = User.Identity.GetUserId();
            user = db.Users.Find(userId);
            user.Subscription = "PremiumYearly";

            db.SaveChanges();
            return RedirectToAction("Index", "HomePageTest");
        }
        [HttpGet]
        public ActionResult YearlyMembership()
        {
            if (Request.IsAuthenticated)
            {
                ViewBag.Title = "YearlyMembership";
                return View();
            }
            return RedirectToAction("login", "Account");
        }
        [HttpPost]
        public ActionResult YearlyMembership(ApplicationUser user)
        {
            string userId = User.Identity.GetUserId();
            user = db.Users.Find(userId);
            user.Subscription = "YearlyMembership";

            db.SaveChanges();
            return RedirectToAction("Index", "HomePageTest");
        }
        [HttpGet]
        public ActionResult OrderHistory()
        {
            var userId = User.Identity.GetUserId();

            var orderHistory = (from invoice in db.invoiceModel
                                join order in db.orders on invoice.ID equals order.FkInvoiceID
                                where invoice.FKUserID == userId
                                group order by order.FkInvoiceID into groupedOrders
                                select groupedOrders.FirstOrDefault()).ToList();

            return View(orderHistory);
        }
        [HttpGet]
        public ActionResult Invoice(int id)
        {
            var Invoice = (from invoice in db.invoiceModel
                                join order in db.orders on invoice.ID equals order.FkInvoiceID
                                where invoice.ID == id
                                select order).ToList();


            TempData["OrderID"]= id;
            return View(Invoice);
        }

        public ActionResult Cancel(int id)
        {
            Order order = db.orders.Find(id);
            TempData["CancelID"] = id;
            return View(order);
        }
        [HttpPost]
        public ActionResult Cancel(Order orderview)
        {
            int id = int.Parse(TempData["CancelID"].ToString());

            Order order = db.orders.Find(id);            

            InvoiceModel invoiceModel = db.invoiceModel.Find(order.FkInvoiceID);        

            Products products = db.Products.Find(order.prodcts.id);
            products.Qty += order.Qty;

            invoiceModel.RefundType = orderview.invoices.RefundType;
            string RefundType = orderview.invoices.RefundType;
            if (RefundType == "BankAccount")
            {
                invoiceModel.status = "Order Canceled \n ~Refund to Bank Account~";
                ViewBag.OrderCancelMessage = "Your Order Has Been Canceled! \n ~Refund to Bank Account~";
                //Email code
            }
            else if(RefundType == "GymAccount")
            {
                string userId = User.Identity.GetUserId();

                // Retrieve the user from the database using the user ID
                ApplicationUser user = db.Users.Find(userId);

                // Update the user's account amount
                user.Account = user.Account + invoiceModel.Total_Bill;

                // Save the changes to the database
                db.SaveChanges();

                // Set the message to be displayed on the next page using ViewBag
                ViewBag.OrderCancelMessage = "Your Order Has Been Canceled! \n ~Refund to Gym Account~";

                // Update the invoice status
                invoiceModel.status = "Order Canceled \n ~Refund to Gym Account~";
            }
            else
            {
                ViewBag.OrderCancelMessage = "Error";
            }

            db.SaveChanges();

            TempData.Remove("CancelID");
            return RedirectToAction("Index", "HomePageTest");
        }

        [HttpGet]
        public ActionResult TrackOrder(int id)
        {
            var Invoice = (from invoice in db.invoiceModel
                           join order in db.orders on invoice.ID equals order.FkInvoiceID
                           where invoice.ID == id
                           select order).ToList();
            TempData["OrderID"] = id;
            return View(Invoice);
        }
    }
}