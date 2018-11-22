using FluentAssertions;
using InterviewApp.Controllers;
using InterviewApp.Core;
using InterviewApp.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InterviewApp.Tests
{
    [TestFixture]
    public class ProductControllerTets
    {
        private List<Product> _testProducts;
        private Mock<IProductsRepository> _repoMock;
        private ProductController _controller;

        [SetUp]
        public void Init()
        {
            _testProducts = new List<Product>
            {
                new Product{Id = 0, Name = "Product1", Description = "Something"},
                new Product{Id = 1, Name = "Product2", Description = "Something else"}
            };
            _repoMock = new Mock<IProductsRepository>();
                       
            _controller = new ProductController(_repoMock.Object);
        }

        [Test]
        public async Task Get_CalledWithoutId_Will_ReturnAllProducts()
        {
            _repoMock.Setup(r => r.GetAllAsync()).Returns(Task.FromResult(_testProducts)).Verifiable();
            var result = await _controller.GetAll();
            result.Should().NotBeNull();
            result.Value.Should().HaveCount(_testProducts.Count);
            result.Value.Should().BeEquivalentTo(_testProducts);
            _repoMock.Verify();
        }

        [TestCase(0)]
        [TestCase(1)]
        public async Task Get_CalledWithExistingId_Will_ReturnProperProduct(long id)
        {
            _repoMock.Setup(r => r.GetByIdAsync(It.IsAny<long>())).Returns<long>(i => Task.FromResult(_testProducts.SingleOrDefault(p => p.Id == i))).Verifiable();
            var expectedProduct = _testProducts.Single(p => p.Id == id);

            var result = await _controller.GetById(id);
            result.Should().NotBeNull();
            result.Value.Should().NotBeNull();
            result.Value.Id.Should().Be(expectedProduct.Id);
            result.Value.Name.Should().Be(expectedProduct.Name);
            result.Value.Description.Should().Be(expectedProduct.Description);
            _repoMock.Verify();
        }

        [Test]
        public async Task Get_CalledWithNotExistingId_Will_ReturnNotFound()
        {
            long nonExistingId = 999;
            _testProducts.Select(p => p.Id).Should().NotContain(nonExistingId);

            _repoMock.Setup(r => r.GetByIdAsync(It.IsAny<long>())).Returns<long>(i => Task.FromResult(_testProducts.SingleOrDefault(p => p.Id == i))).Verifiable();

            var result = await _controller.GetById(nonExistingId);
            result.Should().NotBeNull();
            result.Result.Should().BeAssignableTo<NotFoundResult>();
            result.Value.Should().BeNull();
            _repoMock.Verify();
        }

        [Test]
        public async Task Create_CalledWithValidInput_Will_AddNewProduct_And_ReturnRedirect()
        {
            var newId = 3;
            var newOne = new Product { Name = "Brand New", Description = "Shiny crap" };

            _repoMock.Setup(r => r.AddNewAsync(It.IsAny<Product>())).Returns(Task.CompletedTask).Callback(() => newOne.Id = newId).Verifiable();

            var result = await _controller.Create(newOne);
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<CreatedAtRouteResult>();
            var resultCasted = result as CreatedAtRouteResult;
            resultCasted.RouteName.Should().Be("GetProduct");
            resultCasted.RouteValues.Keys.Should().Contain("id");
            resultCasted.RouteValues["id"].Should().Be(newId);

            resultCasted.Value.Should().Be(newOne);
            ((Product)resultCasted.Value).Id.Should().Be(newId);
            _repoMock.Verify();
        }

        [Test]
        public async Task Delete_CalledforExistingId_Will_DeleteProduct()
        {
            var toDeleteId = 1;
            _testProducts.Select(p => p.Id).Should().Contain(toDeleteId);
            var productToDelete = _testProducts.Single(p => p.Id == toDeleteId);

            _repoMock.Setup(r => r.GetByIdAsync(toDeleteId)).ReturnsAsync(productToDelete).Verifiable();
            _repoMock.Setup(r => r.DeleteAsync(productToDelete)).Returns(Task.CompletedTask).Verifiable();

            var result = await _controller.Delete(toDeleteId);

            result.Should().NotBeNull();
            result.Should().BeAssignableTo<NoContentResult>();
            _repoMock.Verify();
        }

        [Test]
        public async Task Delete_CalledforNonExistingId_Will_ReturnNotFound()
        {
            var toDeleteId = 234;
            _testProducts.Select(p => p.Id).Should().NotContain(toDeleteId);

            _repoMock.Setup(r => r.GetByIdAsync(toDeleteId)).Returns(Task.FromResult<Product>(null)).Verifiable();

            var result = await _controller.Delete(toDeleteId);

            result.Should().NotBeNull();
            result.Should().BeAssignableTo<NotFoundResult>();
            _repoMock.Verify();
            _repoMock.Verify(r => r.DeleteAsync(It.IsAny<Product>()), Times.Never);
        }

        [Test]
        public async Task Update_CalledforExistingId_Will_UpdateProduct()
        {
            var toUpdateId = 0;
            var updated = new Product { Name = "New and Shiny", Description = "To rake the forests!" };
            var product = _testProducts.Single(p => p.Id == toUpdateId);
            _repoMock.Setup(r => r.GetByIdAsync(toUpdateId)).ReturnsAsync(product).Verifiable();

            var result = await _controller.Update(toUpdateId, updated);

            result.Should().NotBeNull();
            result.Should().BeAssignableTo<NoContentResult>();
            product.Name.Should().Be(updated.Name);
            product.Description.Should().Be(updated.Description);
            _repoMock.Verify();
        }

        [Test]
        public async Task Update_CalledforNotExistingId_Will_RetrunNotFound()
        {
            var toUpdateId = 456;
            var updated = new Product { Name = "New and Shiny", Description = "To rake the forests!" };

            _repoMock.Setup(r => r.GetByIdAsync(toUpdateId)).Returns(Task.FromResult<Product>(null)).Verifiable();

            var result = await _controller.Update(toUpdateId, updated);

            result.Should().NotBeNull();
            result.Should().BeAssignableTo<NotFoundResult>();
            _repoMock.Verify();
            _repoMock.Verify(r => r.UpdateAsync(It.IsAny<Product>()), Times.Never);
        }
    }
}
