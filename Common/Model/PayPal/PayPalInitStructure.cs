
namespace Common.Model.PayPal
{
    public class PayPalInitStructure
    {
        public bool InTestMode { get; set; }

        public string ApiUserName { get; set; }

        public string ApiPassword { get; set; }

        public string ApiSignature { get; set; }

        public string RealEndPoint { get; set; }

        public string ExpressCheckoutEndPoint { get; set; }
    }
}
