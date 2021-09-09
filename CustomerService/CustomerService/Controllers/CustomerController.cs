using CustomerService.Models;
using CustomerService.Standard;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CustomerService.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly IBizManager<Customer> CustBizManager;
        public CustomerController(IBizManager<Customer> CustBizManager)
        {
            this.CustBizManager = CustBizManager;
        }
        [HttpGet]
        public IActionResult GetAllCustomers()
        {
            var response = this.CustBizManager.GetAll();
            if (response != null && response.Count != 0)
            {
                return base.Ok(response);
            }
            return base.NotFound();
        }

        //
        [Authorize(Policy = Policies.Admin)]
        [HttpPost]
        public IActionResult AddCustomer([FromBody] Customer customer)
        {
            if (customer != null)
            {
                if(customer.FirstName==null || customer.LastName==null || customer.DOB==null)
                {
                    return base.UnprocessableEntity();
                }
                this.CustBizManager.Add(customer);
                return base.CreatedAtRoute(nameof(this.GetCustomerById), new { id = customer.Id }, customer);
            }
            return base.BadRequest();
        }

        [HttpGet("{id}", Name = "GetCustomerById")]
        public IActionResult GetCustomerById(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return base.BadRequest("Invalid Customer Id");

            }
            var response = this.CustBizManager.GetById(id);
            if (response == null)
            {
                return base.NotFound();
            }
            return base.Ok(response);
        }
        //
        [Authorize(Policy = Policies.Admin)]
        [HttpDelete("{id}", Name = "DeleteCustomerById")]
        public IActionResult DeleteCustomerById(string Id)
        {
           if(string.IsNullOrEmpty(Id))
            {
                return base.BadRequest();
            }
            var wasDeleted = this.CustBizManager.DeleteById(Id);
            if(!wasDeleted)
            {
                return base.NotFound();
            }
            return base.NoContent();
        }

        [Authorize(Policy = Policies.Admin)]
        [HttpPut("{id}")]
        public IActionResult UpdateCustomerById(string id , [FromBody] Customer customer)
        {
            if (string.IsNullOrEmpty(id) || customer==null)
            {
                return base.BadRequest();
            }
            if (customer.FirstName == null || customer.LastName == null || customer.DOB == null)
            {
                return base.UnprocessableEntity();
            }
            var updateStatus=this.CustBizManager.UpdateById(id, customer);
            
            if(!updateStatus)
            {
                return base.NotFound();
            }
            return base.NoContent();
        }

    }
}
