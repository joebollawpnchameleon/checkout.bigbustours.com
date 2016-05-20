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

        public virtual Guid GetCurrencyGuidByCode(string code)
        {
            var currency = _repository.GetSingle(x => x.ISOCode.Equals(code, StringComparison.CurrentCultureIgnoreCase));
            return currency == null ? Guid.Empty : currency.Id;
        }

        public virtual Currency GetCurrencyByCode(string code)
        {
            return _repository.GetSingle(x => x.ISOCode.Equals(code, StringComparison.CurrentCultureIgnoreCase));
        }

        public virtual string GetCurrencyIsoCodeById(string id)
        {
            var currency =
                _repository.GetSingle(x => x.Id.ToString().Equals(id.Trim(), StringComparison.CurrentCultureIgnoreCase));

            return currency == null ? string.Empty : currency.ISOCode;
        }

        public virtual Currency GetCurrencyById(string id)
        {
            return _repository.GetSingle(x => x.Id.ToString().Equals(id.Trim(), StringComparison.CurrentCultureIgnoreCase));
        }
    }
}
