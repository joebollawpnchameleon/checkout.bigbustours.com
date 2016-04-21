using System;

namespace Common.Model
{
    public class Customer
    {
        public Guid Id { get; set; }
       
        public string Email { get; set; }
       
        public string Title { get; set; }
 
        public string Firstname { get; set; }

        public string Lastname { get; set; }

        public string AddressLine1 { get; set; }

        public string AddressLine2 { get; set; }

        public string City { get; set; }

        public string PostCode { get; set; }

        public string StateProvince { get; set; }

        public string CountryId { get; set; }

        public bool? ReceiveNewsletter { get; set; }

        public string LanguageId { get; set; }

        public Guid? CurrencyId { get; set; }

        public Guid? BasketId { get; set; }

        public bool Authorised { get; set; }

        public bool IsAgent { get; set; }

        public string PhoneNumber { get; set; }

        public string MicroSiteId { get; set; }

        public string FriendlyEmail { get; set; }

        public bool W9OnFile { get; set; }

        public bool MobileTicketAgent { get; set; }

        public bool IsEcr { get; set; }

    }
}
