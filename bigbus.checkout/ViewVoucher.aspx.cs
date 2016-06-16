
using System;
using bigbus.checkout.data.Model;
using bigbus.checkout.Models;
using Services.Implementation;
using Services.Infrastructure;

namespace bigbus.checkout
{
    public partial class ViewVoucher : BasePage
    {
        public IClientRenderService PdfRendererService { get; set; }
       
        protected void Page_Load(object sender, EventArgs eventArgs)
        {
            AddMetas(@"<meta name=""robots"" content=""noindex"">");
          
            var orderId = Request.QueryString["oid"];

            if (string.IsNullOrEmpty(orderId))
                return;

            var order = CheckoutService.GetFullOrder(orderId);

            if (order == null)
            {
                Log("Order couldn't load for Orderid: " + orderId);
                Response.Redirect("~/Error/EvoucherError/");
                return;
            }
           
            try
            {
                var url = string.Concat(ResolveUrl("~/voucher.aspx?oid="), orderId);
                
                var buffer = PdfRendererService.GetBytesFromUrl(url, string.Format("Ticket #{0}", order.OrderNumber));
                
                order.NumbViewPdf++;
                order.OpenForPrinting = true;
                order.DatePdfLastViewed = LocalizationService.GetLocalDateTime(MicrositeId);
                CheckoutService.SaveOrder(order);

                Response.Clear();
                Response.AppendHeader("CONTENT-DISPOSITION", "inline; filename=" + orderId + ".pdf");
                Response.AppendHeader("CONTENT-TYPE", "application/pdf");
                Response.ContentType = "application/pdf";
                Response.AddHeader("Content-Length", buffer.Length.ToString());
                Response.AddHeader("X-Robots-Tag", "noindex");
                Response.OutputStream.Write(buffer, 0, buffer.Length);
                Response.OutputStream.Flush();
                Response.OutputStream.Close();
            }
            catch (Exception ex)
            {
               Log(ex.ToString());
            }

            Response.End();
        }
       
    }
}
