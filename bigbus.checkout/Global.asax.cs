using System;
using System.Configuration;
using System.Web;
using System.Web.Routing;
using System.Web.UI;
using Autofac;
using Autofac.Integration.Web;
using bigbus.checkout.App_Start;
using bigbus.checkout.data.Model;
using bigbus.checkout.data.Repositories.Implementation;
using bigbus.checkout.data.Repositories.Infrastructure;
using bigbus.checkout.Helpers;
using bigbus.checkout.Models;
using Common.Model;
using Common.Model.Interfaces;
using Services.Implementation;
using Services.Infrastructure;
using System.Web.Mvc;

namespace bigbus.checkout
{
    public class Global : HttpApplication, IContainerProviderAccessor
    {
        static IContainerProvider _containerProvider;
        
        // Instance property that will be used by Autofac HttpModules
        // to resolve and inject dependencies.
        public IContainerProvider ContainerProvider
        {
            get { return _containerProvider; }
        }

        public void Application_Start(object sender, EventArgs e)
        {
            ScriptManager.ScriptResourceMapping.AddDefinition("jquery",
            new ScriptResourceDefinition
            {
                Path = "~/scripts/jquery-1.7.2.min.js",
                DebugPath = "~/scripts/jquery-1.7.2.min.js",
                CdnPath = "http://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.4.1.min.js",
                CdnDebugPath = "http://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.4.1.js"
            });
            
            WebApiConfig.RegisterRoutes(RouteTable.Routes);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            var builder = new ContainerBuilder();
            

            var fullUrl = ConfigurationManager.AppSettings["BaseUrl"] + ConfigurationManager.AppSettings["BasketApiUrl"];
            var picEndPoint = ConfigurationManager.AppSettings["PciWebsite.ApiDomain"];
            var defaultLanguage = ConfigurationManager.AppSettings["Default.Language"];

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

            builder.RegisterType<TranslationService>().As<ITranslationService>();
            builder.RegisterType<CheckoutService>().As<ICheckoutService>();
            builder.RegisterType<GenericHttpCacheProvider>().As<ICacheProvider>();
            builder.RegisterType<ImageService>().As<IImageService>();
            builder.RegisterType<ImageDbService>().As<IImageDbService>();

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
                    c.Resolve<ISiteService>()
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
               CheckoutService(
                   c.Resolve<IGenericDataRepository<Order>>(), 
                   c.Resolve<IGenericDataRepository<User>>(),
                   c.Resolve<IGenericDataRepository<Ticket>>(),
                   c.Resolve<IGenericDataRepository<Currency>>()
               )
           ).As<ICheckoutService>();

            builder.Register(c => new DBLoggerService(
                c.Resolve<IGenericDataRepository<Log>>()
                )).As<ILoggerService>();

    
            builder.Register(c => new TranslationService(
                c.Resolve<IGenericDataRepository<Language>>(),
                c.Resolve<IGenericDataRepository<Phrase>>(),
                c.Resolve<IGenericDataRepository<PhraseLanguage>>(),
                defaultLanguage
                )).As<ITranslationService>();

            // provider up with your registrations.
            _containerProvider = new ContainerProvider(builder.Build());
           
        }

    }
}