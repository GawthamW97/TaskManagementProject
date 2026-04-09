namespace TaskManagementApp.Models.DomainModels
{
    public class BaseTask
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime DueDate { get; set; }
        public int Status { get; set; }
        public int ProjectId { get; set; }
        public int? AssignedToId { get; set; }
        public BaseUser AssignedTo { get; set; }
        public Project Project { get; set; }
        public List<TaskComment> Comments { get; set; } = new List<TaskComment>();
    }
}
