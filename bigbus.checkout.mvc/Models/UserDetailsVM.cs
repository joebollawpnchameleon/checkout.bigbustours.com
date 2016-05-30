using System.ComponentModel.DataAnnotations;

namespace bigbus.checkout.mvc.Models
{
    public class UserDetailsVM
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Firstname { get; set; }

        [Required]
        public string Lastname { get; set; }

        [Required]
        public string AddressLine1 { get; set; }
                
        public string AddressLine2 { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string PostCode { get; set; }

        public string StateProvince { get; set; }

        [Required]
        public string CountryId { get; set; }

        public bool ReceiveNewsletter { get; set; }         

        public string PhoneNumber { get; set; }  

        public string ExpectedTravelDate { get; set; }
    }
}