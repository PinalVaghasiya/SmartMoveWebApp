using SmartMoveWebApp.BusniessLogic;
using SmartMoveWebApp.Models.ViewModels;
using System;
using System.Linq;
using System.Security.Principal;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using System.Web.Security;

namespace SmartMoveWebApp.Controllers
{
    public class HomeController : Controller
    {
        public SmartMoveEntities _context { get; set; }

        public HomeController()
        {
            _context = new SmartMoveEntities();
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            var model = new ContactUsEmailViewModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult Contact(ContactUsEmailViewModel model) {

            SendGridEmailService.ContactUsEmail(model.Email.Trim(), model.FullName.Trim(), model.Subject.Trim(), model.Message.Trim());

            return RedirectToAction("Index", "Home");
        }

        public ActionResult Help()
        {
            return View();
        }

        public ActionResult Login(string returnURL)
        {
            var userInfo = new LoginViewModel();

            EnsureLoggedOut();
            userInfo.ReturnURL = returnURL;

            return View(userInfo);
        }

        [HttpGet]
        public ActionResult ForgotPassword()
        {
            EnsureLoggedOut();
            return View(new ForgotPasswordViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var login = _context.Logins.SingleOrDefault(l => l.Email == model.Email);

                if (login == null)
                {
                    ModelState.AddModelError("Email", "Given email is not registered with us.");
                }
                else
                {
                    string token = login.LoginId + "hrd84rdc45kaa52165";
                    string verificationUrl = Url.Action("ResetPassword", "Home", new { token = token }, Request.Url.Scheme);
                    string userType = "Customer";

                    string userName = "";
                    switch (login.UserType)
                    {
                        case "C":
                            var customer = _context.Customers.Single(c => c.Email == login.Email);
                            userName = customer.FirstName + customer.LastName;
                            break;
                        case "D":
                            var truckOwner = _context.TruckOwners.Single(t => t.Email == login.Email);
                            userName = truckOwner.FirstName + truckOwner.LastName;
                            userType = "Driver";
                            break;
                    }
                    SendGridEmailService.SendForgotPasswordLink(userType, login.Email, userName, verificationUrl);

                    login.PasswordResetToken = token;
                    _context.SaveChanges();

                    TempData["ViewModel"] = new SuccessPageViewModel { Message = Constants.ForgotPasswordEmailMessage };
                    return RedirectToAction("Success", "Home");
                }
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult ResetPassword(string token)
        {
            EnsureLoggedOut();

            var viewModel = new ResetPasswordViewModel();

            if (String.IsNullOrEmpty(token) || String.IsNullOrWhiteSpace(token))
            {
                TempData["ViewModel"] = new ErrorPageViewModel { ErrorMessage = Constants.ResetPasswordErrorMessage };
                return RedirectToAction("ErrorPage", "Home");
            }

            token = token.Trim();
            string tokenId = token.Substring(0, token.Length - 18);

            if (!Regex.IsMatch(tokenId, @"^\d+$"))
            {
                TempData["ViewModel"] = new ErrorPageViewModel { ErrorMessage = Constants.ResetPasswordErrorMessage };
                return RedirectToAction("ErrorPage", "Home");
            }

            int loginId = Convert.ToInt32(tokenId);

            var login = _context.Logins.Where(l => l.PasswordResetToken.Equals(token)).SingleOrDefault(l => l.LoginId == loginId);

            if (login == null || !login.EmailActivated)
            {
                TempData["ViewModel"] = new ErrorPageViewModel { ErrorMessage = Constants.ResetPasswordErrorMessage };
                return RedirectToAction("ErrorPage", "Home");
            }

            else
                viewModel.LoginId = login.LoginId;

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(ResetPasswordViewModel model)
        {
            if(ModelState.IsValid)
            {
                var login = _context.Logins.Single(l => l.LoginId == model.LoginId);

                var salt = AuthenticationLogic.Get_SALT(64);

                login.Password = AuthenticationLogic.Get_HASH_SHA512(model.Password, login.Email, salt);
                login.PasswordSalt = salt;
                login.PasswordResetToken = String.Empty;
                login.ModifiedTime = DateTime.Now;

                _context.SaveChanges();

                TempData["ViewModel"] = new SuccessPageViewModel { Message = Constants.ResetPasswordSuccessMessage };
                return RedirectToAction("Success", "Home");
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult ErrorPage()
        {

            return View(TempData["ViewModel"] as ErrorPageViewModel);
        }

        [HttpGet]
        public ActionResult Success()
        {
            return View(TempData["ViewModel"] as SuccessPageViewModel);
        }

        private void EnsureLoggedOut()
        {
            // If the request is (still) marked as authenticated we send the user to the logout action  
            if (Request.IsAuthenticated)
                FormsAuthentication.SignOut();

            // Second we clear the principal to ensure the user does not retain any authentication  
            HttpContext.User = new GenericPrincipal(new GenericIdentity(string.Empty), null);

            Session.Clear();
            System.Web.HttpContext.Current.Session.RemoveAll();
        }
    }
}