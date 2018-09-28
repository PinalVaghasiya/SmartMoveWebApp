using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartMoveWebApp.Dtos
{
    public class GetCustomerOrderListDto
    {
        public IEnumerable<CustomerOrderDto> RunningOrders { get; set; }

        public IEnumerable<CustomerOrderDto> CompletedOrders { get; set; }

        public IEnumerable<CustomerOrderDto> CancelledOrders { get; set; }
    }
}