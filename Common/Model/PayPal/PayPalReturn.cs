
namespace Common.Model.PayPal
{
    public class PayPalReturn
    {
        public bool IsError { get; set; }
        public string ErrorMessage { get; set; }
        public string ErrorCode { get; set; }
        public string RedirectURL { get; set; }
        public string Token { get; set; }
        //public string Payer_Id { get; set; }
        public string AUK { get; set; }
        public string Transaction_Id { get; set; }
        public PayPalReturnUserInfo PayPalReturnUserInfo { get; set; }

        public PayPalReturn()
        {
            IsError = false;
        }
    }
}
