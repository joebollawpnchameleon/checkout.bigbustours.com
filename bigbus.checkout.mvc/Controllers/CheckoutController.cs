
using bigbus.checkout.mvc.Helpers;
using Services.Infrastructure;
using System.Web.Mvc;

namespace bigbus.checkout.mvc.Controllers
{
    public class CheckoutController : BaseController
    {
        #region Injectable properties (need to be public)

        public IBasketService BasketService { get; set; }
        public ICountryService CountryService { get; set; }
        public IUserService UserService { get; set; }
        public IPciApiServiceNoASync PciApiService { get; set; }
        public ICurrencyService CurrencyService { get; set; }
        public ITicketService TicketService { get; set; }        
        public IPaypalService PaypalService { get; set; }
        public IImageDbService ImageDbService { get; set; }
        public IImageService ImageService { get; set; }
        public IEcrService EcrService { get; set; }
        public ICheckoutService CheckoutService { get; set; }       

        #endregion

        // GET: Checkout
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult UserDetails()
        {
            return View();
        }

    }
}