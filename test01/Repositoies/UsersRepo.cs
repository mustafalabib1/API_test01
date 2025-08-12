using Microsoft.EntityFrameworkCore;
using test01.Data;

namespace test01.Repositoies
{
    public class UsersRepo(ApplicationDbContext dbContext)
    {
        public async Task<User?> GetUserByIdAsync(int userId)
        {
            return await dbContext.Set<User>().FindAsync(userId);
        }
        public async Task<User?> GetUserByNameAsync(string userName)
        {
            return await dbContext.Set<User>().FirstOrDefaultAsync(u => u.Name == userName);
        }
        public async Task<ICollection<User>> GetAllUsersAsync()
        {
            return await dbContext.Set<User>().ToListAsync();
        }
        public async Task<User> CreateUserAsync(User user)
        {
            dbContext.Set<User>().Add(user);
            await dbContext.SaveChangesAsync();
            return user;
        }
        public async Task<User> UpdateUserAsync(User user)
        {
            dbContext.Set<User>().Update(user);
            await dbContext.SaveChangesAsync();
            return user;
        }
        public async Task DeleteUserAsync(int userId)
        {
            var user = await GetUserByIdAsync(userId);
            if (user != null)
            {
                dbContext.Set<User>().Remove(user);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
