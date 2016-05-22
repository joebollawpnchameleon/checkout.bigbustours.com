using bigbus.checkout.data.Model;
using Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Infrastructure
{
    public interface INotificationService
    {
        bool CreateHtmlEmail();

        string CreateOrderConfirmationEmail(OrderConfirmationEmailRequest request);
    }


}
