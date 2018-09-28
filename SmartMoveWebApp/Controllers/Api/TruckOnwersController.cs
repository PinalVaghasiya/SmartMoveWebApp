using AutoMapper;
using SmartMoveWebApp.BusniessLogic;
using SmartMoveWebApp.Dtos;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SmartMoveWebApp.Controllers.Api
{
    [RoutePrefix("api/TruckOwner")]
    public class TruckOnwersController : ApiController
    {
        public SmartMoveEntities _context { get; set; }

        public TruckOnwersController()
        {
            _context = new SmartMoveEntities();
        }

        [HttpGet]
        public IEnumerable<TruckType> GetTruckTypes()
        {
            return _context.TruckTypes.ToList();
        }

        [HttpGet]
        [Route("GetNewOrders")]
        public IHttpActionResult GetNewOrders(int truckOwnerId)
        {
            var truckInDb = _context.Trucks.Single(t => t.TruckOwnerId == truckOwnerId);
            int truckTypeId = truckInDb.TruckTypeId;

            var pendingBids = _context.OrderBids
                .Where(o => o.TruckOwnerId == truckOwnerId)
                .Where(o => o.BidStatus == Constants.OrderBidStatus.PENDING.ToString())
                .Select(b => b.OrderId)
                .ToList();

            var orders = _context.Orders
                .Where(o => o.TruckTypeId <= truckTypeId)
                .Where(o => !pendingBids.Contains(o.OrderId))
                .Where(o => o.OrderStatus == Constants.OrderStatus.PENDING.ToString())
                .OrderBy(o => o.OrderDateTime)
                .ToList();

            var acceptedBids = _context.OrderBids
                .Where(o => o.TruckOwnerId == truckOwnerId)
                .Where(o => o.BidStatus == Constants.OrderBidStatus.ACCEPTED.ToString())
                .Select(b => b.OrderId)
                .ToList();

            var confirmedOrders = _context.Orders
                .Where(o => acceptedBids.Contains(o.OrderId))
                .Where(o => o.OrderStatus != Constants.OrderStatus.PENDING.ToString())
                .OrderBy(o => o.OrderDateTime)
                .ToList();

            foreach (var confirmedOrder in confirmedOrders)
            {
                TimeSpan time = TimeSpan.FromMilliseconds(Convert.ToDouble(confirmedOrder.Time));
                DateTime orderStartDate = new DateTime(1970, 1, 1) + time;
                TimeSpan difference = orderStartDate - DateTime.Now;
                if (confirmedOrder.OrderStatus == "CONFIRMED" && difference.Days == 0)
                    orders.Add(confirmedOrder);
                else if (confirmedOrder.OrderStatus != "CONFIRMED")
                    orders.Add(confirmedOrder);
            }

            return Ok(orders.Select(Mapper.Map<Order, OrderDto>));
        }

        [HttpGet]
        [Route("GetMyOrders")]
        public IHttpActionResult GetMyOrders(int truckOwnerId)
        {
            var orderBids = _context.OrderBids
                .Where(b => b.TruckOwnerId == truckOwnerId)
                .Select(b => b.OrderId)
                .ToList();

            var cancelledOrders = _context.Orders
                .Where(o => orderBids.Contains(o.OrderId))
                .Where(o => o.OrderStatus == Constants.OrderStatus.CANCELLED.ToString())
                .ToList();

            var completedOrders = _context.Orders
                .Where(o => orderBids.Contains(o.OrderId))
                .Where(o => o.OrderStatus == Constants.OrderStatus.COMPLETED.ToString())
                .ToList();

            var runningOrders = _context.Orders
                .Where(o => orderBids.Contains(o.OrderId))
                .Where(o => o.OrderStatus == Constants.OrderStatus.CONFIRMED.ToString())
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

        [HttpGet]
        [Route("GetMyBids")]
        public IHttpActionResult GetMyBids(int truckOwnerId)
        {
            var pendingBids = _context.OrderBids
                .Where(b => b.TruckOwnerId == truckOwnerId)
                .Where(b => b.BidStatus == Constants.OrderBidStatus.PENDING.ToString())
                .OrderByDescending(b => b.DeliveryStartTime)
                .ToList();

            var acceptedBids = _context.OrderBids
                .Where(b => b.TruckOwnerId == truckOwnerId)
                .Where(b => b.BidStatus != Constants.OrderBidStatus.PENDING.ToString())
                .OrderByDescending(b => b.DeliveryStartTime)
                .ToList();

            var getBidsListDto = new GetBidsListDto
            {
                PendingBids = pendingBids.Select(Mapper.Map<OrderBid, OrderBidDto>),
                AcceptedBids = acceptedBids.Select(Mapper.Map<OrderBid, OrderBidDto>)
            };

            return Ok(getBidsListDto);
        }

        [HttpPost]
        [Route("PlaceBid")]
        public IHttpActionResult PlaceBid(OrderBidDto orderBidDto)
        {
            var orderBid = Mapper.Map<OrderBidDto, OrderBid>(orderBidDto);

            orderBid.BidStatus = Constants.OrderBidStatus.PENDING.ToString();

            TimeSpan time = TimeSpan.FromMilliseconds(orderBidDto.Time);
            DateTime startDateTime = new DateTime(1970, 1, 1) + time;

            orderBid.DeliveryStartTime = startDateTime;
            orderBid.CreatedTime = DateTime.Now;

            _context.OrderBids.Add(orderBid);
            _context.SaveChanges();

            return Ok(Mapper.Map<OrderBid, OrderBidDto>(orderBid));
        }

        [HttpPost]
        [Route("RemoveMyBid")]
        public IHttpActionResult RemoveMyBid(int bidId)
        {
            var orderBid = _context.OrderBids
                .Single(b => b.BidId == bidId);

            orderBid.BidStatus = Constants.OrderBidStatus.CANCELLED.ToString();
            orderBid.ModifiedTime = DateTime.Now;

            _context.SaveChanges();

            return Ok(orderBid);
        }

        [HttpPost]
        [Route("StartTrip")]
        public IHttpActionResult StartTrip(int orderId)
        {
            var order = _context.Orders.Single(o => o.OrderId == orderId);

            order.OrderStatus = Constants.OrderStatus.DELIVERING.ToString();
            _context.SaveChanges();

            return Ok(Mapper.Map<Order, OrderDto>(order));
        }

        [HttpPost]
        [Route("EndTrip")]
        public IHttpActionResult EndTrip(int orderId)
        {
            var order = _context.Orders
                .Single(o => o.OrderId == orderId);
            order.OrderStatus = Constants.OrderStatus.COMPLETED.ToString();

            var orderBid = _context.OrderBids
                .Where(b => b.BidStatus == Constants.OrderBidStatus.ACCEPTED.ToString())
                .Single(b => b.OrderId == orderId);
            orderBid.BidStatus = Constants.OrderBidStatus.COMPLETED.ToString();
            orderBid.ModifiedTime = DateTime.Now;

            _context.SaveChanges();

            return Ok(Mapper.Map<Order, OrderDto>(order));
        }

        [HttpPost]
        [Route("RateCustomer")]
        public IHttpActionResult RateCustomer(CustomerRatingDto customerRatingDto)
        {
            var customerRating = new CustomerRating();

            customerRating = Mapper.Map<CustomerRatingDto, CustomerRating>(customerRatingDto);
            customerRating.CreatedTime = DateTime.Now;

            _context.CustomerRatings.Add(customerRating);
            _context.SaveChanges();

            return Ok();
        }

        [HttpGet]
        [Route("GetAverageDriverRating")]
        public IHttpActionResult GetAverageDriverRating(int truckOwnerId)
        {
            double averageRating = _context.TruckOwnerRatings.Where(r => r.TruckOwnerId == truckOwnerId).Average(r => r.Rating);
            return Ok(averageRating);
        }
    }
}
