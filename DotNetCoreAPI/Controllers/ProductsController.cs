using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using DotNetCoreAPI.Models;
using DotNetCoreAPI.Data;

namespace DotNetCoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;

        public ProductsController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }        

        [HttpGet]
        [SwaggerResponse(200, "Success", typeof(IEnumerable<Product>))]
        public IEnumerable<Product> GetProducts()
        {
            return _dbContext.Products.ToList();
        }

        [HttpGet("{id}", Name = "GetProduct")]
        [SwaggerResponse(200, "Success", typeof(Product))]
        [SwaggerResponse(404, "Not Found")]
        public ActionResult<Product> GetProduct(int id)
        {
            var product = _dbContext.Products.SingleOrDefault(p => p.ProductID == id);
            if (product == null) return NotFound();

            return product;
        }

        [HttpPost]
        [SwaggerResponse(201, "Created", typeof(Product))]
        [SwaggerResponse(400, "Bad Request")]
        public ActionResult<Product> CreateProduct([FromBody] Product product)
        {
            _dbContext.Products.Add(product);
            _dbContext.SaveChanges();

            return CreatedAtRoute("GetProduct", new { id = product.ProductID }, product);
        }

        [HttpPut("{id}")]
        [SwaggerResponse(204, "No Content")]
        [SwaggerResponse(400, "Bad Request")]
        [SwaggerResponse(404, "Not Found")]
        public IActionResult UpdateProduct(int id, [FromBody] Product product)
        {
            var productInDb = _dbContext.Products.SingleOrDefault(p => p.ProductID == id);
            if (productInDb == null) return NotFound();

            productInDb.Description = product.Description;
            productInDb.Quantity = product.Quantity;

            _dbContext.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id}")]
        [SwaggerResponse(204, "No Content")]
        [SwaggerResponse(404, "Not Found")]
        public IActionResult DeleteProduct(int id)
        {
            var product = _dbContext.Products.SingleOrDefault(p => p.ProductID == id);
            if (product == null) return NotFound();

            _dbContext.Remove(product);
            _dbContext.SaveChanges();

            return NoContent();
        }
    }
}