using SmartMoveWebApp.BusniessLogic;
using SmartMoveWebApp.Models;
using SmartMoveWebApp.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using System.Web.Security;

namespace SmartMoveWebApp.Controllers
{
	public class CustomersController : Controller
	{
		public SmartMoveEntities _context { get; set; }

		public CustomersController()
		{
			_context = new SmartMoveEntities();
		}

		[CheckCustomerAuthorization]
		public ActionResult Index()
		{
			string email = GetCustomerEmail();
			var customer = _context.Customers.Single(c => c.Email == email);
			return View(customer);
		}

		[HttpGet]
		[AllowAnonymous]
		public ActionResult Login(string returnURL)
		{
			var userInfo = new LoginViewModel();

			EnsureLoggedOut();
			userInfo.ReturnURL = returnURL;

			return View(userInfo);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Login(LoginViewModel model)
		{
			string OldHASHValue = string.Empty;
			byte[] SALT = new byte[64];

			if (ModelState.IsValid)
			{
				var customer = _context.Customers.SingleOrDefault(c => c.Email == model.Email);
				if (customer == null)
				{
					ModelState.AddModelError("", "Given Email is not registered with us.");
					return View(model);
				}

				var login = _context.Logins.SingleOrDefault(l => l.Email == customer.Email);
				if (login == null)
				{
					ModelState.AddModelError("", "Given Email is not registered with us.");
					return View(model);
				}
				else if (!login.EmailActivated)
				{
					ModelState.AddModelError("", "Email is not verified, please verify from the email sent.");
					return View(model);
				}

				OldHASHValue = login.Password;
				SALT = login.PasswordSalt;

				bool isValidLogin = AuthenticationLogic.CompareHashValue(model.Password, model.Email, OldHASHValue, SALT);

				if (!isValidLogin)
				{
					ModelState.AddModelError("", "Given password is incorrect.");
					return View(model);
				}

				FormsAuthentication.SignOut();
				// Write the authentication cookie
				FormsAuthentication.SetAuthCookie(customer.Email, false);

				Session["CustomerID"] = model.Email;

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
			// If the return url starts with a slash "/" we assume it belongs to our site  
			// so we will redirect to this "action"
			if (!string.IsNullOrWhiteSpace(returnURL) && Url.IsLocalUrl(returnURL))
				return RedirectToAction(returnURL);

			// If we cannot verify if the url is local to our host we redirect to a default location  
			return RedirectToAction("Dashboard", "Customers");
		}

		private void EnsureLoggedOut()
		{
			// If the request is (still) marked as authenticated we send the user to the logout action  
			if (Request.IsAuthenticated)
				Logout();
		}

		[HttpGet]
		public ActionResult Register()
		{
			var model = new RegisterCustomerViewModel();
			return View(model);
		}

		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public ActionResult Register(RegisterCustomerViewModel model)
		{
			if (ModelState.IsValid)
			{
				var checkEmailUniqueness = _context.Logins.SingleOrDefault(l => l.Email == model.Email);
				var checkPhoneUnqiueness = _context.Customers.SingleOrDefault(t => t.Phone == model.Phone);

				if (checkEmailUniqueness != null)
					ModelState.AddModelError("", "Email id is already registered.");

				if (checkPhoneUnqiueness != null)
					ModelState.AddModelError("", "Phone number is already registered.");

				if (checkEmailUniqueness == null && checkPhoneUnqiueness == null)
				{
					var salt = AuthenticationLogic.Get_SALT(64);

					var login = new Login
					{
						Email = model.Email.Trim(),
						Password = AuthenticationLogic.Get_HASH_SHA512(model.Password, model.Email, salt),
						PasswordSalt = salt,
						UserType = "C",
						EmailActivated = false,
						CreatedTime = DateTime.Now,
						ModifiedTime = DateTime.Now
					};
					_context.Logins.Add(login);

					var customer = new Customer
					{
						FirstName = model.FirstName.Trim(),
						LastName = model.LastName.Trim(),
						Phone = model.Phone.Trim(),
						Email = model.Email.Trim(),
						CreatedTime = DateTime.Now,
						ModifiedTime = DateTime.Now
					};
					_context.Customers.Add(customer);
					_context.SaveChanges();

					string token = customer.CustomerId + "c45kaa52165hrd84rd";
					string verificationUrl = Url.Action("VerifyEmail", "Customers", new { token = token }, Request.Url.Scheme);

					SendGridEmailService.SendEmailActivationLink("Customer", customer.Email, customer.FirstName, verificationUrl);

					return RedirectToAction("Index", "Home");
				}
			}
			return View(model);
		}

		public ActionResult VerifyEmail(string token)
		{
			var viewModel = new VerifyEmailViewModel();

			if (String.IsNullOrEmpty(token) || String.IsNullOrWhiteSpace(token))
			{
				viewModel.PageContent = VerifyEmailViewModel.GetInvalidTokenMessage();
				return View(viewModel);
			}

			string tokenId = token.Substring(0, token.Length - 18);

			if (!Regex.IsMatch(tokenId, @"^\d+$"))
				viewModel.PageContent = VerifyEmailViewModel.GetInvalidTokenMessage();

			int customerId = Convert.ToInt32(tokenId);

			var customer = _context.Customers.SingleOrDefault(t => t.CustomerId == customerId);

			if (customer == null)
				viewModel.PageContent = VerifyEmailViewModel.GetInvalidTokenMessage();
			else
			{
				var login = _context.Logins.SingleOrDefault(l => l.Email == customer.Email);
				if (login == null || login.EmailActivated)
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

		[CheckCustomerAuthorization]
		public ActionResult MyTrips()
		{
			int customerId = GetCustomerId();
			IEnumerable<Order> orders = _context.Orders.Where(o => o.CustomerId == customerId).ToList();

			ViewBag.Name = GetCustomerName();
			return View(orders);
		}

		[CheckCustomerAuthorization]
		public ActionResult Dashboard()
		{
			string email = GetCustomerEmail();
			var customer = _context.Customers.Single(c => c.Email == email);

			ViewBag.Name = GetCustomerName();
			return View(customer);
		}

		[CheckCustomerAuthorization]
		public ActionResult Wallet()
		{
			ViewBag.Name = GetCustomerName();
			return View();
		}

		[CheckCustomerAuthorization]
		public ActionResult Share()
		{
			ViewBag.Name = GetCustomerName();
			return View();
		}

		private string GetCustomerEmail()
		{
			if (this.HttpContext != null)
				return this.HttpContext.Session["CustomerID"].ToString();
			else
				return String.Empty;
		}

		private int GetCustomerId()
		{
			string email = GetCustomerEmail();
			var customerId = _context.Customers.Single(c => c.Email == email).CustomerId;
			return customerId;
		}

		private string GetCustomerName()
		{
			string email = GetCustomerEmail();
			if (String.IsNullOrEmpty(email))
				return String.Empty;
			else
			{
				var customer = _context.Customers.Single(c => c.Email == email);
				return customer.FirstName + " " + customer.LastName;
			}
		}
	}
}