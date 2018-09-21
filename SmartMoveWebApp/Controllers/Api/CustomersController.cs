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
        public IHttpActionResult GetOrders(int customerId, string orderStatus)
        {
            var orderList = _context.Orders.Where(o => o.CustomerId == customerId).Where(o => o.OrderStatus == orderStatus).ToList();
            return Ok();
        }

        [HttpPost]
        [Route("CreateOrder")]
        public IHttpActionResult CreateOrder(OrderDto orderDto)
        {
            orderDto.OrderId = 44;
            return Ok(orderDto);
        }

    }
}
