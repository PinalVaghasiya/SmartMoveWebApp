using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartMoveWebApp.Dtos
{
    public class GetBidsListDto
    {
        public IEnumerable<OrderBidDto> PendingBids { get; set; }

        public IEnumerable<OrderBidDto> AcceptedBids { get; set; }
    }
}