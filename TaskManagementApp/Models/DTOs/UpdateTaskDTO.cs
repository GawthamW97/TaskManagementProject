using System.ComponentModel.DataAnnotations;

namespace TaskManagementApp.Models.DTOs
{
    public class UpdateTaskDTO
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        [Required]
        [MaxLength(1000)]
        public string Description { get; set; }
        [Required]
        public string UpdatedBy { get; set; }
        [Required]
        public DateTime DueDate { get; set; }
        [Required]
        public int Status { get; set; }
        public string? Comments { get; set; }
        [Required]
        public int ProjectId { get; set; }
    }
}
