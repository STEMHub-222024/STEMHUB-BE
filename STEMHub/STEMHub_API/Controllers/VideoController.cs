using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using STEMHub.STEMHub_Data.Entities;
using STEMHub.STEMHub_Service.Constants;
using STEMHub.STEMHub_Service.DTO;
using STEMHub.STEMHub_Service;

namespace STEMHub.STEMHub_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VideoController : BaseController
    {
        public VideoController(UnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

        [HttpGet]
        public async Task<IActionResult> GetAllVideo()
        {
            var video = await _unitOfWork.VideoRepository.GetAllAsync<VideoDto>();
            return Ok(video);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetVideo(Guid id)
        {
            var video = await _unitOfWork.VideoRepository.GetByIdAsync<VideoDto>(id);

            if (video == null)
                return StatusCode(StatusCodes.Status404NotFound,
                    new Response { Status = "Thất bại", Message = "ID không tồn tại. Vui lòng kiểm tra lại" });

            return Ok(video);
        }

        [HttpPost]
        public async Task<IActionResult> CreateVideo(VideoDto? videoModel)
        {
            try
            {
                if (videoModel == null)
                    return StatusCode(StatusCodes.Status400BadRequest,
                        new Response { Status = "Thất bại", Message = "Gửi request thất bại!" });

                var videoEntity = _unitOfWork.Mapper.Map<Video>(videoModel);
                if (videoEntity != null)
                {
                    await _unitOfWork.VideoRepository.AddAsync(videoEntity);
                    await _unitOfWork.CommitAsync();

                    var videoDto = _unitOfWork.Mapper.Map<VideoDto>(videoEntity);

                    return CreatedAtAction(nameof(GetVideo), new { id = videoDto!.VideoId }, videoDto);
                }
            }
            catch (Exception e)
            {
                return StatusCode(500, "Internal Server Error");
            }

            return StatusCode(StatusCodes.Status500InternalServerError,
                new Response { Status = "Thất bại", Message = "Không thể tạo video!" });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateVideo(Guid id, VideoDto updatedVideoModel)
        {
            try
            {
                var existingVideoEntity =await _unitOfWork.VideoRepository.GetByIdAsync<Video>(id);

                // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
                if (existingVideoEntity == null)
                    return StatusCode(StatusCodes.Status404NotFound,
                        new Response { Status = "Thất bại", Message = "ID không tồn tại. Vui lòng kiểm tra lại" });

                existingVideoEntity.VideoTitle = updatedVideoModel.VideoTitle;
                existingVideoEntity.Path = updatedVideoModel.Path;

                await _unitOfWork.VideoRepository.UpdateAsync(existingVideoEntity);
                await _unitOfWork.CommitAsync();

                return Ok(new { message = "Cập nhật thành công" });
            }
            catch (Exception e)
            {
                //if (_uniqueConstraintHandler.IsUniqueConstraintViolation(e))
                //{
                //    Log.Error(e, "Vi phạm trùng lặp!");
                //    return BadRequest(new { ErrorMessage = "Vi phạm trùng lặp!", ErrorCode = "DUPLICATE_KEY" });
                //}
                //else
                //{
                return StatusCode(500, "Internal Server Error");
                //}
            }

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVideo(Guid id)
        {
            var videoEntity = await _unitOfWork.VideoRepository.GetByIdAsync<VideoDto>(id);

            if (videoEntity == null)
                return NotFound();

            await _unitOfWork.VideoRepository.DeleteAsync(id);
            await _unitOfWork.CommitAsync();

            return Ok(new { message = "Xóa thành công" });
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchVideos([FromQuery] string videoKey)
        {
            var videos = await _unitOfWork.VideoRepository.SearchAsync<VideoDto>(video =>
                video.VideoTitle != null &&
                video.VideoTitle.Contains(videoKey));
            if (!videos.Any())
            {
                return StatusCode(StatusCodes.Status404NotFound,
                    new Response { Status = "Thất bại", Message = $"Không có Video chứa từ khoá {videoKey}" });
            }
            return Ok(videos);
        }
    }
}

