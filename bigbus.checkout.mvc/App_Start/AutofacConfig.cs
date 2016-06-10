using Autofac;
using Autofac.Integration.Mvc;
using System.Configuration;
using System.Web.Mvc;
using System;
using Services.Implementation;
using Common.Model.PayPal;
using Services.Infrastructure;
using Common.Model;
using Common.Model.Interfaces;
using bigbus.checkout.data.Repositories.Infrastructure;
using bigbus.checkout.data.Model;
using bigbus.checkout.data.Repositories.Implementation;
using bigbus.checkout.mvc.Helpers;

namespace bigbus.checkout.mvc.App_Start
{
    public class AutofacConfig
    {
        public static void ConfigureContainer()
        {
            var builder = new ContainerBuilder();

            // Register dependencies in controllers
            builder.RegisterControllers(typeof(MvcApplication).Assembly).PropertiesAutowired(); 
                   
            // Register dependencies in filter attributes
            builder.RegisterFilterProvider();

            // Register dependencies in custom views
            builder.RegisterSource(new ViewRegistrationSource());

            BuildDependecies(builder);

            var container = builder.Build();

            // Set MVC DI resolver to use our Autofac container
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }

        private static void BuildDependecies(ContainerBuilder builder)
        {
            var fullUrl = ConfigurationManager.AppSettings["BaseUrl"] + ConfigurationManager.AppSettings["BasketApiUrl"];
            var picEndPoint = ConfigurationManager.AppSettings["PciWebsite.ApiDomain"];
            var defaultLanguage = ConfigurationManager.AppSettings["Default.Language"];
            var sessionCookieName = ConfigurationManager.AppSettings["Session.CookieName"];
            var sessionId = AuthenticationService.GetCookieValue(sessionCookieName);
            var ecrApiKey = ConfigurationManager.AppSettings["EcrApiKey"];
            var ecrAgentCode = ConfigurationManager.AppSettings["EcrAgentCode"];
            var ecrAgentUiId = ConfigurationManager.AppSettings["EcrAgentUiId"];
            var environmentId = ConfigurationManager.AppSettings["Environment"];
            var liveEcrPoint = ConfigurationManager.AppSettings["LiveEcrEndPoint"];

            var paypalInitStruct = new PayPalInitStructure
            {
                ApiUserName = ConfigurationManager.AppSettings["PayPal.Username"],
                ApiPassword = ConfigurationManager.AppSettings["PayPal.Password"],
                ApiSignature = ConfigurationManager.AppSettings["PayPal.Signature"],
                ExpressCheckoutEndPoint = ConfigurationManager.AppSettings["PayPal.ExpressCheckoutEndPoint"],
                InTestMode = Convert.ToBoolean(ConfigurationManager.AppSettings["PayPal.InTestMode"]),
                RealEndPoint = ConfigurationManager.AppSettings["PayPal.RealEndPoint"]
            };

            builder.RegisterType<ApiConnectorService>().As<IApiConnectorService>()
                .WithParameter("fullUrl", fullUrl);

            builder.RegisterType<PciApiServiceNoASync>().As<IPciApiServiceNoASync>()
                .WithParameter("pciEndPoint", picEndPoint);

            builder.RegisterType<GenericDataRepository<Basket>>().As<IGenericDataRepository<Basket>>();
            builder.RegisterType<GenericDataRepository<BasketLine>>().As<IGenericDataRepository<BasketLine>>();
            builder.RegisterType<GenericDataRepository<Currency>>().As<IGenericDataRepository<Currency>>();
            builder.RegisterType<GenericDataRepository<Ticket>>().As<IGenericDataRepository<Ticket>>();
            builder.RegisterType<GenericDataRepository<Country>>().As<IGenericDataRepository<Country>>();
            builder.RegisterType<GenericDataRepository<Session>>().As<IGenericDataRepository<Session>>();
            builder.RegisterType<GenericDataRepository<User>>().As<IGenericDataRepository<User>>();
            builder.RegisterType<GenericDataRepository<MicroSite>>().As<IGenericDataRepository<MicroSite>>();
            builder.RegisterType<GenericDataRepository<Language>>().As<IGenericDataRepository<Language>>();
            builder.RegisterType<GenericDataRepository<MicroSiteLanguage>>().As<IGenericDataRepository<MicroSiteLanguage>>();
            builder.RegisterType<GenericDataRepository<Order>>().As<IGenericDataRepository<Order>>();
            builder.RegisterType<GenericDataRepository<Image>>().As<IGenericDataRepository<Image>>();
            builder.RegisterType<GenericDataRepository<ImageFolder>>().As<IGenericDataRepository<ImageFolder>>();
            builder.RegisterType<GenericDataRepository<ImageMetaData>>().As<IGenericDataRepository<ImageMetaData>>();
            builder.RegisterType<GenericDataRepository<Log>>().As<IGenericDataRepository<Log>>();
            builder.RegisterType<GenericDataRepository<Phrase>>().As<IGenericDataRepository<Phrase>>();
            builder.RegisterType<GenericDataRepository<PhraseLanguage>>().As<IGenericDataRepository<PhraseLanguage>>();
            builder.RegisterType<GenericDataRepository<TransactionAddressPaypal>>().As<IGenericDataRepository<TransactionAddressPaypal>>();
            builder.RegisterType<GenericDataRepository<OrderLine>>().As<IGenericDataRepository<OrderLine>>();
            builder.RegisterType<GenericDataRepository<OrderLineGeneratedBarcode>>().As<IGenericDataRepository<OrderLineGeneratedBarcode>>();
            builder.RegisterType<GenericDataRepository<EcrOrderLineBarcode>>().As<IGenericDataRepository<EcrOrderLineBarcode>>();
            builder.RegisterType<GenericDataRepository<ContactData>>().As<IGenericDataRepository<ContactData>>();
            builder.RegisterType<GenericDataRepository<MicrositeEmailTemplate>>().As<IGenericDataRepository<MicrositeEmailTemplate>>();
            builder.RegisterType<GenericDataRepository<EmailTemplate>>().As<IGenericDataRepository<EmailTemplate>>();
            builder.RegisterType<GenericDataRepository<Email>>().As<IGenericDataRepository<Email>>();

            builder.RegisterType<TranslationService>().As<ITranslationService>();
            builder.RegisterType<CheckoutService>().As<ICheckoutService>();
            builder.RegisterType<GenericHttpCacheProvider>().As<ICacheProvider>();
            builder.RegisterType<ImageService>().As<IImageService>();
            builder.RegisterType<ImageDbService>().As<IImageDbService>();
            builder.RegisterType<LocalizationService>().As<ILocalizationService>();
            builder.RegisterType<PdfClientRenderer>().As<IClientRenderService>();
            builder.RegisterType<NotificationService>().As<INotificationService>();

            if (environmentId.Equals(Common.Enums.Environment.Live.ToString()))
            { 
                builder.Register(c => new
                 EcrService(ecrApiKey, liveEcrPoint, ecrAgentCode, ecrAgentUiId))
                 .As<IEcrService>();
            }
            else
            {
                builder.Register(c => new
                 EcrService(ecrApiKey, ecrAgentCode, ecrAgentUiId))
                 .As<IEcrService>();
            }

            builder.Register(c => new
               CountryService(
                   c.Resolve<IGenericDataRepository<Country>>(),
                   c.Resolve<ITranslationService>()
                   )
           ).As<ICountryService>();

            builder.Register(c => new
                CurrencyService(
                    c.Resolve<IGenericDataRepository<Currency>>()
                    )
            ).As<ICurrencyService>();

            builder.Register(c => new
                TicketService(
                    c.Resolve<IGenericDataRepository<Ticket>>()
                    )
            ).As<ITicketService>();

            builder.Register(c => new
                BasketService(
                    c.Resolve<IGenericDataRepository<Basket>>(),
                    c.Resolve<IGenericDataRepository<BasketLine>>(),
                    c.Resolve<ICurrencyService>(),
                    c.Resolve<ITicketService>(),
                    c.Resolve<ISiteService>(),
                    c.Resolve<ITranslationService>()
                )
            ).As<IBasketService>();

            builder.Register(c => new
                 UserService(
                     c.Resolve<IGenericDataRepository<User>>()
                     )
             ).As<IUserService>();

            builder.Register(c => new
               SiteService(
                   c.Resolve<IGenericDataRepository<MicroSite>>()
                   )
           ).As<ISiteService>();

            builder.Register(c => new
              AuthenticationService(
                  c.Resolve<IGenericDataRepository<Session>>()
                  )
          ).As<IAuthenticationService>();

            builder.Register(c => new
                CheckoutService()
            ).As<ICheckoutService>();

            builder.Register(c => new DbLoggerService(sessionId, c.Resolve<IGenericDataRepository<Log>>(), c.Resolve<IGenericDataRepository<BornBasketDump>>()))
               .As<ILoggerService>();

            builder.Register(c => new TranslationService(
                c.Resolve<IGenericDataRepository<Language>>(),
                c.Resolve<IGenericDataRepository<Phrase>>(),
                c.Resolve<IGenericDataRepository<PhraseLanguage>>(),
                defaultLanguage
                )).As<ITranslationService>();

            builder.Register(c => new PayPalService(paypalInitStruct,
                   c.Resolve<ILoggerService>()
                   )).As<IPaypalService>();
        }
    }
}