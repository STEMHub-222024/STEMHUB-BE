using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;

namespace STEMHub.STEMHub_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadImageController : ControllerBase
    {
        private readonly string _imageDirectory;

        public UploadImageController()
        {
            _imageDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Image");
        }

        [HttpGet]
        public IActionResult GetAllImages()
        {
            try
            {
                var imageFiles = Directory.GetFiles(_imageDirectory, "*.*");
                var imageList = new List<string>();

                foreach (var imageFile in imageFiles)
                {
                    var imageFileName = Path.GetFileName(imageFile);
                    imageList.Add(imageFileName);
                }

                return Ok(imageList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi máy chủ nội bộ: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetImageById(string id)
        {
            try
            {
                var imageFilePath = Path.Combine(_imageDirectory, id);

                if (!System.IO.File.Exists(imageFilePath))
                {
                    return NotFound("Ảnh không được tìm thấy");
                }

                var imageBytes = System.IO.File.ReadAllBytes(imageFilePath);

                var contentType = "application/octet-stream";
                if (id.EndsWith(".jpg") || id.EndsWith(".jpeg"))
                {
                    contentType = "image/jpeg";
                }
                else if (id.EndsWith(".png"))
                {
                    contentType = "image/png";
                }

                return File(imageBytes, contentType);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi máy chủ nội bộ: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    return BadRequest("Không có tệp nào được cung cấp.");
                }

                string uploadsDirectory = Path.Combine(_imageDirectory);

                if (!Directory.Exists(uploadsDirectory))
                {
                    Directory.CreateDirectory(uploadsDirectory);
                }

                string uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
                string filePath = Path.Combine(uploadsDirectory, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }

                var fileSizeInMBs = file.Length / (1024.0 * 1024);
                var fileUrl = $"{Request.Scheme}://{Request.Host}/api/uploadimage/{uniqueFileName}";

                return Ok(new { FileSizeInMBs = fileSizeInMBs, FileUrl = fileUrl });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi máy chủ nội bộ: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteImage(string id)
        {
            try
            {
                var imageFilePath = Path.Combine(_imageDirectory, id);

                if (!System.IO.File.Exists(imageFilePath))
                {
                    return NotFound("Ảnh không được tìm thấy");
                }

                System.IO.File.Delete(imageFilePath);

                return Ok("Ảnh đã được xóa thành công");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi máy chủ nội bộ: {ex.Message}");
            }
        }
    }
}
