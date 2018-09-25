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

            var plainTextContent = "Please click on below link or paste the following URL: " + verificationUrl + "in your browser.";
            var htmlContent = "<a href='" + verificationUrl + "'>Verify Email</a>";

            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = client.SendEmailAsync(msg);
        }

        public static void SendForgotPasswordLink(string userEmail, string userName, string verificationUrl)
        {
            var client = new SendGridClient(Constants.SendGridApiKey);
            var from = new EmailAddress("noreply@smartmove.com", "SmartMove");
            var subject = "SmartMove - Password Reset URL. Please reset your password by clicking on the link below.";
            var to = new EmailAddress(userEmail, userName);

            var plainTextContent = "Please click on below link or paste the following URL: " + verificationUrl + "in your browser.";
            var htmlContent = "<a href='" + verificationUrl + "'>Reset Password</a>";

            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = client.SendEmailAsync(msg);
        }

        public static void ContactUsEmail(string userEmail, string fullName, string subject, string message)
        {
            var client = new SendGridClient(Constants.SendGridApiKey);
            var from = new EmailAddress("noreply@smartmove.com", "SmartMove");
            string adminEmail = "ksoni004@gmail.com";
            var to = new EmailAddress(adminEmail, "Admin - SmartMove Web");

            var htmlContent = File.ReadAllText(Path.Combine(Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).AbsolutePath), @"..\Views\EmailTemplates\VerifyEmail.aspx"));

            var msg = MailHelper.CreateSingleEmail(from, to, subject, "Contact Us Email", message + htmlContent);
            var response = client.SendEmailAsync(msg);
        }
    }
}