using System;

namespace Services.Infrastructure
{
    public interface ICurrencyService
    {
        Guid GetCurrencyGuidByCode(string code);

        string GetCurrencyIsoCodeById(string id);
    }
}
