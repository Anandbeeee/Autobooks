using GroceryStoreAPI.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GroceryStoreAPI.Repository
{
    public class JSONContext
    {
        private readonly IConfiguration config;
        private readonly string fileName;
        private readonly string jsonString;
        
        public JSONContext()
        {
            config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            fileName = config["CustomStrings:DbName"].ToString();
            jsonString = File.ReadAllText(fileName);
        }

        public List<Customer> GetAllCustomers()
        {
            CustomerList custList = JsonConvert.DeserializeObject<CustomerList>(jsonString);
            List<Customer> customerList = new();
            if (custList != null)
                customerList = custList.Customers.ToList();
            return customerList;
        }

        public Customer GetCustomerById(int CustId)
        {
            CustomerList custList = JsonConvert.DeserializeObject<CustomerList>(jsonString);
            var result = custList.Customers.ToList().FirstOrDefault(x => x.Id == CustId);
            return (Customer)result;
        }

        public Customer AddCustomer(Customer newCustomer)
        {
            return AddOrUpdateCustomer(newCustomer, true);
        }

        public Customer UpdateCustomer(Customer existingCustomer)
        {
            return AddOrUpdateCustomer(existingCustomer, false);
        }

        //Single function to improve code reusability
        public Customer AddOrUpdateCustomer(Customer customer, bool isInsert)
        {
            List<Customer> customerList = new();
            customerList = GetAllCustomers();

            //Verify if the input customer is valid
            if ((!isInsert && !customerList.Where(x => x.Id == customer.Id).Any()) ||
                 (isInsert && customerList.Where(x => x.Id == customer.Id).Any()))
            {
                return null;
            }

            //Check to verify if it's new or existing customer
            if (isInsert)
            {
                customerList.Add(customer);
            }
            else
            {
                customerList.Where(i => i.Id == customer.Id).ToList().ForEach(x => x.Name = customer.Name);
            }

            //Rewrite the JSON text with latest update
            var convertedJson = JsonConvert.SerializeObject(customerList, Formatting.Indented);
            System.IO.File.WriteAllText(fileName, string.Concat(config["CustomStrings:CustomDBName"],
                                                   convertedJson, config["CustomStrings:ClosingTag"]));

            //Return the newly added or modified customer
            var updatedCustomer = customerList.ToList().FirstOrDefault(x => x.Id == customer.Id);
            return updatedCustomer;
        }
    }
}
