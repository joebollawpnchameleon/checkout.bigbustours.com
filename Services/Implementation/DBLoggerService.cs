using bigbus.checkout.data.Model;
using bigbus.checkout.data.Repositories.Infrastructure;
using Services.Infrastructure;
using System;

namespace Services.Implementation
{
    public class DbLoggerService : ILoggerService
    {
        private readonly IGenericDataRepository<Log> _logRepository;

        public DbLoggerService(IGenericDataRepository<Log> logRepository)
        {
            _logRepository = logRepository;  
        }

        public void Log(string message, string loggerId)
        {
            _logRepository.Add(new bigbus.checkout.data.Model.Log
            {
                Message = message,
                CreatedOn = DateTime.Now,
                Logger = loggerId
            });
        }
    }
}
