using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SmartMoveWebApp.Controllers.Api
{
    public class TruckOnwersController : ApiController
    {
        public SmartMoveEntities _context { get; set; }

        public TruckOnwersController()
        {
            _context = new SmartMoveEntities();
        }

        [HttpGet]
        public IEnumerable<TruckType> GetTruckTypes()
        {
            return _context.TruckTypes.ToList();
        }
    }
}
