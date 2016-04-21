
using System;

namespace Common.Model.Pci
{
    public class Basket
    {
        public string ID { get; set; }
        public string IPAddress { get; set; }
        public bool IsAgent { get; set; }
        public string ISOCurrencyCode { get; set; }
        public bool IsFromUS { get; set; }
        public decimal Total { get; set; }
        public BasketLine[] Items { get; set; }
        public User User { get; set; }
        public int Status { get; set; }
    }

    public class User
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string AddressLine1 { get; set; }
        public string City { get; set; }
        public string PostCode { get; set; }
        public string CountryCode { get; set; }
        public string EmailAddress { get; set; }
        public string FullName { get; set; }
    }

    public class Payment
    {
        public string ID { get; set; }
        public int TransactionType { get; set; }
        public string GatewayReference { get; set; }
        public string GatewaySessionID { get; set; }
        public string GatewayName { get; set; }
        public string GatewayCookie { get; set; }
        public string GatewayEchoData { get; set; }
        public string MerchantAccountID { get; set; }
        public string AcquirerResponse { get; set; }
        public string CVCResponse { get; set; }
        public string AVSResponse { get; set; }
        public string ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
        public string IssuerUrl { get; set; }
        public string IssuerPaRequest { get; set; }
        public Persistedcard PersistedCard { get; set; }
        public Basket Basket { get; set; }
        public int Amount { get; set; }
        public Paymentcard PaymentCard { get; set; }
    }

    public class Persistedcard
    {
        public string ID { get; set; }
        public string CardNumber { get; set; }
        public string SecurityCode { get; set; }
        public int CardType { get; set; }
        public string ExpiryMM { get; set; }
        public string ExpiryYYYY { get; set; }
        public string CardHolder { get; set; }
    }

    public class Paymentcard
    {
        public int CardType { get; set; }
        public string ExpiryMM { get; set; }
        public string ExpiryYYYY { get; set; }
        public string CardHolder { get; set; }
        public string CardNumber { get; set; }
        public string SecurityCode { get; set; }
    }

    public class BasketLine
    {
        public string Name { get; set; }
        public DateTime? Date { get; set; }
        public int TicketQuantity { get; set; }
        public string TicketType { get; set; }
        public decimal Total { get; set; }
    }
}
