using Microsoft.EntityFrameworkCore;
using TaskManagementApp.Data;

namespace TaskManagementApp.Services
{
    public interface IAuthorizationService
    {
        Task<bool> IsProjectMemberAsync(int projectId, string userId);
        Task<bool> CanModifyTaskAsync(int taskId, string userId, bool isAdmin = false);
        Task<bool> IsTaskCompletedAsync(int taskId);
    }

    public class AuthorizationService : IAuthorizationService
    {
        private readonly TaskManagementDbContext _dbContext;

        public AuthorizationService(TaskManagementDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> IsProjectMemberAsync(int projectId, string userId)
        {
            var user = await _dbContext.Users
                .FirstOrDefaultAsync(u => u.Email == userId && u.ProjectId == projectId);
            return user != null;
        }

        public async Task<bool> CanModifyTaskAsync(int taskId, string userId, bool isAdmin = false)
        {
            if (isAdmin)
                return true;

            var task = await _dbContext.Tasks
                .Include(t => t.Project)
                .FirstOrDefaultAsync(t => t.Id == taskId);

            if (task == null)
                return false;

            // Check if user is project creator or task creator or assignee
            var isCreator = task.CreatedBy == userId;
            var isProjectCreator = task.Project.CreatedBy == userId;
            var isAssignee = false;

            if (task.AssignedTo != null)
            {
                isAssignee = task.AssignedTo.Email == userId;
            }

            return isCreator || isProjectCreator || isAssignee;
        }

        public async Task<bool> IsTaskCompletedAsync(int taskId)
        {
            var task = await _dbContext.Tasks.FindAsync(taskId);
            return task != null && task.Status == 2; // Assuming 2 = Completed
        }
    }
}
