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
    }
}