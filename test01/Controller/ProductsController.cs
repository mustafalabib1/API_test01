﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;
using test01.Data;

namespace test01.Controller
{
    [ApiController]
    [Route("[controller]")]
    public class ProductsController: ControllerBase
    {
        private readonly ApplicationDbContext dbContext;

        public ProductsController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpPost]
        [Route("")]
        public async Task<ActionResult<int>> CreateProduct( Product product)
        {
            product.Id= 0; // Ensure Id is set to 0 for new products
            dbContext.Set<Product>().Add(product);
            await dbContext.SaveChangesAsync();
            return Ok(product.Id);
        }
        [HttpPut]
        [Route("{id:int}")]
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
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            var products = await dbContext.Set<Product>().ToListAsync();
            return Ok(products);
        }
        [HttpGet]
        [Route("{id:int}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await dbContext.Set<Product>().FindAsync(id);
            return product != null ? Ok(product) : NotFound();
        }

    }
}
