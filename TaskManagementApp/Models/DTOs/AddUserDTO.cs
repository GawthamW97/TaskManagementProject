using System.ComponentModel.DataAnnotations;

namespace TaskManagementApp.Models.DTOs
{
    public class AddUserDTO
    {
        [Required]
        [MaxLength(100)]
        public string FullName { get; set; }
        [Required]
        [MaxLength(100)]
        public string Email { get; set; }
        [Required]
        public string CreatedBy { get; set; }
        [Required]
        public string UpdatedBy { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Role { get; set; }
    }
}
