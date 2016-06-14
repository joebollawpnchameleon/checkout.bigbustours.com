
namespace Common.Model
{
    public class OrderConfirmationEmailRequest
    {
        public string ReceiverFirstname { get; set; }
        public string EmailSubject { get; set; }
        public string SenderEmail { get; set; }
        public string ReceiverEmail { get; set; }
        public string CityName { get; set; }
        public string LanguageId { get; set; }
        public string OrderNumber{get;set;}
        public string ViewAndPrintTicketLink{get;set;}
        public string UserFullName{get;set;}
        public string DateOfOrder{get;set;}
        public string OrderTotal{get;set;}
        public string TicketDetails { get; set; }
        public string TicketQuantity{get;set;}
        public string TermsAndConditionsLink{get;set;}
        public string PrivacyPolicyLink{get;set;}
        public string AppStoreLink { get;set;}
        public string GooglePlayLink { get;set;}
        public string ContactUsLink { get; set; }
        public string FaqLink { get; set; }
        public string DownloadMapLink { get; set; }
        public string TrustPilotLink { get; set; }
        public string TripAdvisorLink { get; set; }
        public string CityNumber{get;set;}
        public string CityEmail{get;set;}
        public string HtmlBody { get; set; }
    }
}
