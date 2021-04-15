using GroceryStoreAPI.Models;
using System.Collections.Generic;

namespace GroceryStoreAPI.Repository
{
    public class CustomerRepository:ICustomerRepository
    {
        private readonly JSONContext context;

        public CustomerRepository()
        {
            context = new JSONContext();
        }

        public List<Customer> GetAll() => context.GetAllCustomers();

        public Customer GetById(int CustomerID) => context.GetCustomerById(CustomerID);

        public Customer Insert(Customer customer) => context.AddCustomer(customer);

        public Customer Update(Customer customer) => context.UpdateCustomer(customer);
    }
}
