using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManagementApp.Models.DomainModels
{
    public class ImageUpload
    {
        public Guid Id { get; set; }
        [NotMapped]
        public IFormFile File { get; set; }
        public string FileName { get; set; }
        public string? FileDescription { get; set; }
        public string FileExtension { get; set; }
        public long FileSize{ get; set; }
        public string FilePath { get; set; }
        public DateTime UploadedAt { get; set; }
    }
}
