using SmartMoveWebApp.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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

        [HttpGet]
        public ActionResult BecomeDriver()
        {
            var truckTypes = _context.TruckTypes.ToList();
            var driverRegisterModel = new RegisterDriverViewModel();
            driverRegisterModel.TruckTypesList = truckTypes;
            driverRegisterModel.TruckYear = null;
            return View(driverRegisterModel);
        }
    }
}