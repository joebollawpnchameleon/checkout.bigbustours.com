using Common.Model;
using Services.Infrastructure;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace bigbus.checkout.mvc.Models
{
    public class UserDetailsVM
    {     

        [LocalRequired("Pleaseenteryouremailaddress")]
        public string Email { get; set; }

        [LocalRequired("Title")]
        public string Title { get; set; }

        [LocalRequired("Pleaseenteryourfirstname")]
        public string Firstname { get; set; }

        [LocalRequired("Pleaseenteryourlastname")]
        public string Lastname { get; set; }

        [LocalRequired("Pleaseenteryouraddress")]
        public string AddressLine1 { get; set; }
                
        public string AddressLine2 { get; set; }

        [LocalRequired("Pleasenteryourtowncity")]
        public string City { get; set; }

        [LocalRequired("Pleaseenteryourpostzipcode")]
        public string PostCode { get; set; }

        public string StateProvince { get; set; }

        [LocalRequired("Booking_SelectCountryError")]
        [DisplayName("Country")]
        public string CountryId { get; set; }

        [DisplayName("We will send you a confirmation email Please include me in marketing emails")]
        public bool ReceiveNewsletter { get; set; }         

        public string PhoneNumber { get; set; }  

        [DisplayName("Expected Travel Date")]
        public string ExpectedTravelDate { get; set; }

        [DisplayName("IhavereadandagreetotheTermsandConditions")]
        public bool TermsAndCAccepted { get; set; }
    }
}