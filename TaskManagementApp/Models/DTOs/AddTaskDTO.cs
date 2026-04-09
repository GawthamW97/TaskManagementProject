using System.ComponentModel.DataAnnotations;
using TaskManagementApp.Models.DomainModels;

namespace TaskManagementApp.Models.DTOs
{
    public class AddTaskDTO
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        [Required]
        [MaxLength(1000)]
        public string Description { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime DueDate { get; set; }
        [Required]
        public int Status { get; set; }
        [Required]
        public int ProjectId { get; set; }
        public int? AssignedToId { get; set; }
    }
}
