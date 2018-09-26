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
            PENDING,
            CONFIRMED,
            DELIVERING,
            COMPLETED,
            CANCELLED
        }

        public enum OrderBidStatus
        {
            PENDING,
            ACCEPTED,
            COMPLETED,
            CANCELLED
        }

        public static double InitialOrderPaymentAmount = 25.0;

        public static string RegisterSuccessMessage = "SmartMove Registration successful. A verification link has been sent to your email. Please click on that link to get access to your account. Or continue to Login.";

        public static string ForgotPasswordEmailMessage = "Reset Password link has been sent to your email. Please click on that link and reset your password to get access to your account. Or continue to Login.";

        public static string ResetPasswordSuccessMessage = "Password reset successful. Continue to Login.";

        public static string ResetPasswordErrorMessage = "Reset Password link is expired. Please reset your password using a valid link sent to your email. Or continue to Login.";
    }
}