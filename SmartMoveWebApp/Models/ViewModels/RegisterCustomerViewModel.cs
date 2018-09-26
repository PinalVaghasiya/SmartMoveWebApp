using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SmartMoveWebApp.Models.ViewModels
{
    public class RegisterCustomerViewModel
    {
        public int CustomerId { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [StringLength(50)]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 8)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "The Password and Confirm Password values do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        [StringLength(12)]
        [RegularExpression("\\d{3}-\\d{3}-\\d{4}", ErrorMessage = "Phone Number should be of 10 digits only")]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        [Required]
        [Display(Name = "Address Line 1")]
        public string Address1 { get; set; }

        [Display(Name = "Address Line 2")]
        public string Address2 { get; set; }

        [Required]
        [StringLength(10, ErrorMessage = "Zip Code should be of maximum 10 charcters.")]
        [DataType(DataType.PostalCode)]
        [RegularExpression("^\\d{5}(?:[-]\\d{4})?$", ErrorMessage = "Zip Code should be in a valid format.")]
        [Display(Name = "Zip Code")]
        public string ZipCode { get; set; }

        [Required]
        [StringLength(50)]
        public string City { get; set; }

        [Required]
        [StringLength(50)]
        public string State { get; set; }
    }
}