using TaskManagementApp.Models.DomainModels;

namespace TaskManagementApp.Repository
{
    public interface ITaskRepository
    {
            Task<BaseTask> AddTaskAsync(BaseTask task);
            Task<IReadOnlyList<BaseTask>> GetAllTasksAsync();
            Task<BaseTask?> GetTaskByIdAsync(int id);
            Task<BaseTask?> DeleteTaskAsync(int id);
            Task<BaseTask?> UpdateTaskAsync(int id, BaseTask task);
    }
}
