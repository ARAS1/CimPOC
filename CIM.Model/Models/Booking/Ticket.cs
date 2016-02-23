using System;

namespace CIM.Model.Models.Booking
{
    public class Ticket
    {
        public int TicketId { get; set; }
        public DateTime departTime { get; set; }
        public DateTime returntTime { get; set; }
    }
}
