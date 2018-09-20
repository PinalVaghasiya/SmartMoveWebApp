using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartMoveWebApp.BusniessLogic
{
    public static class Constants
    {
        public static string SendGridApiKey = "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";

        public enum OrderStatus
        {
            Pending,
            Confirmed,
            Completed

        }
    }
}