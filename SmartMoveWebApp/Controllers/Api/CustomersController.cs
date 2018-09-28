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
                .OrderBy(o => o.OrderDateTime)
                .ToList();

            List<string> statusList = new List<string>
            {
                Constants.OrderStatus.COMPLETED.ToString(),
                Constants.OrderStatus.CANCELLED.ToString()
            };

            var completedOrders = _context.Orders
                .Where(o => o.CustomerId == customerId)
                .Where(o => statusList.Contains(o.OrderStatus))
                .OrderBy(o => o.OrderDateTime)
                .ToList();
            List<CustomerOrderDto> completedOrderDtos = new List<CustomerOrderDto>();
            foreach (var order in completedOrders)
            {
                var customerOrder = new CustomerOrderDto();
                customerOrder = Mapper.Map<Order, CustomerOrderDto>(order);
                var hasCustomerRating = _context.TruckOwnerRatings.SingleOrDefault(t => t.OrderId == order.OrderId);
                if (hasCustomerRating != null)
                    customerOrder.HasCustomerRating = true;
                completedOrderDtos.Add(customerOrder);
            }

            var runningOrders = _context.Orders
                .Where(o => o.CustomerId == customerId)
                .Where(o => o.OrderStatus != Constants.OrderStatus.CANCELLED.ToString())
                .Where(o => o.OrderStatus != Constants.OrderStatus.COMPLETED.ToString())
                .OrderBy(o => o.OrderDateTime)
                .ToList();

            GetCustomerOrderListDto ordersList = new GetCustomerOrderListDto
            {
                RunningOrders = runningOrders.Select(Mapper.Map<Order, CustomerOrderDto>),
                CompletedOrders = completedOrderDtos,
                CancelledOrders = cancelledOrders.Select(Mapper.Map<Order, CustomerOrderDto>)
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
                .Where(b => b.OrderId == orderId)
                .OrderBy(o => o.BidAmount)
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
                if (orderBid.BidStatus == Constants.OrderBidStatus.COMPLETED.ToString())
                    return Ok(customerOrderBids);
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

            var notAcceptedOrderBids = _context.OrderBids
                .Where(b => b.OrderId == order.OrderId)
                .Where(b => b.BidId != bidId)
                .ToList();

            foreach (OrderBid notAcceptedOrderBid in notAcceptedOrderBids)
            {
                notAcceptedOrderBid.BidStatus = "NOT ACCEPTED";
            }

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
        public IHttpActionResult RateDriver(CustomerRatingDto driverRatingDto)
        {
            var orderBid = _context.OrderBids
                .Where(b => b.BidStatus == Constants.OrderBidStatus.COMPLETED.ToString())
                .Single(b => b.OrderId == driverRatingDto.OrderId);

            var truckOwnerRating = new TruckOwnerRating
            {
                OrderId = driverRatingDto.OrderId,
                TruckOwnerId = orderBid.TruckOwnerId,
                Rating = driverRatingDto.Rating,
                CreatedTime = DateTime.Now
            };
            _context.TruckOwnerRatings.Add(truckOwnerRating);
            _context.SaveChanges();

            return Ok();
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