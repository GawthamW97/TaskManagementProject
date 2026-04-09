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
            task.CreatedDate = DateTime.UtcNow;
            task.UpdatedDate = DateTime.UtcNow;
            await dbContext.Tasks.AddAsync(task);
            await dbContext.SaveChangesAsync();
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
            var tasks = await dbContext.Tasks
                .Include(t => t.Comments)
                .Include(t => t.AssignedTo)
                .ToListAsync();
            return tasks;
        }

        public async Task<IReadOnlyList<BaseTask>> GetProjectTasksAsync(int projectId)
        {
            var tasks = await dbContext.Tasks
                .Where(t => t.ProjectId == projectId)
                .Include(t => t.Comments)
                .Include(t => t.AssignedTo)
                .ToListAsync();
            return tasks;
        }

        public async Task<BaseTask?> GetTaskByIdAsync(int id)
        {
            var task = await dbContext.Tasks
                .Include(t => t.Comments)
                .Include(t => t.AssignedTo)
                .FirstOrDefaultAsync(t => t.Id == id);
            if (task == null)
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
            existingTask.ProjectId = task.ProjectId;
            existingTask.AssignedToId = task.AssignedToId;

            dbContext.Tasks.Update(existingTask);
            await dbContext.SaveChangesAsync();
            return existingTask;
        }

        public async Task<TaskComment> AddCommentAsync(TaskComment comment)
        {
            comment.CreatedDate = DateTime.UtcNow;
            await dbContext.TaskComments.AddAsync(comment);
            await dbContext.SaveChangesAsync();
            return comment;
        }

        public async Task<IReadOnlyList<TaskComment>> GetTaskCommentsAsync(int taskId)
        {
            var comments = await dbContext.TaskComments
                .Where(c => c.TaskId == taskId)
                .OrderByDescending(c => c.CreatedDate)
                .ToListAsync();
            return comments;
        }

        public async Task<TaskComment?> DeleteCommentAsync(int commentId)
        {
            var comment = await dbContext.TaskComments.FindAsync(commentId);
            if (comment == null)
            {
                return null;
            }
            dbContext.TaskComments.Remove(comment);
            await dbContext.SaveChangesAsync();
            return comment;
        }
    }
}
