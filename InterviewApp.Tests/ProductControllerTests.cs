using InterviewApp.Controllers;
using InterviewApp.Core;
using InterviewApp.DataAccess;
using InterviewApp.Models;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InterviewApp.Tests
{
    [TestFixture]
    public class ProductControllerTets
    {
        private Mock<IProductsRepository> _repoMock;
        private ProductController _controller;

        [SetUp]
        public void Init()
        {
            _repoMock = new Mock<IProductsRepository>();
            _controller = new ProductController(_repoMock.Object);
        }

        [Test]
        public async Task Get_CalledWithoutId_Will_ReturnAllProducts()
        {
            var products = new List<Product>
            {
                new Product{Id = 0, Name = "Product1", Description = "Something"},
                new Product{Id = 1, Name = "Product2", Description = "Something else"}
            };
            _repoMock.Setup(r => r.GetAllAsync()).Returns(Task.FromResult(products));

            var result = await _controller.GetAll();

            
        }

        [Test]
        public void Get_CalledWithExistingId_Will_ReturnProperProduct()
        {

        }

        [Test]
        public void Get_CalledWithNotExistingId_Will_ReturnNotFound()
        {

        }

        [Test]
        public void Create_CalledWithValidInput_Will_AddNewProduct_And_ReturnRedirect()
        {

        }

        [Test]
        public void Delete_CalledforExistingId_Will_DeleteProduct()
        {

        }
    }
}
