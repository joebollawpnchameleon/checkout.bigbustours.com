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
        
        public DbLoggerService(string sessionId, IGenericDataRepository<Log> logRepository)
        {
            _logRepository = logRepository;
            _sessionId = sessionId;
        }

        public void LogItem(string message)
        {
            _logRepository.Add(new bigbus.checkout.data.Model.Log
            {
                Message = message,
                CreatedOn = DateTime.Now,
                Logger = _sessionId
            });
        }
    }
}
