namespace TaskManagementApp.Models.DomainModels
{
    public class TaskComment
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public int TaskId { get; set; }
        public BaseTask Task { get; set; }
    }
}
