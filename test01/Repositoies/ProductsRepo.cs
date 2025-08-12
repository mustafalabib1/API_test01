using Microsoft.EntityFrameworkCore;
using test01.Data;

namespace test01.Repositoies
{
    public class ProductsRepo(ApplicationDbContext dbContext)
    {
        public async Task<Product?> GetProductByIdAsync(int productId)
        {
            return await dbContext.Set<Product>().FindAsync(productId);
        }
        public async Task<Product?> GetProductBySKUAsync(string sku)
        {
            return await dbContext.Set<Product>().FirstOrDefaultAsync(p => p.SKU == sku);
        }
        public async Task<ICollection<Product>> GetAllProductsAsync()
        {
            return await dbContext.Set<Product>().ToListAsync();
        }
        public async Task<Product> CreateProductAsync(Product product)
        {
            dbContext.Set<Product>().Add(product);
            await dbContext.SaveChangesAsync();
            return product;
        }
        public async Task<Product> UpdateProductAsync(Product product)
        {
            dbContext.Set<Product>().Update(product);
            await dbContext.SaveChangesAsync();
            return product;
        }
        public async Task DeleteProductAsync(int productId)
        {
            var product = await GetProductByIdAsync(productId);
            if (product != null)
            {
                dbContext.Set<Product>().Remove(product);
                await dbContext.SaveChangesAsync();
            }
        }

    }
}
