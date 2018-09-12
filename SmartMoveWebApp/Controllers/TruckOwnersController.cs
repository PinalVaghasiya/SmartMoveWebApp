using SmartMoveWebApp.BusniessLogic;
using SmartMoveWebApp.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using SmartMoveWebApp.Dtos;
using SmartMoveWebApp.Models;
using System.Text.RegularExpressions;

namespace SmartMoveWebApp.Controllers
{
    public class TruckOwnersController : Controller
    {
        public SmartMoveEntities _context { get; set; }

        public TruckOwnersController()
        {
            _context = new SmartMoveEntities();
        }

        [CheckDriverAuthorization]
        public ActionResult Index()
        {
            string emailId = this.HttpContext.Session["DriverID"].ToString();

            var truckOwner = _context.TruckOwners.Single(t => t.Email == emailId);
            return View(truckOwner);
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login(string returnURL)
        {
            var driverInfo = new LoginViewModel();

            EnsureLoggedOut();
            driverInfo.ReturnURL = returnURL;

            return View(driverInfo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model)
        {
            string OldHASHValue = string.Empty;
            byte[] SALT = new byte[64];

            if(ModelState.IsValid)
            {
                var truckOwner = _context.TruckOwners.SingleOrDefault(t => t.Email == model.Email);
                if (truckOwner == null)
                {
                    ModelState.AddModelError("", "Given Email is not registered with us.");
                    return View(model);
                }

                var login = _context.Logins.SingleOrDefault(l => l.Email == truckOwner.Email);
                if(login == null)
                {
                    ModelState.AddModelError("", "Given Email is not registered with us.");
                    return View(model);
                } else if (!login.EmailActivated)
                {
                    ModelState.AddModelError("", "Email is not verified, please verify from the email sent.");
                    return View(model);
                }

                OldHASHValue = login.Password;
                SALT = login.PasswordSalt;

                bool isValidLogin = AuthenticationLogic.CompareHashValue(model.Password, model.Email, OldHASHValue, SALT);

                if(!isValidLogin)
                {
                    ModelState.AddModelError("", "Given password is incorrect.");
                    return View(model);
                }

                FormsAuthentication.SignOut();
                // Write the authentication cookie  
                FormsAuthentication.SetAuthCookie(truckOwner.Email, false);

                Session["DriverID"] = model.Email;

                return RedirectToLocal(model.ReturnURL);
            }

            return View(model);
        }

        public ActionResult Logout()
        {
            // First we clean the authentication ticket like always  
            FormsAuthentication.SignOut();

            // Second we clear the principal to ensure the user does not retain any authentication  
            HttpContext.User = new GenericPrincipal(new GenericIdentity(string.Empty), null);

            Session.Clear();
            System.Web.HttpContext.Current.Session.RemoveAll();

            // Last we redirect to a controller/action that requires authentication to ensure a redirect takes place 
            // this clears the Request.IsAuthenticated flag since this triggers a new request  
            return RedirectToLocal();
        }

        private ActionResult RedirectToLocal(string returnURL = "")
        {
            try
            {
                // If the return url starts with a slash "/" we assume it belongs to our site  
                // so we will redirect to this "action"  
                if (!string.IsNullOrWhiteSpace(returnURL) && Url.IsLocalUrl(returnURL))
                    return RedirectToAction(returnURL);

                // If we cannot verify if the url is local to our host we redirect to a default location  
                return RedirectToAction("Index", "TruckOwners");
            }
            catch
            {
                throw;
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult RegisterDriver(RegisterDriverViewModel model)
        {
            if (ModelState.IsValid)
            {
                var checkEmailUniqueness = _context.Logins.SingleOrDefault(l => l.Email == model.Email);
                var checkPhoneUnqiueness = _context.TruckOwners.SingleOrDefault(t => t.Phone == model.Phone);
                var checkTruckUniqueness = _context.Trucks.SingleOrDefault(t => t.LicensePlate == model.LicensePlate);

                if(checkEmailUniqueness != null)
                    ModelState.AddModelError("", "Email id is already registered.");

                if (checkPhoneUnqiueness != null)
                    ModelState.AddModelError("", "Phone number is already registered.");

                if (checkTruckUniqueness != null)
                    ModelState.AddModelError("", "Truck License Number is already registered.");

                if(checkEmailUniqueness == null && checkPhoneUnqiueness == null && checkTruckUniqueness == null)
                {
                    var salt = AuthenticationLogic.Get_SALT(64);

                    var login = new Login
                    {
                        Email = model.Email.Trim(),
                        Password = AuthenticationLogic.Get_HASH_SHA512(model.Password, model.Email, salt),
                        PasswordSalt = salt,
                        UserType = "D",
                        EmailActivated = false,
                        CreatedTime = DateTime.Now,
                        ModifiedTime = DateTime.Now
                    };
                    _context.Logins.Add(login);

                    var truckOwner = new TruckOwner
                    {
                        FirstName = model.FirstName.Trim(),
                        LastName = model.LastName.Trim(),
                        Phone = model.Phone.Trim(),
                        Email = model.Email.Trim(),
                        CurrentStatusActive = false,
                        Address1 = model.Address1.Trim(),
                        Address2 = model.Address2.Trim(),
                        ZipCode = model.ZipCode.Trim(),
                        City = model.City.Trim(),
                        State = model.State.Trim(),
                        DriverLicenseNumber = model.DriverLicenseNumber.Trim(),
                        VehicleRegNumber = model.VehicleRegNumber.Trim(),
                        DriverInsurancePolicy = model.DriverInsurancePolicy.Trim(),
                        CreatedTime = DateTime.Now,
                        ModifiedTime = DateTime.Now
                    };
                    _context.TruckOwners.Add(truckOwner);

                    var truck = new Truck
                    {
                        TruckOwnerId = truckOwner.TruckOwnerId,
                        TruckTypeId = model.TruckTypeId,
                        TruckMake = model.TruckMake.Trim(),
                        TruckModel = model.TruckModel.Trim(),
                        TruckYear = model.TruckYear.ToString(),
                        LicensePlate = model.LicensePlate.Trim(),
                        TruckColor = model.TruckColor.Trim(),
                        CreatedTime = DateTime.Now,
                        ModifiedTime = DateTime.Now
                    };
                    _context.Trucks.Add(truck);
                    _context.SaveChanges();

                    string token = truckOwner.TruckOwnerId + "c45kaa52165hrd84rd";
                    string verificationUrl = Url.Action("VerifyEmail", "TruckOwners", new { token = token }, Request.Url.Scheme);

                    SendGridEmailService.SendEmailActivationLink("Driver", truckOwner.Email, truckOwner.FirstName, verificationUrl);

                    return RedirectToAction("Index", "Home");
                }
            }
            model.TruckTypesList = GetTruckTypes();
            return View("../Home/BecomeDriver", model);
        }

        [AllowAnonymous]
        public ActionResult VerifyEmail(string token)
        {
            var viewModel = new VerifyEmailViewModel();

            if(String.IsNullOrEmpty(token) || String.IsNullOrWhiteSpace(token))
            {
                viewModel.PageContent = VerifyEmailViewModel.GetInvalidTokenMessage();
                return View(viewModel);
            }

            token = token.Trim();
            string tokenId = token.Substring(0, token.Length - 18);

            if (!Regex.IsMatch(tokenId, @"^\d+$"))
                viewModel.PageContent = VerifyEmailViewModel.GetInvalidTokenMessage();

            int truckOwnerId = Convert.ToInt32(tokenId);

            var truckOwner = _context.TruckOwners.SingleOrDefault(t => t.TruckOwnerId == truckOwnerId);

            if (truckOwner == null)
                viewModel.PageContent = VerifyEmailViewModel.GetInvalidTokenMessage();
            else
            {
                var login = _context.Logins.SingleOrDefault(l => l.Email == truckOwner.Email);
                if(login == null || login.EmailActivated)
                    viewModel.PageContent = VerifyEmailViewModel.GetInvalidTokenMessage();
                else
                {
                    login.EmailActivated = true;
                    _context.SaveChanges();
                    viewModel.PageContent = VerifyEmailViewModel.GetSuccessMessage();
                }
            }
            return View(viewModel);
        }

        private void EnsureLoggedOut()
        {
            // If the request is (still) marked as authenticated we send the user to the logout action  
            if (Request.IsAuthenticated)
                Logout();
        }

        public IEnumerable<TruckType> GetTruckTypes()
        {
            return _context.TruckTypes.ToList();
        }
    }
}