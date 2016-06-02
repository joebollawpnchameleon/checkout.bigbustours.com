
namespace Common.Enums
{
    public enum EcrResponseCodes
    {
        SiteNoQrSupport = 0,
        ApiDown = 1,
        ProductNotAvailable = 2,
        BookingFailure = 3,
        BookingSuccess = 4,
        QrCodeRetrievalFailure = 5
    }

    public class EcrResult
    {
        public EcrResponseCodes Status { get; set; }

        public string ErrorMessage { get; set; }
    }
}
