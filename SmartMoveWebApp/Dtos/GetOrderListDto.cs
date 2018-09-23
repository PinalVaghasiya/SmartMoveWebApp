using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartMoveWebApp.Dtos
{
    public class GetOrderListDto
    {
        public IEnumerable<OrderDto> RunningOrders { get; set; }

        public IEnumerable<OrderDto> CompletedOrders { get; set; }

        public IEnumerable<OrderDto> CancelledOrders { get; set; }
    }
}