using bigbus.checkout.data.Model;
using bigbus.checkout.data.Repositories.Infrastructure;
using Services.Infrastructure;
using System;

namespace Services.Implementation
{
    public class DbLoggerService : ILoggerService
    {
        private readonly string _sessionId;
        private readonly IGenericDataRepository<Log> _logRepository;
        private readonly IGenericDataRepository<BornBasketDump> _bornLogRepository;
 
        public DbLoggerService(string sessionId, IGenericDataRepository<Log> logRepository, IGenericDataRepository<BornBasketDump> bornLogRepository)
        {
            _logRepository = logRepository;
            _bornLogRepository = bornLogRepository;
            _sessionId = sessionId;
        }

        public void LogItem(string message)
        {
            _logRepository.Add(new Log
            {
                Message = message,
                CreatedOn = DateTime.Now,
                Logger = _sessionId
            });
        }

        public void LogItem(string message, string externalSessionId)
        {
            _logRepository.Add(new Log
            {
                Message = message,
                CreatedOn = DateTime.Now,
                Logger = externalSessionId
            });
        }

        public void LogBornBasket(string json, string externalCookieValue)
        {
            var existingBasket =
                _bornLogRepository.GetSingle(
                    x =>
                        !string.IsNullOrEmpty(x.ExternalCookieValue) &&
                        x.ExternalCookieValue.Equals(externalCookieValue, StringComparison.CurrentCultureIgnoreCase));

            if (existingBasket == null)
            {
                _bornLogRepository.Add(new BornBasketDump
                {
                    DateCreated = DateTime.Now,
                    BasketJsonDump = json,
                    ExternalCookieValue = externalCookieValue
                });
            }
            else
            {
                existingBasket.BasketJsonDump = json;
                _bornLogRepository.Update(existingBasket);
            }
        }
    }
}
