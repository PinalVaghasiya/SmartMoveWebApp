using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SmartMoveWebApp.Dtos
{
    public class OrderBidDto
    {
        public int BidId { get; set; }

        [Required]
        public int OrderId { get; set; }

        [Required]
        public int TruckOwnerId { get; set; }

        [Required]
        public System.DateTime DeliveryStartTime { get; set; }

        public long Time { get; set; }

        [Required]
        public double NumberOfHours { get; set; }

        [Required]
        public int NumberOfTrips { get; set; }

        [Required]
        public double BidAmount { get; set; }

        [Required]
        [StringLength(50)]
        public string BidStatus { get; set; }
    }
}