using System;
using bigbus.checkout.data.Model;
using Common.Model;

namespace Services.Infrastructure
{
    public interface IUserService
    {
        Guid CreateUser(User user);

        User CreateCustomer(Customer newCustomer);
    }
}
