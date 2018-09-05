using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SmartMoveWebApp.Models.Metadatas
{
    public class OrderPaymentMetadata
    {
        [Required]
        public int OrderId { get; set; }

        [Required]
        public int CustomerCCId { get; set; }

        [Required]
        public double PaymentAmout { get; set; }

        [Required]
        [StringLength(50)]
        public string PaymentType { get; set; }

        [Required]
        [StringLength(50)]
        public string PaymentStatus { get; set; }
    }
}