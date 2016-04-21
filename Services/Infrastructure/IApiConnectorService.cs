
using Common.Model;
using Common.Model.Pci;

namespace Services.Infrastructure
{
    public interface IApiConnectorService
    {
        BornBasket GetExternalBasketByCookie(string cookieValue);
      
    }
}
