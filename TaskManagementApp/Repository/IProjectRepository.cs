using TaskManagementApp.Models.DomainModels;

namespace TaskManagementApp.Repository
{
    public interface IProjectRepository
    {
        Task<IReadOnlyList<BaseProject>> GetAllProjectsAsync();
        Task<BaseProject?> GetProjectByIdAsync(int id);
        Task<BaseProject> AddProjectAsync(BaseProject project);
        Task<BaseProject?> UpdateProjectAsync(int id,BaseProject project);
        Task<BaseProject?> DeleteProjectAsync(int id);
    }
}
