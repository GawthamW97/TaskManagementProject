using System.ComponentModel.DataAnnotations;
using TaskManagementApp.Models.DomainModels;

namespace TaskManagementApp.Models.DTOs
{
    public class AddProjectDTO
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        [Required]
        [MaxLength(10000)]
        public string Description { get; set; }
        [Required]
        public string CreatedBy { get; set; }
        [Required]
        public string UpdatedBy { get; set; }
    }
}
