using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SmartMoveWebApp.Dtos
{
    public class TruckOwnerDto
    {
        [Required]
        public string TruckOwnerId { get; set; }

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
        [StringLength(50)]
        [MinLength(10)]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        [Required]
        [Display(Name = "Address Line 1")]
        public string Address1 { get; set; }

        public string Address2 { get; set; }

        [Required]
        [StringLength(50)]
        [DataType(DataType.PostalCode)]
        [Display(Name = "Zip Code")]
        public string ZipCode { get; set; }

        [Required]
        [StringLength(50)]
        public string City { get; set; }

        [Required]
        [StringLength(50)]
        public string State { get; set; }

        public ICollection<Truck> Trucks { get; set; }
    }
}