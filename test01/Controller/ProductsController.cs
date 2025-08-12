using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using test01.Authorization;
using test01.Data;
using test01.Filters;

namespace test01.Controller
{
    [ApiController]
    [Route("[controller]")]
    public class ProductsController(ApplicationDbContext dbContext, ILogger<ProductsController> logger) : ControllerBase
    {
        [HttpPost]
        [Route("")]
        [CheckPermission(Permission.Write)]
        public async Task<ActionResult<int>> CreateProduct( Product product)
        {
            product.Id= 0; // Ensure Id is set to 0 for new products
            dbContext.Set<Product>().Add(product);
            await dbContext.SaveChangesAsync();
            return Ok(product.Id);
        }
        [HttpPut]
        [Route("{id:int}")]
        [CheckPermission(Permission.Update)]
        public async Task<ActionResult> UpdateProducte( Product product )
        {
            var existingProduct = await dbContext.Set<Product>().FindAsync(product.Id);
            if (existingProduct == null)
            {
                return NotFound();
            }
            existingProduct.Name = product.Name;
            existingProduct.SKU = product.SKU;
            dbContext.Set<Product>().Update(existingProduct);
            await dbContext.SaveChangesAsync();
            return NoContent();
        }
        [HttpDelete]
        [Route("")]
        [CheckPermission(Permission.Delete)]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            var product = await dbContext.Set<Product>().FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            dbContext.Set<Product>().Remove(product);
            await dbContext.SaveChangesAsync();
            return NoContent();
        }
        [HttpGet]
        [Route("")]
        [LogSensitiveAction]
        [Authorize]
        [CheckPermission(Permission.Read)]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            var user = User.Identity?.Name;
            var userId = ((ClaimsIdentity)User.Identity)?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var products = await dbContext.Set<Product>().ToListAsync();
            return Ok(products);
        }
        [HttpGet]
        [Route("{id:int}")]
        [CheckPermission(Permission.Read)]
        public async Task<ActionResult<Product>> GetProduct(/*[FromRoute(Name ="id")]*/int id)
        {
            logger.LogDebug("Geting Product by Id # ", id);
            logger.LogDebug("Geting Product by Id # {Id}", id);
            var product = await dbContext.Set<Product>().FindAsync(id);
            return product != null ? Ok(product) : NotFound();
        }

    }
}
