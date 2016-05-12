

using System.Data.Entity;
using bigbus.checkout.data.Model;

namespace bigbus.checkout.data
{
    public class CheckoutDbContext : DbContext
    {
        public CheckoutDbContext()
            : base("name=BigBusDataModelConn")
        {
            //Database.SetInitializer(new CheckoutContextCustomInitializer());
            Configuration.LazyLoadingEnabled = false;
            Configuration.ProxyCreationEnabled = false;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Basket>()
               .Property(e => e.DiscountValue)
               .HasPrecision(18, 0);

        }

        #region Models Declaration 

        public virtual DbSet<EcrOrderLineBarcode> EcrOrderLineBarcodes { get; set; }
        public virtual DbSet<OrderLineGeneratedBarcode> OrderLineBarcodes { get; set; }
        public virtual DbSet<TransactionAddressPaypal> AddressPaypals { get; set; }
        public virtual DbSet<Phrase> Phrases { get; set; }
        public virtual DbSet<PhraseLanguage> PhraseLanguages { get; set; }
        public virtual DbSet<Log> Logs { get; set; }
        public virtual DbSet<MicroSite> MicroSites { get; set; }
        public virtual DbSet<Language> Languages { get; set; }
        public virtual DbSet<MicroSiteLanguage> MicroSiteLanguages { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Session> Sessions { get; set; }
        public virtual DbSet<Country> Countries { get; set; }
        public virtual DbSet<Ticket> Tickets { get; set; }
        public virtual DbSet<Currency> Currencies { get; set; }
        public virtual DbSet<Basket> Baskets { get; set; }
        public virtual DbSet<BasketLine> BasketLines { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderLine> OrderLines { get; set; }
        public virtual DbSet<Image> Images { get; set; }
        public virtual DbSet<ImageMetaData> ImageMetaDatas { get; set; }
        public virtual DbSet<ImageFolder>  ImageFolders{ get; set; }

        #endregion
    }
}
