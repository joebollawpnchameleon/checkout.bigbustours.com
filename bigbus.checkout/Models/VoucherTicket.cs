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

        public ImageMetaData AttractionImageData { get; set; }

        public string BarCodeImageUrl { get; set; }

        public string BarCode { get; set; }

        public string QrCodeImageUrl { get; set; }

        public int BarCodeFixQuantity { get; set; }

        public bool UseQrCode { get; set; }

        public bool IsAttraction 
        { 
            get
            {
                return Ticket != null && Ticket.IsAttraction;
            } 
        }

        public bool IsTour
        {
            get
            {
                return Ticket != null && Ticket.IsTour;
            }
        }
    }
}