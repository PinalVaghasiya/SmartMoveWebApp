using AutoMapper;
using SmartMoveWebApp.BusniessLogic;
using SmartMoveWebApp.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SmartMoveWebApp.Controllers.Api
{
    [RoutePrefix("api/Customer")]
    public class CustomersController : ApiController
    {
        public SmartMoveEntities _context { get; set; }

        public CustomersController()
        {
            _context = new SmartMoveEntities();
        }

        [HttpGet]
        [Route("GetOrders")]
        public IHttpActionResult GetOrders(int customerId)
        {
            var cancelledOrders = _context.Orders
                .Where(o => o.CustomerId == customerId)
                .Where(o => o.OrderStatus == "CANCELLED")
                .ToList();

            var completedOrders = _context.Orders
                .Where(o => o.CustomerId == customerId)
                .Where(o => o.OrderStatus == "COMPLETED")
                .ToList();

            var runningOrders = _context.Orders
                .Where(o => o.CustomerId == customerId)
                .Where(o => o.OrderStatus != "CANCELLED")
                .Where(o => o.OrderStatus != "COMPLETED")
                .ToList();

            var list = cancelledOrders.Select(Mapper.Map<Order, OrderDto>);

            GetOrderListDto ordersList = new GetOrderListDto
            {
                RunningOrders = runningOrders.Select(Mapper.Map<Order, OrderDto>),
                CompletedOrders = completedOrders.Select(Mapper.Map<Order, OrderDto>),
                CancelledOrders = cancelledOrders.Select(Mapper.Map<Order, OrderDto>)
            };

            return Ok(ordersList);
        }

        [HttpPost]
        [Route("CreateOrder")]
        public IHttpActionResult CreateOrder(OrderDto orderDto)
        {
            if(ModelState.IsValid)
            {
                var order = new Order
                {
                    CustomerId = orderDto.CustomerId,
                    TruckTypeId = orderDto.TruckTypeId,
                    OrderDateTime = orderDto.OrderDateTime,
                    PickupPlace = orderDto.PickupPlace,
                    PickupLat = orderDto.PickupLat,
                    PickupLong = orderDto.PickupLong,
                    PickupUnitNumber = orderDto.PickupUnitNumber,
                    PickupFloor = orderDto.PickupFloor,
                    PickupHasElevator = orderDto.PickupHasElevator,
                    PickupDistanceFromParking = orderDto.PickupDistanceFromParking,
                    PickupAdditionalInfo = orderDto.PickupAdditionalInfo,
                    DropPlace = orderDto.DropPlace,
                    DropLat = orderDto.DropLat,
                    DropLong = orderDto.DropLong,
                    DropUnitNumber = orderDto.DropUnitNumber,
                    DropFloor = orderDto.DropFloor,
                    DropHasElevator = orderDto.DropHasElevator,
                    DropDistanceFromParking = orderDto.DropDistanceFromParking,
                    DropAdditionalInfo = orderDto.DropAdditionalInfo,
                    EstimatedNumOfTrips = String.IsNullOrEmpty(orderDto.EstimatedNumOfTrips.ToString()) ?orderDto.EstimatedNumOfTrips : 0,
                    EstimatedWeight = orderDto.EstimatedWeight,
                    EstimatedArea = orderDto.EstimatedArea,
                    CreatedTime = DateTime.Now,
                    OrderStatus = Constants.OrderStatus.PENDING.ToString()
                };

                var orderPayment = new OrderPayment
                {
                    OrderId = order.OrderId,
                    CustomerCCId = 4,
                    PaymentAmout = 10,
                    PaymentType = "INITIAL",
                    PaymentStatus = "SUCCESS",
                    Timestamp = DateTime.Now
                };

                _context.OrderPayments.Add(orderPayment);

                _context.Orders.Add(order);
                _context.SaveChanges();

                orderDto.OrderId = order.OrderId;

                return Ok(orderDto);
            }
            return BadRequest(ModelState);
        }

        [HttpPost]
        [Route("CancelOrder")]
        public IHttpActionResult CancelOrder(int orderId)
        {
            var orderInDb = _context.Orders.Single(o => o.OrderId == orderId);

            orderInDb.OrderStatus = Constants.OrderStatus.CANCELLED.ToString();

            var orderBidsList = _context.OrderBids.Where(b => b.OrderId == orderId).ToList();

            var orderPaymentsList = _context.OrderPayments.Where(p => p.OrderId == orderId).ToList();

            foreach (OrderBid orderBid in orderBidsList)
                orderBid.BidStatus = Constants.OrderStatus.CANCELLED.ToString();

            foreach (OrderPayment orderPayment in orderPaymentsList)
                orderPayment.PaymentStatus = "REVERSED";

            _context.SaveChanges();

            return Ok(Mapper.Map<Order, OrderDto>(orderInDb));
        }

        [HttpGet]
        [Route("GetOrderBids")]
        public IHttpActionResult GetOrderBids(int orderId)
        {
            var orderBidsList = _context.OrderBids
                .Where(b => b.BidStatus != "DELETED")
                .Where(b => b.OrderId == orderId)
                .ToList();
            return Ok(orderBidsList.Select(Mapper.Map<OrderBid, OrderBidDto>));
        }

        [HttpPost]
        [Route("AcceptBid")]
        public IHttpActionResult AcceptBid(int bidId)
        {
            var orderBid = _context.OrderBids.Single(b => b.BidId == bidId);
            orderBid.BidStatus = "ACCEPTED";

            var order = _context.Orders.Single(o => o.OrderId == orderBid.OrderId);
            order.OrderStatus = Constants.OrderStatus.CONFIRMED.ToString();

            var orderPayment = new OrderPayment
            {
                OrderId = orderBid.OrderId,
                CustomerCCId = 4,
                PaymentAmout = orderBid.BidAmount - 10,
                PaymentType = "FINAL",
                PaymentStatus = "SUCCESS",
                Timestamp = DateTime.Now
            };

            _context.OrderPayments.Add(orderPayment);
            _context.SaveChanges();

            return Ok(Mapper.Map<OrderBid, OrderBidDto>(orderBid));
        }

        [HttpPost]
        [Route("RateDriver")]
        public IHttpActionResult RateDriver(TruckOwnerRatingDto driverRatingDto)
        {
            var truckOwnerRating = new TruckOwnerRating();

            truckOwnerRating = Mapper.Map<TruckOwnerRatingDto, TruckOwnerRating>(driverRatingDto);
            truckOwnerRating.CreatedTime = DateTime.Now;

            _context.SaveChanges();

            return Ok(true);
        }

        [HttpGet]
        [Route("GetAverageCustomerRating")]
        public IHttpActionResult GetAverageCustomerRating(int customerId)
        {
            double averageRating = _context.CustomerRatings.Where(r => r.CustomerId == customerId).Average(r => r.Rating);
            return Ok(averageRating);
        }
    }
}