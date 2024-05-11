using GymApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GymApp.Controllers
{
    public class AdminController : Controller
    {
       ApplicationDbContext db = new ApplicationDbContext();
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ListOfOrders()
        {
            float t = 0;
            List<Order> order = db.orders.ToList<Order>();
            foreach (var item in order)
            {
                t += item.Order_Bill;
            }
            TempData["OrderTotal"] = t;
            return View(order);
        }
        public ActionResult ListOfInvoices()
        {
            float t = 0;
            List<InvoiceModel> invoice = db.invoiceModel.ToList<InvoiceModel>();

            foreach (var item in invoice)
            {
                t += item.Total_Bill;


            }
            TempData["InvoiceTotal"] = t;
            return View(invoice);
        }
    }
}