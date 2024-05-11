using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GymApp.Models
{
    public class Order
    {
        //Customer Order
        [Key]
        public int Id { get; set; }
        public int Qty { get; set; }
        public int Unit_Price { get; set; }
        public float Order_Bill { get; set; }
        public DateTime? Order_Date { get; set; }
        public int? FkProdId { get; set; }
        [ForeignKey("FkProdId")]
        public virtual Products prodcts { get; set; }
        public int? FkInvoiceID { get; set; }
        [ForeignKey("FkInvoiceID")]
        public virtual InvoiceModel invoices { get; set; }

        //Delivery
        public string DeliveryFirstName { get; set; }
        public string DeliveryLastName { get; set; }
        public string DeliveryPhoneNum { get; set;}
        public string DeliveryAddress { get; set; }
        public string DeliverySubrub { get; set; }
        public string DeliveryOption { get; set; }
        public string AssignedDriver { get; set; }
        public int AssignedVehicle { get; set; }
        public string Status { get; set; }
        public bool IsDelivered { get; set; }
        public string ZipCode { get; set; }
        public DateTime? DeliveryDate { get; set;}//Assigned By Admin

        //Driver Details
        public string DriverName { get; set; }
        public string DriverVehicle { get; set; }
        public string NumberPlate { get; set; }
        public bool IsPreparing { get; set; }
        public bool IsArriving { get; set; }
        public bool IsReceived { get; set; }
        public string TrackingNumber { get; set; }
        public Order()
        {
            var guid = Guid.NewGuid().ToString();
            this.IsArriving = false;
            this.IsReceived = true;
            this.IsPreparing = false;
            this.IsDelivered = false;
            this.TrackingNumber = guid.Substring(6).ToUpper();

        }
    }
}