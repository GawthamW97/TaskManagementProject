using Microsoft.EntityFrameworkCore;
using TaskManagementApp.Data;
using TaskManagementApp.Models.DomainModels;

namespace TaskManagementApp.Repository
{
    public class TaskRepository : ITaskRepository
    {
        private readonly TaskManagementDbContext dbContext;

        public TaskRepository(TaskManagementDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<BaseTask> AddTaskAsync(BaseTask task)
        {
            await dbContext.Tasks.AddAsync(task);
            dbContext.SaveChanges();
            return task;
        }

        public async Task<BaseTask?> DeleteTaskAsync(int id)
        {
            var task = await dbContext.Tasks.FindAsync(id);
            if (task == null)
            {
                return null;
            }
            dbContext.Tasks.Remove(task);
            await dbContext.SaveChangesAsync();
            return task;
        }

        public async Task<IReadOnlyList<BaseTask>> GetAllTasksAsync()
        {
            var tasks = await dbContext.Tasks.ToListAsync();
            return tasks;
        }

        public async Task<BaseTask?> GetTaskByIdAsync(int id)
        {
            var task = await dbContext.Tasks.FindAsync(id);
            if ( task == null)
            {
                return null;
            }
            return task;
        }

        public async Task<BaseTask?> UpdateTaskAsync(int id, BaseTask task)
        {
            var existingTask = await dbContext.Tasks.FindAsync(id);
            if (existingTask == null)
            {
                return null;
            }
            existingTask.Name = task.Name;
            existingTask.Description = task.Description;
            existingTask.UpdatedDate = DateTime.UtcNow;
            existingTask.UpdatedBy = task.UpdatedBy;
            existingTask.DueDate = task.DueDate;
            existingTask.Status = task.Status;
            existingTask.Comments = task.Comments;
            existingTask.ProjectId = task.ProjectId;
            existingTask.AssignedTo = task.AssignedTo;

            dbContext.Tasks.Update(existingTask);
            await dbContext.SaveChangesAsync();
            return existingTask;
        }
    }
}
