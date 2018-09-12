using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SmartMoveWebApp.Dtos
{
    public class TruckOwnerCreditCardMetadata
    {
        [Required]
        public int TruckOwnerId { get; set; }

        [Required]
        [StringLength(50)]
        public string CCNumber { get; set; }

        [Required]
        [StringLength(100)]
        public string CCName { get; set; }

        [Required]
        [StringLength(50)]
        public string CCType { get; set; }

        [Required]
        public int CCCRVNumber { get; set; }

        [Required]
        public int CCExpiryMonth { get; set; }

        [Required]
        public int CCExpiryYear { get; set; }
    }
}