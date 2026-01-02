using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalksAPI.Models.Domain;
using NZWalksAPI.Models.DTO;
using NZWalksAPI.Repositories;

namespace NZWalksAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IImageRepository imageRepository;

        public ImagesController(IImageRepository imageRepository)
        {
            this.imageRepository = imageRepository;
        }


        [HttpPost]
        [Route("Upload")]
        public async Task<IActionResult> Upload([FromForm] ImageUploadRequestDTO request)
        {
            ValidateFileUpload(request);

            if(ModelState.IsValid)
            {
                var imageDomainModel = new Image
                {
                    FileName = request.FileName,
                    FileExtension = Path.GetExtension(request.File.FileName),
                    FileSizeInBytes = request.File.Length,
                    File = request.File,
                    FileDescription = request.FileDescription
                };

                 await imageRepository.Upload(imageDomainModel);

                return Ok(imageDomainModel);


            }

            return BadRequest(ModelState);
        }

        private void ValidateFileUpload(ImageUploadRequestDTO request)
        {
            var allowedExtensions = new string[] { ".jpg", ".jpeg", ".png", ".gif" };
            var maxFileSizeInBytes = 10 * 1024 * 1024; // 10 MB
            var fileExtension = Path.GetExtension(request.File.FileName).ToLower();
            if (allowedExtensions.Contains(fileExtension) == false)
            {
                ModelState.AddModelError("File", "Unsupported file format. Allowed formats are .jpg, .jpeg, .png, .gif.");

            }
            if (request.File.Length > maxFileSizeInBytes)
            {
               ModelState.AddModelError("File", "File size exceeds the maximum limit of 10 MB.");
            }
        }
    }
}
