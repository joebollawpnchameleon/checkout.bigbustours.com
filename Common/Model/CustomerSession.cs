
using System;

namespace Common.Model
{
    public class CustomerSession
    {
        public Guid DbId { get; set; }

        public Guid CurrencyId { get; set; }

        public Guid BasketId { get; set; }

        public string CookieDomain { get; set; }

        public string CookieName { get; set; }
    }
}
