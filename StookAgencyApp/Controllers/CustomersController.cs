using StookAgencyApp.Helpers;
using StookAgencyApp.Models;
using StookAgencyApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace StookAgencyApp.Controllers
{
    [Authorize]
    [RoutePrefix("customers")]
    public class CustomersController : Controller
    {
        private readonly AppDbContext _appDbContext;

        public CustomersController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext ?? throw new ArgumentNullException(nameof(appDbContext));
        }

        [HttpPost]
        [Route("")]
        public async Task<ActionResult> Post(CustomerViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return ResponseValidateFailed();
            }

            var customer = new Customer
            {
                Name = model.Name,
                DateJoined = model.DateJoined,
                Email = model.Email
            };

            _appDbContext.Customers.Add(customer);

            await _appDbContext.SaveChangesAsync();

            return new JsonCamelCaseResult(customer, JsonRequestBehavior.AllowGet);
        }
        
        [HttpPut]
        [Route("{id:int}")]
        public async Task<ActionResult> Put(int id, CustomerViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return ResponseValidateFailed();
            }

            var customer = _appDbContext.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }

            customer.Name = model.Name;
            customer.Email = model.Email;
            customer.DateJoined = model.DateJoined;

            await _appDbContext.SaveChangesAsync();

            return new JsonCamelCaseResult(customer, JsonRequestBehavior.AllowGet);
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var customer = _appDbContext.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }

            _appDbContext.Customers.Remove(customer);

            await _appDbContext.SaveChangesAsync();

            return Json(true, JsonRequestBehavior.AllowGet);
        }

        private ActionResult ResponseValidateFailed()
        {
            return Json(GetValidateErrors(), JsonRequestBehavior.AllowGet);
        }

        private IEnumerable<string> GetValidateErrors()
        {
            IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
            var errMessages = allErrors.Select(e => e.ErrorMessage);
            return errMessages;
        }
    }
}