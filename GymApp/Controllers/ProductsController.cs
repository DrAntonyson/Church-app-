using GymApp.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Web;
using System.Web.Mvc;

namespace GymApp.Controllers
{
    public class ProductsController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Error()
        {
            return View();
        }
        public ActionResult Index()
        {
            List<Products> products = db.Products.ToList<Products>();
            return View(products);
        }
        
        [HttpGet]
        public ActionResult CreateNewProduct()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CreateNewProduct(HttpPostedFileBase file , Products products)
        {
            if(file != null && file.ContentLength > 0)
                try
                {
                    string path = Path.Combine(Server.MapPath("~/Images"),
                                               Path.GetFileName(file.FileName));
                    file.SaveAs(path);
                    string filename= file.FileName;
                    ViewBag.Message = "File uploaded successfully";
                    products.ProductPicture = "Images/"+ filename;
                    db.Products.Add(products);
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    ViewBag.Message = "ERROR:" + ex.Message.ToString();
                }
            else
            {
                ViewBag.Message = "You have not specified a file.";
            } 
            return View();
        }
    
        [HttpGet]
        public ActionResult EditProduct(int id)
        {
            Products products = db.Products.Find(id);
            if (products == null)
            {
                return HttpNotFound();
            }
            return View(products);
        }
        [HttpPost]
        public ActionResult EditProduct(HttpPostedFileBase file, Products products)
        {
            if (file != null && file.ContentLength > 0)
                try
                {
                    string path = Path.Combine(Server.MapPath("~/Images"),
                                               Path.GetFileName(file.FileName));
                    file.SaveAs(path);
                    string filename = file.FileName;
                    ViewBag.Message = "File uploaded successfully";
                    products.ProductPicture = "Images/" + filename;
                    db.Entry(products).State = EntityState.Modified;
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    ViewBag.Message = "ERROR:" + ex.Message.ToString();
                }
            else
            {
                ViewBag.Message = "You have not specified a file.";
            }
            return RedirectToAction("ViewProductsAdmin", "Products");
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            Products products = db.Products.Find(id);
            db.Products.Remove(products);
            db.SaveChanges();
            return RedirectToAction("Index", "Admin");
        }

        public ActionResult ViewProductsAdmin()
        {
            List<Products> products = db.Products.ToList<Products>();
            return View(products);
        }
        List<Cart> li = new List<Cart>();

        [HttpGet]
        public ActionResult addToCart(int? Id)
        {
            if (Request.IsAuthenticated)
            {
                ViewBag.Title = "Add to cart";
                Products products = db.Products.Find(Id);
                return View(products);
            }
            else
            {
                return RedirectToAction("Login","Account");
            }
        }
        [HttpPost]
        public ActionResult addToCart(int Id, string number)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            string userId = User.Identity.GetUserId();
            ApplicationUser user = db.Users.Find(userId);

            Products products = db.Products.Find(Id);
            Cart cart = new Cart();

            cart.productId = products.id;
            cart.productName = products.ProductName;
            cart.productPic = products.ProductPicture;
            cart.price = products.ProductPrice;

            cart.qty = Convert.ToInt32(number);
            cart.bill = cart.price * cart.qty;

