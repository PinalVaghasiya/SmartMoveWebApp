//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SmartMoveWebApp
{
    using System;
    using System.Collections.Generic;
    
    public partial class OrderBid
    {
        public int BidId { get; set; }
        public int OrderId { get; set; }
        public int TruckOwnerId { get; set; }
        public System.DateTime DeliveryStartTime { get; set; }
        public Nullable<long> Time { get; set; }
        public double NumberOfHours { get; set; }
        public int NumberOfTrips { get; set; }
        public double BidAmount { get; set; }
        public string BidStatus { get; set; }
        public Nullable<System.DateTime> CreatedTime { get; set; }
        public Nullable<System.DateTime> ModifiedTime { get; set; }
    
        public virtual Order Order { get; set; }
        public virtual TruckOwner TruckOwner { get; set; }
    }
}
