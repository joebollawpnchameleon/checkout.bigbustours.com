
using Common.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace bigbus.checkout.ViewModels
{
    public class UserDetailVM
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Address1 { get; set; }

        public string Address2 { get; set; }

        [Required]
        public string Town { get; set; }

        [Required]
        public string PostCode { get; set; }

        [Required]
        public string State { get; set; }

        [Required]
        public string Country { get; set; }

        public bool IsTermsAndConditionsAccepted { get; set; }

    }
}