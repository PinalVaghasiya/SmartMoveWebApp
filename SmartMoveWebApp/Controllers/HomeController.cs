using SmartMoveWebApp.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
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

            return View();
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