using System;
using System.Security.Cryptography.X509Certificates;
using bigbus.checkout.data.Model;
using bigbus.checkout.data.Repositories.Infrastructure;
using Common.Model;
using Services.Infrastructure;

namespace Services.Implementation
{
    public class UserService : IUserService
    {
        //inject logger to log error if anything happens
        private readonly IGenericDataRepository<User> _userRepository;

        public UserService(IGenericDataRepository<User> userRepository)
        {
            _userRepository = userRepository;
        }

        public Guid CreateUser(User user)
        {
            return new Guid();
        }

        public User CreateCustomer(Customer newCustomer)
        {
            var newUser = new User
            {
                Firstname = newCustomer.Firstname,
                Lastname = newCustomer.Lastname,
                Email = string.Concat(Guid.NewGuid(), "_", newCustomer.Email),
                FriendlyEmail = newCustomer.Email,
                ReceiveNewsletter = newCustomer.ReceiveNewsletter,
                LanguageId = newCustomer.LanguageId,
                CurrencyId = newCustomer.CurrencyId,
                MicroSiteId = newCustomer.MicroSiteId,
                AddressLine1 = newCustomer.AddressLine1,
                AddressLine2 = newCustomer.AddressLine2,
                City = newCustomer.City,
                PostCode = newCustomer.PostCode,
                CountryId = newCustomer.CountryId,
                StateProvince = newCustomer.StateProvince
            };

            _userRepository.Add(newUser);

            newCustomer.Id = newUser.Id;

            return newUser;
        }
    }
}
