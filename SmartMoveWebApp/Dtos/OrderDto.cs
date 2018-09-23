using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SmartMoveWebApp.Dtos
{
    public class OrderDto
    {
        public int OrderId { get; set; }

        [Required]
        public int CustomerId { get; set; }

        [Required]
        public int TruckTypeId { get; set; }

        public System.DateTime OrderDateTime { get; set; }

        public long date { get; set; }

        public long time { get; set; }

        [Required]
        public string PickupPlace { get; set; }

        [Required]
        public double PickupLat { get; set; }

        [Required]
        public double PickupLong { get; set; }

        [StringLength(50)]
        public string PickupUnitNumber { get; set; }

        [Required]
        public int PickupFloor { get; set; }

        [Required]
        public bool PickupHasElevator { get; set; }

        [Required]
        [StringLength(50)]
        public string PickupDistanceFromParking { get; set; }

        public string PickupAdditionalInfo { get; set; }

        [Required]
        public string DropPlace { get; set; }

        [Required]
        public double DropLat { get; set; }

        [Required]
        public double DropLong { get; set; }

        [StringLength(50)]
        public string DropUnitNumber { get; set; }

        [Required]
        public int DropFloor { get; set; }

        [Required]
        public bool DropHasElevator { get; set; }

        [Required]
        [StringLength(50)]
        public string DropDistanceFromParking { get; set; }

        public string DropAdditionalInfo { get; set; }

        public int? EstimatedNumOfTrips { get; set; }

        [StringLength(50)]
        public string EstimatedWeight { get; set; }

        [StringLength(50)]
        public string EstimatedArea { get; set; }

        [Required]
        [StringLength(50)]
        public string OrderStatus { get; set; }
    }
}