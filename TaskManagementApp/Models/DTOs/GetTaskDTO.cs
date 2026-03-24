using TaskManagementApp.Models.DomainModels;

namespace TaskManagementApp.Models.DTOs
{
    public class GetTaskDTO
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
        public string? Comments { get; set; }
        public int ProjectId { get; set; }
    }
}
