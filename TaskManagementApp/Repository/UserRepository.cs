using Microsoft.EntityFrameworkCore;
using TaskManagementApp.Class;
using TaskManagementApp.Data;
using TaskManagementApp.Models.DomainModels;

namespace TaskManagementApp.Repository
{
    public class UserRepository : IUserRepository
    {
        public UserRepository(TaskManagementDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public TaskManagementDbContext dbContext { get; }

        public async Task<BaseUser> AddUserAsync(BaseUser user)
        {
            user.UpdatedOn = DateTime.Now;
            user.CreatedOn = DateTime.Now;
            await dbContext.Users.AddAsync(user);
            dbContext.SaveChanges();
            return user;
        }
        public async Task<IEnumerable<BaseUser>> GetAllUsersAsync()
        {
            var users = await dbContext.Users.ToListAsync();
            return users;
        }

        public async Task<BaseUser?> GetUserByIdAsync(int id)
        {
            var user = await dbContext.Users.FirstOrDefaultAsync(x => x.Id == id);
            if (user == null) 
            {
                return null;
            }
            return user;
        }

        public async Task<BaseUser?> DeleteUserAsync(int id)
        {
            var user = await dbContext.Users.FirstOrDefaultAsync(x => x.Id == id);
            if (user == null)
            {
                return null;
            }
            dbContext.Users.Remove(user);
            return user;
        }

        public async Task<BaseUser?> UpdateUserAsync(int id,BaseUser user)
        {
            var existingUser = await dbContext.Users.FirstOrDefaultAsync(x => x.Id == id);
            if (existingUser == null)
            {
                return null;
            }
            existingUser.FullName = user.FullName;
            existingUser.Email = user.Email;
            existingUser.UpdatedBy = user.UpdatedBy;
            existingUser.UpdatedOn = DateTime.Now;
            existingUser.Username = user.Username;
            existingUser.Role = user.Role;
            dbContext.Users.Update(existingUser);
            await dbContext.SaveChangesAsync();
            return existingUser;
        }
    }
}
