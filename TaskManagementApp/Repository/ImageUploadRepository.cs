using TaskManagementApp.Data;
using TaskManagementApp.Models.DomainModels;

namespace TaskManagementApp.Repository
{
    public class ImageUploadRepository : IImageUploadRepository
    {
        private readonly IWebHostEnvironment webHost;
        private readonly IHttpContextAccessor contextAccessor;
        private readonly TaskManagementDbContext dbContext;

        public ImageUploadRepository(IWebHostEnvironment webHost,
            IHttpContextAccessor contextAccessor,TaskManagementDbContext dbContext)
        {
            this.webHost = webHost;
            this.contextAccessor = contextAccessor;
            this.dbContext = dbContext;
        }
        public Task<ImageUpload?> DeleteImage(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ImageUpload?> GetImage(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<ImageUpload> UploadImage(ImageUpload image)
        {
            var localFilePath = Path.Combine(webHost.ContentRootPath, "Images",$"{image.FileName}{image.FileExtension}");
            // upload image to local path
            using var stream = new FileStream(localFilePath, FileMode.Create);
            await image.File.CopyToAsync(stream);

            var urlFilePath = $"{contextAccessor.HttpContext.Request.Scheme}://{contextAccessor.HttpContext.Request.Host}{contextAccessor.HttpContext.Request.PathBase}/Images/{image.FileName}{image.FileExtension}";
            image.FilePath = urlFilePath;

            //add the image to images table
            await dbContext.Images.AddAsync(image);
            await dbContext.SaveChangesAsync();
            return image;
        }
    }
}
