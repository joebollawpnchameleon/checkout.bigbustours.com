using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using bigbus.checkout.data.Model;
using bigbus.checkout.Models;
using Common.Enums;
using Services.Infrastructure;
using Services.Implementation;

namespace bigbus.checkout
{
    public partial class Default : BasePage
    {
      
        protected void Page_Load(object sender, EventArgs e)
        {
        //    var basket = BasketService.GetBasket(new Guid("B2E65DBA-925E-4407-9F1E-A004314A7021"));
        //    var lines = basket.BasketLines;
            if(!IsPostBack)
                PlantCookie(sender, e);

            //var checkoutService = new CheckoutService();
            //var order = checkoutService.GetFullOrder("5A9A8646-E714-4DB5-9978-C70AA57D65C2");

            var order = CheckoutService.GetFullOrder("A40C2417-8A29-44FE-BA93-A744A414633F");

            SendBookingToEcr(order);
        }

        protected void PlantCookie(object sender, EventArgs e)
        {

            //var imageFolder = ImageDbService.GetImageFolder("london");
            //var name = imageFolder.FolderName;

            var basket = BasketService.GetLatestBasket();
            txtCookieValue.Text = IncreaseCookieValue(basket.ExternalCookieValue);

            AuthenticationService.SetCookie(ExternalBasketCookieName, SessionCookieDomain, txtCookieValue.Text);

            //var cookie = new HttpCookie(ExternalBasketCookieName,txtCookieValue.Text)
            //{
            //    Domain = SessionCookieDomain,
            //    HttpOnly = true
            //};

            //HttpContext.Current.Response.Cookies.Add(cookie);
        }

        private string IncreaseCookieValue(string cookieValue)
        {
            if (string.IsNullOrEmpty(cookieValue) || cookieValue.Length < 11)
                return string.Empty;

            var val = cookieValue.Substring(19);
            var newval = Convert.ToInt32(val);
            newval++;
            return cookieValue.Replace(val, newval.ToString());
        }

        protected void GoToCheckout(object sender, EventArgs e)
        {
            Response.Redirect("~/BookingAddress.aspx");
        }

        //protected EcrResult SendBookingToEcr(Order order)
        //{
        //    Log("Sending booking to ECR");

        //    var orderId = order.Id.ToString();
        //    var orderLineDetails = CheckoutService.GetOrderLineDetails(orderId);

        //    if (orderLineDetails == null)
        //    {
        //        Log("Could not retrieve orderline details for order id: " + orderId);
        //        return new EcrResult { ErrorMessage = "Booking failed for ECR OrderId: " + order.Id + " couldn't retrieve orderline details.", Status = EcrResponseCodes.BookingFailure };
        //    }

        //    //check site that doesn't support QR Code as all may need it.
        //    var queryVersionGroups =
        //       from detail in orderLineDetails
        //       group detail by detail.NewCheckoutVersionId into versionGroup
        //       orderby versionGroup.Key
        //       select versionGroup;

        //    foreach (var versionGroup in queryVersionGroups)
        //    {
        //        var ecrVersionId = versionGroup.Key;

        //        if (ecrVersionId == (int) EcrVersion.Three)
        //        {
        //            var response = SendBookingToEcr3(order, versionGroup.ToList());

        //            //check response is OK
        //            if (response == null)
        //            {
        //                return new EcrResult { ErrorMessage = "Booking failed for ECR OrderId: " + order.Id, Status = EcrResponseCodes.BookingFailure };
        //            }

        //            //check barcode are available
        //            if (response.Barcodes == null || response.Barcodes.Length < 1)
        //            {
        //                return new EcrResult
        //                {
        //                    ErrorMessage = "Booking failed (no barcode returned for ECR OrderId: " + order.Id,
        //                    Status = EcrResponseCodes.QrCodeRetrievalFailure
        //                };
        //            }

        //            order.EcrBookingShortReference = response.TransactionReference;
        //            CheckoutService.SaveOrder(order);

        //            Log("Saving external barcodes");
        //            SaveBarcodes(response, order.OrderNumber);

        //            return new EcrResult { Status = EcrResponseCodes.BookingSuccess };
        //        }

        //        if (ecrVersionId != (int) EcrVersion.One) continue;

        //        SendBookingToEcr1(order, versionGroup.ToList());
        //        return new EcrResult { Status = EcrResponseCodes.BookingSuccess };
        //    }

        //    return new EcrResult { ErrorMessage = "Booking failed for ECR OrderId: " + order.Id + " couldn't retrieve orderline details.", Status = EcrResponseCodes.BookingFailure }; ;
           
        //}
    }
}