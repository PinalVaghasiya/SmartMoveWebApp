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
        public int TruckOwnerId { get; set; }

        [Required]
        [StringLength(50)]
        public string Email { get; set; }

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        [Required]
        [StringLength(12)]
        public string Phone { get; set; }

        [Required]
        public string Address1 { get; set; }

        public string Address2 { get; set; }

        [Required]
        [StringLength(50)]
        public string ZipCode { get; set; }

        [Required]
        [StringLength(50)]
        public string City { get; set; }

        [Required]
        [StringLength(50)]
        public string State { get; set; }

        [Required(ErrorMessage = "Truck Type field is required.")]
        public int TruckTypeId { get; set; }

        [Required]
        [StringLength(50)]
        public string LicensePlate { get; set; }

        [Required]
        [StringLength(50)]
        public string TruckColor { get; set; }

        [Required]
        [StringLength(50)]
        public string TruckMake { get; set; }

        [Required]
        [StringLength(50)]
        public string TruckModel { get; set; }

        [Required]
        public int TruckYear { get; set; }

        [Required]
        [StringLength(50)]
        public string DriverLicenseNumber { get; set; }

        [Required]
        [StringLength(50)]
        public string VehicleRegNumber { get; set; }

        [Required]
        [StringLength(50)]
        public string DriverInsurancePolicy { get; set; }

        public string ProfilePictureURL { get; set; }

        public double AverageRating { get; set; }
    }
}