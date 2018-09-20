using AutoMapper;
using SmartMoveWebApp.BusniessLogic;
using SmartMoveWebApp.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SmartMoveWebApp.Controllers.Api
{
    [RoutePrefix("api/Account")]
    public class AccountsController : ApiController
    {
        public SmartMoveEntities _context { get; set; }

        public AccountsController()
        {
            _context = new SmartMoveEntities();
        }

        [HttpPost]
        [Route("Login")]
        public IHttpActionResult Login(AccountLoginDto model)
        {
            string OldHASHValue = string.Empty;
            byte[] SALT = new byte[64];
            List<string> errors = new List<string>();

            if (!ModelState.IsValid)
                return BadRequest();

            switch(model.AccountType)
            {
                case "C":
                    var customer = _context.Customers.SingleOrDefault(c => c.Email == model.Email);
                    if (customer == null)
                    {
                        ModelState.AddModelError("", "Given Email is not registered with us.");
                        return BadRequest(ModelState);
                    }

                    var login = _context.Logins.SingleOrDefault(l => l.Email == customer.Email);
                    if (login == null)
                    {
                        ModelState.AddModelError("", "Given Email is not registered with us.");
                        return BadRequest(ModelState);
                    }
                    else if (!login.EmailActivated)
                    {
                        ModelState.AddModelError("", "Email is not verified, please verify from the email sent.");
                        return BadRequest(ModelState);
                    }
                    OldHASHValue = login.Password;
                    SALT = login.PasswordSalt;

                    bool isValidLogin = AuthenticationLogic.CompareHashValue(model.Password, model.Email, OldHASHValue, SALT);

                    if (!isValidLogin)
                    {
                        ModelState.AddModelError("", "Given password is incorrect.");
                        return BadRequest(ModelState);
                    }
                    else
                    {
                        var customerDto = new CustomerDto
                        {
                            FirstName = customer.FirstName,
                            LastName = customer.LastName,
                        };
                        return Ok(customerDto);
                    }
                case "D":
                    var truckOwner = _context.TruckOwners.SingleOrDefault(t => t.Email == model.Email);
                    if (truckOwner == null)
                    {
                        ModelState.AddModelError("", "Given Email is not registered with us.");
                        return BadRequest(ModelState);
                    }

                    var truckOwnerLogin = _context.Logins.SingleOrDefault(l => l.Email == truckOwner.Email);
                    if (truckOwnerLogin == null)
                    {
                        ModelState.AddModelError("", "Given Email is not registered with us.");
                        return BadRequest(ModelState);
                    }
                    else if (!truckOwnerLogin.EmailActivated)
                    {
                        ModelState.AddModelError("", "Email is not verified, please verify from the email sent.");
                        return BadRequest(ModelState);
                    }

                    OldHASHValue = truckOwnerLogin.Password;
                    SALT = truckOwnerLogin.PasswordSalt;

                    bool isValid = AuthenticationLogic.CompareHashValue(model.Password, model.Email, OldHASHValue, SALT);

                    if (!isValid)
                    {
                        ModelState.AddModelError("", "Given password is incorrect.");
                        return BadRequest(ModelState);
                    }
                    else
                    {
                        var truckOwnerDto = new TruckOwnerDto
                        {
                            FirstName = truckOwner.FirstName,
                            LastName = truckOwner.LastName,
                            Trucks = truckOwner.Trucks
                        };
                        return Ok(truckOwnerDto);
                    }
                default:
                    ModelState.AddModelError("", "Invalid Account type");
                    return BadRequest(ModelState);
            }
        }
    }
}