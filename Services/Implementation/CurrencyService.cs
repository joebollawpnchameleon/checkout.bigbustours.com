using System;
using bigbus.checkout.data.Model;
using bigbus.checkout.data.Repositories.Infrastructure;
using Services.Infrastructure;

namespace Services.Implementation
{
    public class CurrencyService : ICurrencyService
    {
        private readonly IGenericDataRepository<Currency> _repository;
 
        public CurrencyService(IGenericDataRepository<Currency> repository)
        {
            _repository = repository;
        }

        public Guid GetCurrencyGuidByCode(string code)
        {
            var currency = _repository.GetSingle(x => x.ISOCode.Equals(code, StringComparison.CurrentCultureIgnoreCase));
            return currency == null ? Guid.Empty : currency.Id;
        }

        public string GetCurrencyIsoCodeById(string id)
        {
            var currency =
                _repository.GetSingle(x => x.Id.ToString().Equals(id.Trim(), StringComparison.CurrentCultureIgnoreCase));

            return currency == null ? string.Empty : currency.ISOCode;
        }
    }
}
