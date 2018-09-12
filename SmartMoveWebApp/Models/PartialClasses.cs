using SmartMoveWebApp.Dtos;
using System;
using System.ComponentModel.DataAnnotations;

namespace SmartMoveWebApp.Models
{
    [MetadataType(typeof(CustomerDto))]
    public partial class Customer
    {
    }

    [MetadataType(typeof(CustomerCreditCardDto))]
    public partial class CustomerCreditCard
    {
    }

    [MetadataType(typeof(CustomerRatingDto))]
    public partial class CustomerRating
    {
    }

    [MetadataType(typeof(OrderMetadata))]
    public partial class Order
    {
    }

    [MetadataType(typeof(OrderBidDto))]
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