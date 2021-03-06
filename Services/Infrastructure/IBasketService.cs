﻿using System;
using bigbus.checkout.data.Model;
using Common.Model;
using Common.Model.PayPal;

namespace Services.Infrastructure
{
    public interface IBasketService
    {
        Basket GetBasket(Guid basketId);

        bool IsBornBasketValid(BornBasket brnBasket);

        Guid PersistBasket(BornBasket basket);
        
        bool DoesBasketExist(string externalSessionId);

        Basket GetBasketBySessionId(string externalSessionId);

        Basket GetBasketById(string basketId);

        bool ConnectUserToBasket(Guid userId, Guid basketId);

        Common.Model.Pci.Basket GetPciBasket(Customer customer, Basket basket);

        PayPalOrder BuildPayPalOrder(Basket basket);

        Basket GetLatestBasket();

        void DeleteBasket(Basket basket);
    }
}
