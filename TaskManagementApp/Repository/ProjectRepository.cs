using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TaskManagementApp.Data;
using TaskManagementApp.Models.DomainModels;

namespace TaskManagementApp.Repository
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly TaskManagementDbContext dbContext;

        public ProjectRepository(TaskManagementDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<BaseProject> AddProjectAsync(BaseProject project)
        {
            project.CreatedDate = DateTime.UtcNow;
            project.UpdatedDate = DateTime.UtcNow;
            await dbContext.Projects.AddAsync(project);
            await dbContext.SaveChangesAsync();
            return project;
        }

        public async Task<BaseProject?> DeleteProjectAsync(int id)
        {
            var existingProject = await dbContext.Projects.FirstOrDefaultAsync(p => p.Id == id);
            if (existingProject == null)
            {
                return null;
            }
            dbContext.Projects.Remove(existingProject);
            await dbContext.SaveChangesAsync();
            return existingProject;
        }

        public async Task<IReadOnlyList<BaseProject>> GetAllProjectsAsync()
        {
            var projects = await dbContext.Projects
                .Include(p => p.Users)
                .ToListAsync();
            return projects;
        }

        public async Task<IReadOnlyList<BaseProject>> GetUserProjectsAsync(string userId)
        {
            var projects = await dbContext.Projects
                .Where(p => p.Users.Any(u => u.Email == userId) || p.CreatedBy == userId)
                .Include(p => p.Users)
                .ToListAsync();
            return projects;
        }

        public async Task<BaseProject?> GetProjectByIdAsync(int id)
        {
            var project = await dbContext.Projects
                .Include(p => p.Users)
                .FirstOrDefaultAsync(p => p.Id == id);
            if (project == null)
            {
                return null;
            }
            return project;
        }

        public async Task<BaseProject?> UpdateProjectAsync(int id, BaseProject project)
        {
            var existingProject = await dbContext.Projects.FirstOrDefaultAsync(p => p.Id == id);
            if (existingProject == null)
            {
                return null;
            }
            existingProject.Name = project.Name;
            existingProject.Description = project.Description;
            existingProject.UpdatedDate = DateTime.UtcNow;
            existingProject.UpdatedBy = project.UpdatedBy;

            dbContext.Update(existingProject);
            await dbContext.SaveChangesAsync();
            return existingProject;
        }

        public async Task<bool> AddProjectMemberAsync(int projectId, int userId)
        {
            var project = await dbContext.Projects
                .Include(p => p.Users)
                .FirstOrDefaultAsync(p => p.Id == projectId);

            if (project == null)
                return false;

            var user = await dbContext.Users.FindAsync(userId);
            if (user == null)
                return false;

            // Check if user is already a member
            if (project.Users.Any(u => u.Id == userId))
                return false;

            user.ProjectId = projectId;
            dbContext.Users.Update(user);
            await dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveProjectMemberAsync(int projectId, int userId)
        {
            var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId && u.ProjectId == projectId);
            if (user == null)
                return false;

            user.ProjectId = null;
            dbContext.Users.Update(user);
            await dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<IReadOnlyList<BaseUser>> GetProjectMembersAsync(int projectId)
        {
            var members = await dbContext.Users
                .Where(u => u.ProjectId == projectId)
                .ToListAsync();
            return members;
        }

        public async Task<bool> IsProjectMemberAsync(int projectId, int userId)
        {
            var isMember = await dbContext.Users
                .AnyAsync(u => u.Id == userId && u.ProjectId == projectId);
            return isMember;
        }
    }
}
