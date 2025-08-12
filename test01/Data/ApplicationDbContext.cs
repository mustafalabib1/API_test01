using Microsoft.EntityFrameworkCore;
using test01.Data;

namespace test01.Data
{
    public class ApplicationDbContext :DbContext
    {
        public ApplicationDbContext(DbContextOptions options):base(options)
        {
            
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Product>().ToTable("Products");
            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<UserPermission>()
                .ToTable("UserPermissions")
                .HasKey(up => new { up.UserId, up.PermissionId });
        }
    }
}
