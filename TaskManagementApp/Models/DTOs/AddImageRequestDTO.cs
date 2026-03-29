using System.ComponentModel.DataAnnotations;

namespace TaskManagementApp.Models.DTOs
{
    public class AddImageRequestDTO
    {
        [Required]
        public IFormFile File { get; set; }
        [Required]
        public string FileName { get; set; }
        public string? FileDescription { get; set; }
    }
}
