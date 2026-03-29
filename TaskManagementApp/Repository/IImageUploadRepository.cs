using TaskManagementApp.Models.DomainModels;

namespace TaskManagementApp.Repository
{
    public interface IImageUploadRepository
    {
        Task<ImageUpload> UploadImage(ImageUpload image);
        Task<ImageUpload?> DeleteImage(int id);
        Task<ImageUpload?> GetImage(int id);
    }
}
