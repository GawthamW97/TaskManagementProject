using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskManagementApp.Models.DomainModels;
using TaskManagementApp.Models.DTOs;
using TaskManagementApp.Repository;

namespace TaskManagementApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly IImageUploadRepository image;

        public ImageController(IImageUploadRepository image)
        {
            this.image = image;
        }
        [HttpPost]
        public async Task<IActionResult> UploadImage([FromForm] AddImageRequestDTO requestDTO)
        {
            ValidateFileUpload(requestDTO);
            if (ModelState.IsValid)
            {
                var imageDomainModel = new ImageUpload
                {
                    File = requestDTO.File,
                    FileExtension = Path.GetExtension(requestDTO.File.FileName),
                    FileSize = requestDTO.File.Length,
                    FileName = requestDTO.File.FileName,
                    FileDescription = requestDTO.FileDescription,
                };

                await image.UploadImage(imageDomainModel);
                return Ok(imageDomainModel);
            }
            return BadRequest(ModelState);
        }

        private void ValidateFileUpload(AddImageRequestDTO requestDTO)
        {
            if (requestDTO.File == null || requestDTO.File.Length == 0)
            {
                throw new ArgumentException("File is required.");
            }
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            var fileExtension = Path.GetExtension(requestDTO.File.FileName).ToLower();
            if (!allowedExtensions.Contains(fileExtension))
            {
                ModelState.AddModelError("File", "Invalid file type. Only JPG, JPEG, PNG, and GIF are allowed.");
            }

            if (requestDTO.File.Length > 5 * 1024 * 1024) // 5 MB limit
            {
                ModelState.AddModelError("File", "File size exceeds the 5 MB limit.");
            }

        }
    }
}
