using TaskManagementApp.Models.DomainModels;
using TaskManagementApp.Models.DTOs;

namespace TaskManagementApp.Class
{
    public interface IUserRepository
    {
        Task<BaseUser?> GetUserByIdAsync(int id);
        Task<IEnumerable<BaseUser>> GetAllUsersAsync();
        Task<BaseUser> AddUserAsync(BaseUser user);
        Task<BaseUser?> UpdateUserAsync(int id,BaseUser user);
        Task<BaseUser?> DeleteUserAsync(int id);
    }
}
