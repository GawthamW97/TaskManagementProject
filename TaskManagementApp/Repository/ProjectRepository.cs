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
            return existingProject;
        }

        public async Task<IReadOnlyList<BaseProject>> GetAllProjectsAsync()
        {
            var projects = await dbContext.Projects.ToListAsync();
            return projects;
        }

        public async Task<BaseProject?> GetProjectByIdAsync(int id)
        {
            var project = await dbContext.Projects.FirstOrDefaultAsync(p => p.Id == id);
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
    }
}
