using System;
using bigbus.checkout.data.Model;
using Common.Model;

namespace Services.Infrastructure
{
    public interface IBasketService
    {
        Basket GetBasket(Guid basketId);

        bool IsBornBasketValid(BornBasket brnBasket);

        Guid PersistBasket(BornBasket basket);
        
        bool DoesBasketExist(string externalSessionId);

        Basket GetBasketBySessionId(string externalSessionId);

        bool ConnectUserToBasket(Guid userId, Guid basketId);

        Common.Model.Pci.Basket GetPciBasket(Customer customer, Basket basket);
    }
}
