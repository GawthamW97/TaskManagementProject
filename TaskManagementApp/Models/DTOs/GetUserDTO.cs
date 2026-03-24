using System.ComponentModel.DataAnnotations;

namespace TaskManagementApp.Models.DTOs
{
    public class GetUserDTO
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string FullName { get; set; }
        [Required]
        public string Email { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public string UpdatedBy { get; set; }
        [Required]
        public string Username { get; set; }
        public string Role { get; set; }
    }
}
