using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartMoveWebApp.Models.ViewModels
{
    public class VerifyEmailViewModel
    {
        public string PageContent { get; set; }

        public static string GetSuccessMessage()
        {
            return "Thank you for confirming your email.";
        }

        public static string GetInvalidTokenMessage()
        {
            return "Error. Email was not verified.";
        }
    }
}