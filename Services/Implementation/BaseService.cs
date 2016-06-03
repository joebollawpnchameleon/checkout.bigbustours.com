using Autofac;
using Autofac.Integration.Web;
using bigbus.checkout.data.Model;
using bigbus.checkout.data.PlainQueries;
using bigbus.checkout.data.Repositories.Infrastructure;
using Services.Infrastructure;
using System.Web;

namespace Services.Implementation
{
    public class BaseService
    {
        public IGenericDataRepository<OrderLineGeneratedBarcode> OrderLineGeneratedBCRepository { get; set; }
        public IGenericDataRepository<OrderLine> OrderLineRepository { get; set; }
        public IGenericDataRepository<Order> OrderRepository { get; set; }
        public IGenericDataRepository<User> UserRepository { get; set; }
        public IGenericDataRepository<Ticket> TicketRepository { get; set; }
        public IGenericDataRepository<Currency> CurrencyRepository { get; set; }
        public IGenericDataRepository<TransactionAddressPaypal> AddressPpRepository { get; set; }
        public IGenericDataRepository<OrderLineGeneratedBarcode> BarcodeRepository { get; set; }
        public IGenericDataRepository<EmailTemplate> EmailTemplateRepository { get; set; }
        public IGenericDataRepository<MicrositeEmailTemplate> MicrositeEmailRepository { get; set; }
        public IGenericDataRepository<Email> EmailRepository { get; set; }
        public IGenericDataRepository<ContactData> ContactsRepository { get; set; }
 
        public ILocalizationService LocalizationService { get; set; }
        public ILoggerService LoggerService { get; set; }
        public AuthenticationService AuthenticationService { get; set; }
        public IBarcodeFunctions BarcodeDBFunctions { get; set; }

        public BaseService()
        {
            var cpa = (IContainerProviderAccessor)HttpContext.Current.ApplicationInstance;
            var cp = cpa.ContainerProvider;
            cp.RequestLifetime.InjectProperties(this);
        }

        public void Log(string message)
        {
            LoggerService.LogItem(message);
        }
    }
}
