using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SmartMoveWebApp.Controllers
{
    public class TruckOwnersController : Controller
    {
        // GET: TruckOwners
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }
    }
}