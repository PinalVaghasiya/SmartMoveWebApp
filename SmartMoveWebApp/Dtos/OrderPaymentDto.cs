using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SmartMoveWebApp.Dtos
{
    public class OrderPaymentDto
    {
        public int OrderId { get; set; }

        public double PaymentAmout { get; set; }

        public string PaymentType { get; set; }

        public string PaymentStatus { get; set; }

        public long Time { get; set; }
    }
}