            if (TempData["cart"] == null)
            {
                li.Add(cart);
                TempData["cart"] = li;
            }
            else
            {
                List<Cart> li2 = TempData["cart"] as List<Cart>;

                int flag = 0;
                foreach (var item in li2)
                {
                    if (item.productId == cart.productId)
                    {
                        item.qty += cart.qty;
                        item.bill += cart.bill;
                        flag = 1;
                    }
                }
                if (flag == 0)
                {
                    li2.Add(cart);
                }
                TempData["cart"] = li2;
            }
            TempData.Keep();
            return RedirectToAction("Checkout","Products");
        }
        public ActionResult Checkout()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            string userId = User.Identity.GetUserId();
            ApplicationUser user = db.Users.Find(userId);

            ViewBag.Title = "Checkout";
            TempData.Keep();
            if (TempData["cart"] != null)
            {
                float x = 0;
                List<Cart> li2 = TempData["cart"] as List<Cart>;
                foreach (var item in li2)
                {
                    x += item.bill;                 
                }
                
                if (user.Subscription == "MonthlyMembership")
                {
                    TempData["totalDiscount"] = x * 0.05F;
                    TempData["total"] = x - (float)TempData["totalDiscount"];
                }
                else if (user.Subscription == "YearlyMembership")
                {
                    TempData["totalDiscount"] = x * 0.10F;
                    TempData["total"] = x - (float)TempData["totalDiscount"];
                }
                else if (user.Subscription == "PremiumYearly")
                {
                    TempData["totalDiscount"] = x * 0.20F;
                    TempData["total"] = x - (float)TempData["totalDiscount"];
                }
                else
                {
                    TempData["totalDiscount"] = 0;
                    TempData["total"] = x;
                }
                
            }
            TempData.Keep();
            return View();
        }
        public ActionResult Payment()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            string userId = User.Identity.GetUserId();
            ApplicationUser user = db.Users.Find(userId);

            Session["isDelivery"]?.ToString();
            ViewBag.Title = "Payment";
            TempData.Keep();
            if (TempData["cart"] != null)
            {
                float x = 0;
                List<Cart> li2 = TempData["cart"] as List<Cart>;
                foreach (var item in li2)
                {
                    x += item.bill;
                }
                if (user.Subscription == "MonthlyMembership")
                {
                    TempData["totalDiscount"] = x * 0.05F;
                    TempData["total"] = x - (float)TempData["totalDiscount"];
                }
                else if (user.Subscription == "YearlyMembership")
                {
                    TempData["totalDiscount"] = x * 0.10F;
                    TempData["total"] = x - (float)TempData["totalDiscount"];
                }
                else if (user.Subscription == "PremiumYearly")
                {
                    TempData["totalDiscount"] = x * 0.20F;
                    TempData["total"] = x - (float)TempData["totalDiscount"];
                }
                else
                {
                    TempData["totalDiscount"] = 0;
                    TempData["total"] = x;
                }
            }          
            TempData.Keep();
            return View();
        }
        [HttpPost]
        public ActionResult SetDelivery(string isDelivery)
        {
            Session["isDelivery"] = isDelivery;
            return Json(new { success = true });
        }
        [HttpPost]
        public ActionResult Payment(Order order)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            string userId = User.Identity.GetUserId();
            ApplicationUser user = db.Users.Find(userId);

            string x = (string)Session["isDelivery"];
            if (x == "Yes") 
            {
                string iduser = User.Identity.GetUserId();

                List<Cart> li = TempData["cart"] as List<Cart>;
                InvoiceModel invoice = new InvoiceModel();

                invoice.FKUserID = iduser;
                invoice.DateInvoice = System.DateTime.Now;
                invoice.Total_Bill = (float)TempData["Total"];
                invoice.status = "PaidDelivery";
                db.invoiceModel.Add(invoice);
                db.SaveChanges();

                Order odr = new Order();
                foreach (var item in li)
                {
                    odr.FkProdId = item.productId;
                    odr.FkInvoiceID = invoice.ID;
                    odr.Order_Date = System.DateTime.Now;
                    odr.Status = "PaidDelivery";
                    odr.Qty = item.qty;
                    Products products = db.Products.Find(item.productId);
                    products.Qty = products.Qty - item.qty;

                    odr.Unit_Price = (int)item.price;

                    if (user.Subscription == "MonthlyMembership")
                    {
                        odr.Order_Bill = item.bill-(item.bill * 0.05F);
                    }
                    else if (user.Subscription == "YearlyMembership")
                    {
                        odr.Order_Bill = item.bill - (item.bill * 0.10F);
                    }
                    else if (user.Subscription == "PremiumYearly")
                    {
                        odr.Order_Bill = item.bill - (item.bill * 0.20F);
                    }
                    else
                    {
                        odr.Order_Bill = item.bill;
                    }
                    

                    //Delivery 
                    odr.DeliveryFirstName = order.DeliveryFirstName;
                    odr.DeliveryLastName = order.DeliveryLastName;
                    odr.DeliveryPhoneNum = order.DeliveryPhoneNum;
                    odr.DeliveryAddress = order.DeliveryAddress;
                    odr.DeliverySubrub = order.DeliverySubrub;
                    odr.ZipCode = order.ZipCode;

                    db.orders.Add(odr);
                    db.SaveChanges();
                }
                TempData.Remove("total");
                TempData.Remove("cart");
                Session.Remove("isDelivery");
                TempData["OrderPlaced"] = "Your order has been placed successfully! Your Delivery Info will Update soon!";
                TempData.Keep();
                return RedirectToAction("Index","HomePageTest");
            }
            else if (x == "No")
            {
                string iduser = User.Identity.GetUserId();

                List<Cart> li = TempData["cart"] as List<Cart>;
                InvoiceModel invoice = new InvoiceModel();

                invoice.FKUserID = iduser;
                invoice.DateInvoice = System.DateTime.Now;
                invoice.Total_Bill = (float)TempData["Total"];
                invoice.status = "Paid";
                db.invoiceModel.Add(invoice);
                db.SaveChanges();

                foreach (var item in li)
                {
                    Order odr = new Order();
                    odr.FkProdId = item.productId;
                    odr.FkInvoiceID = invoice.ID;
                    odr.Order_Date = System.DateTime.Now;

                    odr.Qty = item.qty;
                    Products products = db.Products.Find(item.productId);
                    products.Qty -= item.qty;

                    odr.Unit_Price = (int)item.price;
                    if (user.Subscription == "MonthlyMembership")
                    {
                        odr.Order_Bill = item.bill - (item.bill * 0.05F);
                    }
                    else if (user.Subscription == "YearlyMembership")
                    {
                        odr.Order_Bill = item.bill - (item.bill * 0.10F);
                    }
                    else if (user.Subscription == "PremiumYearly")
                    {
                        odr.Order_Bill = item.bill - (item.bill * 0.20F);
                    }
                    else
                    {
                        odr.Order_Bill = item.bill;
                    }
                    db.orders.Add(odr);
                    db.SaveChanges();
                }
                TempData.Remove("total");
                TempData.Remove("cart");
                Session.Remove("isDelivery");
                TempData["OrderPlaced"] = "Your order has been placed successfully!";
                TempData.Keep();
                return RedirectToAction("Index","HomePageTest");
            }
            else
            {
                //error data lost
                return RedirectToAction("Error");
            }
        }
        [HttpPost]
        public ActionResult PaymentWithGymAccount(Order order)
        {
            string x = (string)Session["isDelivery"];
            if (x == "Yes")
            {
                string iduser = User.Identity.GetUserId();

                List<Cart> li = TempData["cart"] as List<Cart>;
                InvoiceModel invoice = new InvoiceModel();

                invoice.FKUserID = iduser;
                invoice.DateInvoice = System.DateTime.Now;
                invoice.Total_Bill = (float)TempData["Total"];

                string userId = User.Identity.GetUserId();
                ApplicationUser user = db.Users.Find(userId);
                user.Account = user.Account - invoice.Total_Bill;

                invoice.status = "PaidDelivery";
                db.invoiceModel.Add(invoice);
                db.SaveChanges();

                Order odr = new Order();
                foreach (var item in li)
                {
                    odr.FkProdId = item.productId;
                    odr.FkInvoiceID = invoice.ID;
                    odr.Order_Date = System.DateTime.Now;
                    odr.Status = "PaidDelivery";
                    odr.Qty = item.qty;
                    Products products = db.Products.Find(item.productId);
                    products.Qty = products.Qty - item.qty;

                    odr.Unit_Price = (int)item.price;
                    if (user.Subscription == "MonthlyMembership")
                    {
                        odr.Order_Bill = item.bill - (item.bill * 0.05F);
                    }
                    else if (user.Subscription == "YearlyMembership")
                    {
                        odr.Order_Bill = item.bill - (item.bill * 0.10F);
                    }
                    else if (user.Subscription == "PremiumYearly")
                    {
                        odr.Order_Bill = item.bill - (item.bill * 0.20F);
                    }
                    else
                    {
                        odr.Order_Bill = item.bill;
                    }

                    //Delivery 
                    odr.DeliveryFirstName = order.DeliveryFirstName;
                    odr.DeliveryLastName = order.DeliveryLastName;
                    odr.DeliveryPhoneNum = order.DeliveryPhoneNum;
                    odr.DeliveryAddress = order.DeliveryAddress;
                    odr.DeliverySubrub = order.DeliverySubrub;
                    odr.ZipCode = order.ZipCode;

                    db.orders.Add(odr);
                    db.SaveChanges();
                }
                TempData.Remove("total");
                TempData.Remove("cart");
                Session.Remove("isDelivery");
                TempData["OrderPlaced"] = "Your order has been placed successfully! Your Delivery Info will Update soon!";
                TempData.Keep();
                return RedirectToAction("Index", "HomePageTest");
            }
            else if (x == "No")
            {
                string iduser = User.Identity.GetUserId();

                List<Cart> li = TempData["cart"] as List<Cart>;
                InvoiceModel invoice = new InvoiceModel();

                invoice.FKUserID = iduser;
                invoice.DateInvoice = System.DateTime.Now;
                invoice.Total_Bill = (float)TempData["Total"];

                string userId = User.Identity.GetUserId();
                ApplicationUser user = db.Users.Find(userId);
                user.Account = user.Account - invoice.Total_Bill;
                invoice.status = "Paid";
                db.invoiceModel.Add(invoice);
                db.SaveChanges();

                foreach (var item in li)
                {
                    Order odr = new Order();
                    odr.FkProdId = item.productId;
                    odr.FkInvoiceID = invoice.ID;
                    odr.Order_Date = System.DateTime.Now;

                    odr.Qty = item.qty;
                    Products products = db.Products.Find(item.productId);
                    products.Qty -= item.qty;

                    odr.Unit_Price = (int)item.price;
                    if (user.Subscription == "MonthlyMembership")
                    {
                        odr.Order_Bill = item.bill - (item.bill * 0.05F);
                    }
                    else if (user.Subscription == "YearlyMembership")
                    {
                        odr.Order_Bill = item.bill - (item.bill * 0.10F);
                    }
                    else if (user.Subscription == "PremiumYearly")
                    {
                        odr.Order_Bill = item.bill - (item.bill * 0.20F);
                    }
                    else
                    {
                        odr.Order_Bill = item.bill;
                    }
                    db.orders.Add(odr);
                    db.SaveChanges();
                }

                TempData.Remove("total");
                TempData.Remove("cart");
                Session.Remove("isDelivery");
                TempData["OrderPlaced"] = "Your order has been placed successfully!";
                TempData.Keep();
                return RedirectToAction("Index", "HomePageTest");
            }
            else
            {
                //error data lost
                return RedirectToAction("Error");
            }
        }
        public ActionResult Remove(int? id)
        {
            List<Cart> li2 = TempData["cart"] as List<Cart>;
            Cart c = li2.Where(x => x.productId == id).SingleOrDefault();
            li2.Remove(c);
            float h = 0;
            foreach(var item in li2)
            {
                h += item.bill;
            }
            TempData["total"] = h;
            return RedirectToAction("Checkout");
        }

    }
}