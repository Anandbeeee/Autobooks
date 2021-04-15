using GroceryStoreAPI.Controllers;
using GroceryStoreAPI.Models;
using GroceryStoreAPI.Repository;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using Xunit;

namespace GroceryStoreAPITest.ControllerTest
{
    public class CustomerControllerTests
    {
        private readonly Mock<ICustomerRepository> _mockRepo;
        private readonly CustomerController _controller;
        public CustomerControllerTests()
        {
            //Arrange
            _mockRepo = new Mock<ICustomerRepository>();
            _controller = new CustomerController(_mockRepo.Object);
        }

        [Fact]
        public void GetCustomers_ActionExecutes_ReturnsListOfCustomers()
        {
            //Act
            var result = _controller.GetAllCustomers();
            //Assert
            Assert.IsType<ActionResult<List<Customer>>>(result);
        }

        [Fact]
        public void GetCustomers_ActionExecutes_ReturnsExactNumberOfCustomers()
        {
            //Arrange
            _mockRepo.Setup(repo => repo.GetAll())
                .Returns(new List<Customer>() { new Customer(), new Customer() });

            //Act
            var result = _controller.GetAllCustomers();
            var resultType = Assert.IsType<ActionResult<List<Customer>>>(result);
            var customers = Assert.IsType<List<Customer>>(resultType.Value);

            //Assert
            Assert.Equal(2, customers.Count);
        }

        [Fact]
        public void GetCustomerById_InvalidInput_ReturnsNotFoundResult()
        {
            //Arrange
            _mockRepo.Setup(repo => repo.GetById(2))
                .Returns(new Customer() { Id = 2, Name = "TestCustomer" });

            //Act
            var result = _controller.GetCustomerById(1);

            //Assert
            var resultType = Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void GetCustomerById_ValidInput_ReturnsOkObjectResponse()
        {
            //Arrange
            _mockRepo.Setup(repo => repo.GetById(2))
                .Returns(new Customer() { Id = 2, Name = "TestCustomer" });

            //Act
            var result = _controller.GetCustomerById(2);

            //Assert
            var testCustomer = Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void GetCustomerById_ValidInput_ReturnsValidResponseObject()
        {
            //Arrange
            _mockRepo.Setup(repo => repo.GetById(2))
                .Returns(new Customer() { Id = 2, Name = "TestCustomer" });

            //Act
            var cust = _controller.GetCustomerById(2);
            var result = Assert.IsType<OkObjectResult>(cust);
            var testCustomer = Assert.IsType<Customer>(result.Value);

            //Assert
            Assert.Equal(2, testCustomer.Id);
            Assert.Equal("TestCustomer", testCustomer.Name);
        }


        [Fact]
        public void AddCustomer_InvalidInputId_ReturnsBadRequestObject()
        {
            //Arrange
            var customer = new Customer() { Name = "TestCustomer" };
            _controller.ModelState.AddModelError("Id", "Id is required");

            //Act
            var result = _controller.AddCustomer(customer);

            //Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void AddCustomer_InvalidInputName_ReturnsBadRequestObject()
        {
            //Arrange
            var customer = new Customer() { Id = 1 };
            _controller.ModelState.AddModelError("Name", "Name is required");

            //Act
            var result = _controller.AddCustomer(customer);

            //Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void AddCustomer_ValidInputObject_ReturnsOkObjectResponse()
        {
            //Arrange
            var customer = new Customer { Id = 20, Name = "TestCustomer" };
            _mockRepo.Setup(repo => repo.Insert(customer))
                .Returns(customer);

            //Act
            var result = _controller.AddCustomer(customer);

            //Assert
            var testCustomer = Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void AddCustomer_ValidInputObject_ReturnsValidResponseObject()
        {
            //Arrange
            var customer = new Customer { Id = 20, Name = "TestCustomer" };
            _mockRepo.Setup(repo => repo.Insert(customer))
                .Returns(customer);

            //Act
            var cust = _controller.AddCustomer(customer);
            var result = Assert.IsType<OkObjectResult>(cust);
            var testCustomer = Assert.IsType<Customer>(result.Value);

            //Assert
            Assert.Equal(customer.Id, testCustomer.Id);
            Assert.Equal(customer.Name, testCustomer.Name);
        }

        [Fact]
        public void UpdateCustomer_InvalidInputId_ReturnsBadRequestObject()
        {
            //Arrange
            var customer = new Customer() { Name = "TestCustomer" };
            _controller.ModelState.AddModelError("Id", "Id is required");

            //Act
            var result = _controller.UpdateCustomer(customer);

            //Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void UpdateCustomer_InvalidInputName_ReturnsBadRequestObject()
        {
            //Arrange
            var customer = new Customer() { Id = 2 };
            _controller.ModelState.AddModelError("Name", "Name is required");

            //Act
            var result = _controller.UpdateCustomer(customer);

            //Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void UpdateCustomer_ValidInputObject_ReturnsOkObjectResponse()
        {
            //Arrange
            var customer = new Customer { Id = 10, Name = "TestCustomer" };
            _mockRepo.Setup(repo => repo.Update(customer))
                .Returns(customer);

            //Act
            var result = _controller.UpdateCustomer(customer);

            //Assert
            var testCustomer = Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void UpdateCustomer_ValidInputObject_ReturnsValidResponseObject()
        {
            //Arrange
            var customer = new Customer { Id = 10, Name = "TestCustomer" };
            _mockRepo.Setup(repo => repo.Update(customer))
                .Returns(customer);

            //Act
            var cust = _controller.UpdateCustomer(customer);
            var result = Assert.IsType<OkObjectResult>(cust);
            var testCustomer = Assert.IsType<Customer>(result.Value);

            //Assert
            Assert.Equal(customer.Id, testCustomer.Id);
            Assert.Equal(customer.Name, testCustomer.Name);
        }
    }
}
