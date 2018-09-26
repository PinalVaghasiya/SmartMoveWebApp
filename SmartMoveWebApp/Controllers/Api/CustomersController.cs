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
                .Where(o => o.OrderStatus == Constants.OrderStatus.CANCELLED.ToString())
                .ToList();

            var completedOrders = _context.Orders
                .Where(o => o.CustomerId == customerId)
                .Where(o => o.OrderStatus == Constants.OrderStatus.COMPLETED.ToString())
                .ToList();

            var runningOrders = _context.Orders
                .Where(o => o.CustomerId == customerId)
                .Where(o => o.OrderStatus != Constants.OrderStatus.CANCELLED.ToString())
                .Where(o => o.OrderStatus != Constants.OrderStatus.COMPLETED.ToString())
                .ToList();

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
            if (ModelState.IsValid)
            {
                TimeSpan time = TimeSpan.FromMilliseconds(orderDto.Time);
                DateTime orderDateTime = new DateTime(1970, 1, 1) + time;

                var order = new Order
                {
                    CustomerId = orderDto.CustomerId,
                    TruckTypeId = orderDto.TruckTypeId,
                    OrderDateTime = orderDateTime,
                    Time = orderDto.Time,
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
                    EstimatedNumOfTrips = String.IsNullOrEmpty(orderDto.EstimatedNumOfTrips.ToString()) ? orderDto.EstimatedNumOfTrips : 0,
                    EstimatedWeight = orderDto.EstimatedWeight,
                    EstimatedArea = orderDto.EstimatedArea,
                    CreatedTime = DateTime.Now,
                    OrderStatus = Constants.OrderStatus.PENDING.ToString()
                };

                var orderPayment = new OrderPayment
                {
                    OrderId = order.OrderId,
                    CustomerCCId = 4,
                    PaymentAmout = Constants.InitialOrderPaymentAmount,
                    PaymentType = "INITIAL",
                    PaymentStatus = "SUCCESS",
                    Timestamp = DateTime.Now
                };

                _context.Orders.Add(order);
                _context.OrderPayments.Add(orderPayment);

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
                orderBid.BidStatus = Constants.OrderBidStatus.CANCELLED.ToString();

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
                .Where(b => b.BidStatus != Constants.OrderBidStatus.CANCELLED.ToString())
                .Where(b => b.BidStatus != Constants.OrderBidStatus.COMPLETED.ToString())
                .Where(b => b.OrderId == orderId)
                .ToList();

            List<CustomerOrderBidDto> customerOrderBids = new List<CustomerOrderBidDto>();
            foreach (var orderBid in orderBidsList)
            {
                var customerOrderBid = new CustomerOrderBidDto();
                customerOrderBid = Mapper.Map<OrderBid, CustomerOrderBidDto>(orderBid);
                var truckOwner = _context.TruckOwners.Single(t => t.TruckOwnerId == orderBid.TruckOwnerId);
                customerOrderBid.DriverName = truckOwner.FirstName + " " + truckOwner.LastName;
                customerOrderBid.AverageRating = GetAverageDriverRating(orderBid.TruckOwnerId);
                customerOrderBids.Add(customerOrderBid);
            }
            return Ok(customerOrderBids);
        }

        [HttpPost]
        [Route("AcceptBid")]
        public IHttpActionResult AcceptBid(int bidId)
        {
            var orderBid = _context.OrderBids.Single(b => b.BidId == bidId);
            orderBid.BidStatus = Constants.OrderBidStatus.ACCEPTED.ToString();
            orderBid.ModifiedTime = DateTime.Now;

            var order = _context.Orders.Single(o => o.OrderId == orderBid.OrderId);
            order.OrderStatus = Constants.OrderStatus.CONFIRMED.ToString();

            if (orderBid.BidAmount > Constants.InitialOrderPaymentAmount)
            {
                var orderPayment = new OrderPayment
                {
                    OrderId = orderBid.OrderId,
                    CustomerCCId = 4,
                    PaymentAmout = orderBid.BidAmount - Constants.InitialOrderPaymentAmount,
                    PaymentType = "FINAL",
                    PaymentStatus = "SUCCESS",
                    Timestamp = DateTime.Now
                };
                _context.OrderPayments.Add(orderPayment);
            }

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

        public double GetAverageDriverRating(int truckOwnerId)
        {
            var hasRatings = _context.TruckOwnerRatings.Where(r => r.TruckOwnerId == truckOwnerId).ToList();

            if (hasRatings != null && hasRatings.Count > 0)
            {
                double averageRating = hasRatings.Average(r => r.Rating);
                return averageRating;
            }
            else
                return 0;
        }
    }
}