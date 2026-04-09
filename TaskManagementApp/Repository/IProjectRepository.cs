using TaskManagementApp.Models.DomainModels;

namespace TaskManagementApp.Repository
{
    public interface IProjectRepository
    {
        Task<IReadOnlyList<BaseProject>> GetAllProjectsAsync();
        Task<IReadOnlyList<BaseProject>> GetUserProjectsAsync(string userId);
        Task<BaseProject?> GetProjectByIdAsync(int id);
        Task<BaseProject> AddProjectAsync(BaseProject project);
        Task<BaseProject?> UpdateProjectAsync(int id, BaseProject project);
        Task<BaseProject?> DeleteProjectAsync(int id);
        Task<bool> AddProjectMemberAsync(int projectId, int userId);
        Task<bool> RemoveProjectMemberAsync(int projectId, int userId);
        Task<IReadOnlyList<BaseUser>> GetProjectMembersAsync(int projectId);
        Task<bool> IsProjectMemberAsync(int projectId, int userId);
    }
}
