using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartMoveWebApp.BusniessLogic
{
    public class BusinessLogic
    {
        public static SmartMoveEntities _context = new SmartMoveEntities();

        public static double GetAverageDriverRating(int truckOwnerId)
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

        public static double GetAverageCustomerRating(int customerId)
        {
            var hasRatings = _context.CustomerRatings.Where(r => r.CustomerId == customerId).ToList();

            if (hasRatings != null && hasRatings.Count > 0)
            {
                double averageRating = hasRatings.Average(r => r.Rating);
                return averageRating;
            }
            else
                return 0;
        }
        public static string getTruckOwnerEmail(int truckOwnerId)
        {
            var email = _context.TruckOwners.Single(t => t.TruckOwnerId == truckOwnerId);
            return email.Email;
        }

        public static string getTruckOwnerName(int truckOwnerId)
        {
            var email = _context.TruckOwners.Single(t => t.TruckOwnerId == truckOwnerId);
            return email.FirstName + " " + email.LastName;
        }

        public static string getCustomerEmail(int customerId)
        {
            var email = _context.Customers.Single(t => t.CustomerId == customerId);
            return email.Email;
        }

        public static string getCustomerName(int customerId)
        {
            var email = _context.Customers.Single(t => t.CustomerId == customerId);
            return email.FirstName + " " + email.LastName;
        }
    }
}