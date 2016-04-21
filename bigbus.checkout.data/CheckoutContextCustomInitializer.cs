using System.Data.Entity;

namespace bigbus.checkout.data
{
    public class CheckoutContextCustomInitializer : IDatabaseInitializer<CheckoutDbContext>
    {
        public void InitializeDatabase(CheckoutDbContext context)
        {
            //if (context.Database.Exists())
            //{
            //    if (!context.Database.CompatibleWithModel(true))
            //    {
            //        context.Database.Delete();
            //        context.Database.Create();
            //    }
            //}
            //else
            //{
            //    context.Database.Create();
            //}
            //context.SaveChanges();
        }
    }
}
