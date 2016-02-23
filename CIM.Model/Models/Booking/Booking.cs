
using System;

namespace CIM.Model.Models.Booking
{
    public class Booking
    {
        public int BookingId { get; set; }
        public DateTime PurchaseDateTime { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
