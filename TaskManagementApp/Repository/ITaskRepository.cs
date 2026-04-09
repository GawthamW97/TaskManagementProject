using TaskManagementApp.Models.DomainModels;

namespace TaskManagementApp.Repository
{
    public interface ITaskRepository
    {
        Task<BaseTask> AddTaskAsync(BaseTask task);
        Task<IReadOnlyList<BaseTask>> GetAllTasksAsync();
        Task<IReadOnlyList<BaseTask>> GetProjectTasksAsync(int projectId);
        Task<BaseTask?> GetTaskByIdAsync(int id);
        Task<BaseTask?> DeleteTaskAsync(int id);
        Task<BaseTask?> UpdateTaskAsync(int id, BaseTask task);
        Task<TaskComment> AddCommentAsync(TaskComment comment);
        Task<IReadOnlyList<TaskComment>> GetTaskCommentsAsync(int taskId);
        Task<TaskComment?> DeleteCommentAsync(int commentId);
    }
}
