using System;

namespace CIM.Model.Models.Booking
{
    public class Ticket
    {
        public int TicketId { get; set; }
        public DateTime DepartTime { get; set; }
        public DateTime ReturntTime { get; set; }
    }
}
