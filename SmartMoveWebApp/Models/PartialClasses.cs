using SmartMoveWebApp.Models.Metadatas;
using System;
using System.ComponentModel.DataAnnotations;

namespace SmartMoveWebApp.Models
{
    [MetadataType(typeof(CustomerMetadata))]
    public partial class Customer
    {
    }

    [MetadataType(typeof(CustomerCreditCardMetadata))]
    public partial class CustomerCreditCard
    {
    }

    [MetadataType(typeof(CustomerRatingMetadata))]
    public partial class CustomerRating
    {
    }

    [MetadataType(typeof(OrderMetadata))]
    public partial class Order
    {
    }

    [MetadataType(typeof(OrderBidMetadata))]
    public partial class OrderBid
    {
    }

    [MetadataType(typeof(OrderImageMetadata))]
    public partial class OrderImage
    {
    }

    [MetadataType(typeof(OrderPaymentMetadata))]
    public partial class OrderPayment
    {
    }

    [MetadataType(typeof(TruckMetadata))]
    public partial class Truck
    {
    }

    [MetadataType(typeof(TruckOwnerMetadata))]
    public partial class TruckOwner
    {
    }

    [MetadataType(typeof(TruckOwnerCreditCardMetadata))]
    public partial class TruckOwnerCreditCard
    {
    }

    [MetadataType(typeof(TruckOwnerRatingMetadata))]
    public partial class TruckOwnerRating
    {
    }
}