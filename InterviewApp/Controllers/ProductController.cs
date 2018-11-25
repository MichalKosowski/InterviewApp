using System.Collections.Generic;
using System.Threading.Tasks;
using InterviewApp.Core;
using InterviewApp.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace InterviewApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : Controller
    {
        private readonly IProductsRepository _repository;

        public ProductController(IProductsRepository productsContext)
        {
            _repository = productsContext;
        }

        [HttpGet]
        public async Task<ActionResult<List<Product>>> GetAll()
        {
            return await _repository.GetAllAsync();
        }

        [HttpGet("{productId}", Name = "GetProduct")]
        public async Task<ActionResult<Product>> GetById([FromQuery]long id)
        {
            var product = await _repository.GetByIdAsync(id);
            if(product == null)
            {
                return NotFound();
            }
            return product;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody]Product product)
        {
            await _repository.AddNewAsync(product);

            return CreatedAtRoute("GetProduct", new { id = product.Id }, product);
        }

        [HttpPut("{productId}")]
        public async Task<IActionResult> Update(long id, [FromBody]Product item)
        {
            var product = await _repository.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            product.Name = item.Name;
            product.Description = item.Description;

            await _repository.UpdateAsync(product);
            return NoContent();
        }

        [HttpDelete("{productId}")]
        public async Task<IActionResult> Delete(long id)
        {
            var product = await _repository.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            await _repository.DeleteAsync(product);
            return NoContent();
        }
    }
}
