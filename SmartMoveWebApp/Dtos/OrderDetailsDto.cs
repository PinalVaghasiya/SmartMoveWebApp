using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartMoveWebApp.Dtos
{
    public class OrderDetailsDto
    {
        public int OrderId { get; set; }

        public int CustomerId { get; set; }

        public int TruckTypeId { get; set; }

        public System.DateTime OrderDateTime { get; set; }

        public long Time { get; set; }

        public string PickupPlace { get; set; }

        public double PickupLat { get; set; }

        public double PickupLong { get; set; }

        public string PickupUnitNumber { get; set; }

        public int PickupFloor { get; set; }

        public bool PickupHasElevator { get; set; }

        public string PickupDistanceFromParking { get; set; }

        public string PickupAdditionalInfo { get; set; }

        public string DropPlace { get; set; }

        public double DropLat { get; set; }

        public double DropLong { get; set; }

        public string DropUnitNumber { get; set; }

        public int DropFloor { get; set; }

        public bool DropHasElevator { get; set; }

        public string DropDistanceFromParking { get; set; }

        public string DropAdditionalInfo { get; set; }

        public int? EstimatedNumOfTrips { get; set; }

        public string EstimatedWeight { get; set; }

        public string EstimatedArea { get; set; }

        public string OrderStatus { get; set; }

        public double InitialPaymentAmount { get; set; }

        public string InitialPaymentStatus { get; set; }

        public long InitialPaymentDateTime { get; set; }

        public double FinalPaymentAmount { get; set; }

        public string FinalPaymentStatus { get; set; }

        public long FinalPaymentDateTime { get; set; }
    }
}