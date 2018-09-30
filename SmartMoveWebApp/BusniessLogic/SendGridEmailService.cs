using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Web;

namespace SmartMoveWebApp.BusniessLogic
{
    public class SendGridEmailService
    {
        public static void SendEmailActivationLink(string userType, string userEmail, string userName, string verificationUrl)
        {
            var client = new SendGridClient(Constants.SendGridApiKey);
            var from = new EmailAddress("noreply@smartmove.com", "SmartMove");
            var subject = "SmartMove " + userType + " registration successful. Please verify your email by clicking on the link below.";
            var to = new EmailAddress(userEmail, userName);

            string path1 = Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).AbsolutePath);
            path1 = HttpUtility.UrlDecode(path1);
            var htmlContent = File.ReadAllText(Path.Combine(path1, @"..\Views\EmailTemplates\EmailVerificationTemplate.html"));
            htmlContent = htmlContent.Replace("<%verificationUrl%>", verificationUrl);
            htmlContent = htmlContent.Replace("<%userType%>", userType);

            var msg = MailHelper.CreateSingleEmail(from, to, subject, "", htmlContent);
            var response = client.SendEmailAsync(msg);
        }

        public static void SendForgotPasswordLink(string userType, string userEmail, string userName, string verificationUrl)
        {
            var client = new SendGridClient(Constants.SendGridApiKey);
            var from = new EmailAddress("noreply@smartmove.com", "SmartMove");
            var subject = "SmartMove - Password Reset URL. Please reset your password by clicking on the link below.";
            var to = new EmailAddress(userEmail, userName);

            string path1 = Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).AbsolutePath);
            path1 = HttpUtility.UrlDecode(path1);
            var htmlContent = File.ReadAllText(Path.Combine(path1, @"..\Views\EmailTemplates\ResetPasswordTemplate.html"));
            htmlContent = htmlContent.Replace("<%verificationUrl%>", verificationUrl);
            htmlContent = htmlContent.Replace("<%userType%>", userType);

            var msg = MailHelper.CreateSingleEmail(from, to, subject, "", htmlContent);
            var response = client.SendEmailAsync(msg);
        }

        public static void ContactUsEmail(string userEmail, string fullName, string subject, string message)
        {
            var client = new SendGridClient(Constants.SendGridApiKey);
            var from = new EmailAddress("noreply@smartmove.com", "SmartMove");
            string adminEmail = "ksoni004@gmail.com";
            var to = new EmailAddress(adminEmail, "Admin - SmartMove Web");
            var emailSubject = "SmartMove - Contact Us Query";

            string path1 = Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).AbsolutePath);
            path1 = HttpUtility.UrlDecode(path1);
            var htmlContent = File.ReadAllText(Path.Combine(path1, @"..\Views\EmailTemplates\ContactUsTemplate.html"));
            htmlContent = htmlContent.Replace("<%userEmail%>", userEmail);
            htmlContent = htmlContent.Replace("<%fullName%>", fullName);
            htmlContent = htmlContent.Replace("<%userSubject%>", subject);
            htmlContent = htmlContent.Replace("<%userMessage%>", message);

            var msg = MailHelper.CreateSingleEmail(from, to, emailSubject, "", htmlContent);
            var response = client.SendEmailAsync(msg);
        }

        public static void AccountVerifiedEmail(string email, string name)
        {
            var client = new SendGridClient(Constants.SendGridApiKey);
            var from = new EmailAddress("noreply@smartmove.com", "SmartMove");
            var to = new EmailAddress(email, name);
            var emailSubject = "SmartMove - Email Verified";

            string path1 = Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).AbsolutePath);
            path1 = HttpUtility.UrlDecode(path1);
            var htmlContent = File.ReadAllText(Path.Combine(path1, @"..\Views\EmailTemplates\AccountVerifiedTemplate.html"));

            var msg = MailHelper.CreateSingleEmail(from, to, emailSubject, "", htmlContent);
            var response = client.SendEmailAsync(msg);
        }

        public static void OrderCreated(string email, string name, int orderId, DateTime orderDateTime, string orderStatus, string orderPickupPlace, string orderDropPlace)
        {
            var client = new SendGridClient(Constants.SendGridApiKey);
            var from = new EmailAddress("noreply@smartmove.com", "SmartMove");
            var to = new EmailAddress(email, name);
            var emailSubject = "SmartMove - Order Created";

            string path1 = Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).AbsolutePath);
            path1 = HttpUtility.UrlDecode(path1);
            var htmlContent = File.ReadAllText(Path.Combine(path1, @"..\Views\EmailTemplates\OrderCreatedTemplate.html"));
            htmlContent = htmlContent.Replace("<%orderId%>", "" + orderId);
            htmlContent = htmlContent.Replace("<%orderDateTime%>", "" + orderDateTime);
            htmlContent = htmlContent.Replace("<%orderStatus%>", orderStatus);
            htmlContent = htmlContent.Replace("<%orderPickup%>", orderPickupPlace);
            htmlContent = htmlContent.Replace("<%orderDrop%>", orderDropPlace);

            var msg = MailHelper.CreateSingleEmail(from, to, emailSubject, "", htmlContent);
            var response = client.SendEmailAsync(msg);
        }

        public static void OrderCancelled(string email, string name, int orderId, string orderStatus)
        {
            var client = new SendGridClient(Constants.SendGridApiKey);
            var from = new EmailAddress("noreply@smartmove.com", "SmartMove");
            var to = new EmailAddress(email, name);
            var emailSubject = "SmartMove - Order Cancelled";

            string path1 = Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).AbsolutePath);
            path1 = HttpUtility.UrlDecode(path1);
            var htmlContent = File.ReadAllText(Path.Combine(path1, @"..\Views\EmailTemplates\OrderCancelledTemplate.html"));
            htmlContent = htmlContent.Replace("<%orderId%>", "" + orderId);
            htmlContent = htmlContent.Replace("<%orderStatus%>", orderStatus);

            var msg = MailHelper.CreateSingleEmail(from, to, emailSubject, "", htmlContent);
            var response = client.SendEmailAsync(msg);
        }

        public static void BidAccepted(string email, string name, int orderId, DateTime orderDateTime, string orderStatus, double orderBidAmount, int truckOwnerId, DateTime deliveryStartTime)
        {
            var client = new SendGridClient(Constants.SendGridApiKey);
            var from = new EmailAddress("noreply@smartmove.com", "SmartMove");
            var to = new EmailAddress(email, name);
            var emailSubject = "SmartMove - Bid Accepted";

            string path1 = Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).AbsolutePath);
            path1 = HttpUtility.UrlDecode(path1);
            var htmlContent = File.ReadAllText(Path.Combine(path1, @"..\Views\EmailTemplates\BidAcceptedTemplate.html"));
            htmlContent = htmlContent.Replace("<%orderId%>", "" + orderId);
            htmlContent = htmlContent.Replace("<%orderDateTime%>", "" + orderDateTime);
            htmlContent = htmlContent.Replace("<%bidStatus%>", orderStatus);
            htmlContent = htmlContent.Replace("<%orderBidAmount%>", "" + orderBidAmount);
            htmlContent = htmlContent.Replace("<%truckOwnerId%>", "" + truckOwnerId);
            htmlContent = htmlContent.Replace("<%deliveryStartTime%>", "" + deliveryStartTime);

            var msg = MailHelper.CreateSingleEmail(from, to, emailSubject, "", htmlContent);
            var response = client.SendEmailAsync(msg);
        }

        public static void BidPlaced(string truckOwnerEmail, string truckOwnerName, int orderId, double orderBidAmount, string bidStatus, DateTime deliveryStartTime, double numberOfHours, int numberOfTrips)
        {
            var client = new SendGridClient(Constants.SendGridApiKey);
            var from = new EmailAddress("noreply@smartmove.com", "SmartMove");
            var to = new EmailAddress(truckOwnerEmail, truckOwnerName);
            var emailSubject = "SmartMove - Bid Placed";

            string path1 = Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).AbsolutePath);
            path1 = HttpUtility.UrlDecode(path1);
            var htmlContent = File.ReadAllText(Path.Combine(path1, @"..\Views\EmailTemplates\BidAcceptedTemplate.html"));
            htmlContent = htmlContent.Replace("<%orderId%>", "" + orderId);
            htmlContent = htmlContent.Replace("<%orderBidAmount%>", "" + orderBidAmount);
            htmlContent = htmlContent.Replace("<%bidStatus%>", bidStatus);
            htmlContent = htmlContent.Replace("<%deliveryStartTime%>", "" + deliveryStartTime);
            htmlContent = htmlContent.Replace("<%numberOfHours%>", "" + numberOfHours);
            htmlContent = htmlContent.Replace("<%numberOfTrips%>", "" + numberOfTrips);

            var msg = MailHelper.CreateSingleEmail(from, to, emailSubject, "", htmlContent);
            var response = client.SendEmailAsync(msg);
        }

        public static void BidRemoved(string email, string name, int orderId, double orderBidAmount, string bidStatus)
        {
            var client = new SendGridClient(Constants.SendGridApiKey);
            var from = new EmailAddress("noreply@smartmove.com", "SmartMove");
            var to = new EmailAddress(email, name);
            var emailSubject = "SmartMove - Bid Removed";

            string path1 = Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).AbsolutePath);
            path1 = HttpUtility.UrlDecode(path1);
            var htmlContent = File.ReadAllText(Path.Combine(path1, @"..\Views\EmailTemplates\BidRemovedTemplate.html"));
            htmlContent = htmlContent.Replace("<%orderId%>", "" + orderId);
            htmlContent = htmlContent.Replace("<%orderBidAmount%>", "" + orderBidAmount);
            htmlContent = htmlContent.Replace("<%bidStatus%>", bidStatus);

            var msg = MailHelper.CreateSingleEmail(from, to, emailSubject, "", htmlContent);
            var response = client.SendEmailAsync(msg);
        }

        public static void TripStarted(string email, string name, int orderId, DateTime startTime, string truckType, string orderPickupPlace, string orderDropPlace, string orderStatus)
        {
            var client = new SendGridClient(Constants.SendGridApiKey);
            var from = new EmailAddress("noreply@smartmove.com", "SmartMove");
            var to = new EmailAddress(email, name);
            var emailSubject = "SmartMove - Order Delivery Started";

            string path1 = Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).AbsolutePath);
            path1 = HttpUtility.UrlDecode(path1);
            var htmlContent = File.ReadAllText(Path.Combine(path1, @"..\Views\EmailTemplates\TripStartedTemplate.html"));
            htmlContent = htmlContent.Replace("<%orderId%>", "" + orderId);
            htmlContent = htmlContent.Replace("<%startTime%>", "" + startTime);
            htmlContent = htmlContent.Replace("<%truckType%>", truckType);
            htmlContent = htmlContent.Replace("<%orderPickupPlace%>", orderPickupPlace);
            htmlContent = htmlContent.Replace("<%orderDropPlace%>", orderDropPlace);
            htmlContent = htmlContent.Replace("<%orderStatus%>", orderStatus);

            var msg = MailHelper.CreateSingleEmail(from, to, emailSubject, "", htmlContent);
            var response = client.SendEmailAsync(msg);
        }

        public static void TripFinished(string email, string name, int orderId, DateTime startTime, string truckType, string orderPickupPlace, string orderDropPlace, string orderStatus)
        {
            var client = new SendGridClient(Constants.SendGridApiKey);
            var from = new EmailAddress("noreply@smartmove.com", "SmartMove");
            var to = new EmailAddress(email, name);
            var emailSubject = "SmartMove - Order Delivery Completed";

            string path1 = Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).AbsolutePath);
            path1 = HttpUtility.UrlDecode(path1);
            var htmlContent = File.ReadAllText(Path.Combine(path1, @"..\Views\EmailTemplates\TripFinishedTemplate.html"));
            htmlContent = htmlContent.Replace("<%orderId%>", "" + orderId);
            htmlContent = htmlContent.Replace("<%startTime%>", "" + startTime);
            htmlContent = htmlContent.Replace("<%truckType%>", truckType);
            htmlContent = htmlContent.Replace("<%orderPickupPlace%>", orderPickupPlace);
            htmlContent = htmlContent.Replace("<%orderDropPlace%>", orderDropPlace);
            htmlContent = htmlContent.Replace("<%orderStatus%>", orderStatus);

            var msg = MailHelper.CreateSingleEmail(from, to, emailSubject, "", htmlContent);
            var response = client.SendEmailAsync(msg);
        }
    }
}