
using Common.Model.PayPal;

namespace Services.Infrastructure
{
    public interface IPaypalService
    {
        PayPalReturn ShortcutExpressCheckout(string returnUrl, string cancelUrl, string pageStyle, PayPalOrder order, bool commit);
        PayPalReturn ShortcutExpressCheckout(string returnUrl, string cancelUrl, string pageStyle, PayPalOrder order, bool commit, string solutionType);
        PayPalReturn ConfirmCheckoutDetails(string token);
        PayPalReturn ConfirmPayment(string finalPaymentAmount, string currencyCode, string token, string payerId);
        void SetUserSessionId(string sessionId);
    }
}
