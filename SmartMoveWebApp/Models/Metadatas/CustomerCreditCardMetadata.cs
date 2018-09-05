using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SmartMoveWebApp.Models.Metadatas
{
    public class CustomerCreditCardMetadata
    {
        [Required]
        public int CustomerId { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Card Number")]
        public string CCNumber { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Name")]
        public string CCName { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Card Type")]
        public string CCType { get; set; }

        [Required]
        [Display(Name = "CRV Number")]
        public int CCCRVNumber { get; set; }

        [Required]
        [Display(Name = "Expiry Month")]
        public int CCExpiryMonth { get; set; }

        [Required]
        [Display(Name = "Expiry Year")]
        public int CCExpiryYear { get; set; }
    }
}