using System.Collections.Generic;
using Domain;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace RecruitmentTestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly CustomerService _customerService;

        public CustomerController(CustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet]
        public ActionResult<List<Customer>> Get()
        {
            var temp = _customerService.Get();
            return temp;
        }

        [HttpGet("{id}", Name = "GetCustomer")]
        public ActionResult<Customer> Get(string id)
        {
            var customer = _customerService.Get(int.Parse(id));

            if (customer == null)
            {
                return NotFound();
            }

            return customer;
        }

        [HttpPost]
        public ActionResult<Customer> Create(Customer customer)
        {
            _customerService.Create(customer);

            return CreatedAtRoute("GetCustomer", new { id = customer.Id.ToString() }, customer);
        }

        [HttpPut("{id}")]
        public IActionResult Update(string id, Customer customerIn)
        {
            var customer = _customerService.Get(int.Parse(id));

            if (customer == null)
            {
                return NotFound();
            }

            _customerService.Update(int.Parse(id), customerIn);

            return NoContent();
        }

        [HttpDelete("{id}")]//:length(24)}")]
        public IActionResult Delete(string id)
        {
            var customer = _customerService.Get(int.Parse(id));

            if (customer == null)
            {
                return NotFound();
            }

            _customerService.Remove(customer.Id);

            return NoContent();
        }
    }
}