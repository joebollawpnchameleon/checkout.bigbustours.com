
namespace Common.Model
{
    public class OrderConfirmationEmailRequest
    {
        public string CityName { get; set; }
        public string OrderNumber{get;set;}
        public string ViewAndPrintTicket_Link{get;set;}
        public string UserFullName{get;set;}
        public string DateOfOrder{get;set;}
        public string OrderTotal{get;set;}
        public string TicketQuantity{get;set;}
        public string termsAndConditions_Link{get;set;}
        public string PrivacyPolicy_Link{get;set;}
        public string AppStore_Link { get;set;}
        public string GooglePlay_Link { get;set;}
        public string CityNumber{get;set;}
        public string CityEmail{get;set;}
    }
}
