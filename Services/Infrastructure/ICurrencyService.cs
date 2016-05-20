using bigbus.checkout.data.Model;
using System;

namespace Services.Infrastructure
{
    public interface ICurrencyService
    {
        Guid GetCurrencyGuidByCode(string code);

        Currency GetCurrencyByCode(string code);

        string GetCurrencyIsoCodeById(string id);

        Currency GetCurrencyById(string id);
    }
}
