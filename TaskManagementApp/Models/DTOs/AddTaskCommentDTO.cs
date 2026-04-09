using System.ComponentModel.DataAnnotations;

namespace TaskManagementApp.Models.DTOs
{
    public class AddTaskCommentDTO
    {
        [Required]
        [MaxLength(500)]
        public string Content { get; set; }

        [Required]
        public string CreatedBy { get; set; }

        [Required]
        public int TaskId { get; set; }
    }
}
