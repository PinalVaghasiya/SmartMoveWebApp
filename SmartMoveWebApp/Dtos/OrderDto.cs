using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SmartMoveWebApp.Dtos
{
    public class OrderDto
    {
        [Required]
        public int CustomerId { get; set; }

        [Required]
        public int TruckTypeId { get; set; }

        [Required]
        public double PickupLat { get; set; }

        [Required]
        public double PickupLong { get; set; }

        [StringLength(50)]
        public string PickupUnitNumber { get; set; }

        [StringLength(50)]
        public string PickupDistanceFromParking { get; set; }

        [Required]
        public double DropLat { get; set; }

        [Required]
        public double DropLong { get; set; }

        [StringLength(50)]
        public string DropUnitNumber { get; set; }

        [StringLength(50)]
        public string DropDistanceFromParking { get; set; }

        [StringLength(50)]
        public string EstimatedWeight { get; set; }

        [StringLength(50)]
        public string EstimatedArea { get; set; }
    }
}