using CustomerService.Controllers;
using CustomerService.Models;
using CustomerService.Standard;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TestCustomer.Controller
{
    public sealed class CustomerControllerTest
    {
        private CustomerController _customerController;
        private Mock<IBizManager<Customer>> _custBizManagerMock;
        [SetUp]
        public void SetUp()
        {
            _custBizManagerMock = new Mock<IBizManager<Customer>>();
            _customerController = new CustomerController(_custBizManagerMock.Object);
        }
        [Test]
        public void ShouldReturnNoContent_When_GetAllCustomersEmpty()
        {

            var emptyCustomerList = (IList<Customer>)null;
            _custBizManagerMock
                .Setup(custBizManager => custBizManager.GetAll())
                .Returns(emptyCustomerList);

            var apiResponse = _customerController.GetAllCustomers();

            apiResponse.Should().NotBeNull();
            apiResponse.Should().BeOfType<NotFoundResult>();
            ((NotFoundResult)apiResponse).StatusCode.Should().Be(Convert.ToInt32(HttpStatusCode.NotFound));
        }
        [Test]
        public void ShouldResturnAllCustomerList_When_GetAllCustomersNotEmpty()
        {
            var customers = new List<Customer>
            {
                new Customer{ Id="12345678",FirstName="Arjun",LastName="Garg",DOB="05/11/1111"},
                new Customer{ Id="12345679",FirstName="Karan",LastName="Garg",DOB="05/11/1111"}
            };
            _custBizManagerMock
                .Setup(custBizManager => custBizManager.GetAll())
                .Returns(customers);

            var apiResponse = _customerController.GetAllCustomers();

            apiResponse.Should().NotBeNull();
            apiResponse.Should().BeOfType<OkObjectResult>();
            var httpResponse = apiResponse as OkObjectResult;

            httpResponse.StatusCode.Should().Be(Convert.ToInt32(HttpStatusCode.OK));
            httpResponse.Value.Should().NotBeNull();
            httpResponse.Value.Should().BeOfType<List<Customer>>();

            var CustomerFromResponseBody = httpResponse.Value as List<Customer>;
            CustomerFromResponseBody.Count.Should().Be(2);
        }

    }
}
