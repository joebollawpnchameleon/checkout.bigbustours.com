using System;
using System.Collections.Generic;
using bigbus.checkout.data.Model;
using Microsoft.SqlServer.Server;

namespace bigbus.checkout.Models
{
    public class VoucherTicket
    {
        public Ticket Ticket { get; set; }

        public string ValidTicketName { get; set; }

        public List<OrderLine> OrderLines { get; set; }

        public ImageMetaData ImageData { get; set; }

        public string AttractionImageUrl { get; set; }

        public bool IsAttraction 
        { 
            get
            {
                return Ticket != null && Ticket.TicketType.Equals("attraction", StringComparison.CurrentCultureIgnoreCase);
            } 
        }

        public bool IsTour
        {
            get
            {
                return Ticket != null && Ticket.TicketType.Equals("tour", StringComparison.CurrentCultureIgnoreCase);
            }
        }
    }
}