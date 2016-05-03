
namespace Common.Model.PayPal
{
    public class PayPalReturnUserInfo
    {
        public string Payer_Id { get; set; }
        public string PayerStatus { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string CountryCode { get; set; }

        public PayPalAddressInfo AddressInfo { get; set; }
    }
}
