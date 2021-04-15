using GroceryStoreAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using GroceryStoreAPI;
using GroceryStoreAPI.Repository;

namespace GroceryStoreAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerRepository customerRepository;

        //Dependency Injection through Repository Design pattern
        public CustomerController(ICustomerRepository _customerRepository)
        {
            customerRepository = _customerRepository;
        }

        [HttpGet]
        [Route("")]
        public ActionResult<List<Customer>> GetAllCustomers()
        {
            return customerRepository.GetAll();
        }

        [HttpGet("{id}")]
        public ActionResult GetCustomerById(int id)
        {
            var customer = customerRepository.GetById(id);
            if (customer == null)
            {
                return NotFound();
            }
            return Ok(customer);
        }

        [HttpPost]
        public ActionResult AddCustomer([FromBody] Customer customer)
        {
            var newCustomer = customerRepository.Insert(customer);
            if (newCustomer == null)
            {
                return BadRequest("Duplicate or Invalid customer");
            }
            return Ok(newCustomer);
        }

        [HttpPut]
        public ActionResult UpdateCustomer([FromBody] Customer customer)
        {
            var updatedCustomer = customerRepository.Update(customer);
            if (updatedCustomer == null)
            {
                return BadRequest("Not a valid Customer");
            }
            return Ok(updatedCustomer);
        }
    }
}
