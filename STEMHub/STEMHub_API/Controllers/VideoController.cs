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
        public IActionResult GetAllVideo()
        {
            var video = _unitOfWork.VideoRepository.GetAll<VideoDto>();
            return Ok(video);
        }

        [HttpGet("{id}")]
        public IActionResult GetVideo(Guid id)
        {
            var video = _unitOfWork.VideoRepository.GetById<VideoDto>(id);

            if (video == null)
                return StatusCode(StatusCodes.Status404NotFound,
                    new Response { Status = "Thất bại", Message = "ID không tồn tại. Vui lòng kiểm tra lại" });

            return Ok(video);
        }

        [HttpPost]
        public IActionResult CreateVideo(VideoDto? videoModel)
        {
            try
            {
                if (videoModel == null)
                    return StatusCode(StatusCodes.Status400BadRequest,
                        new Response { Status = "Thất bại", Message = "Gửi request thất bại!" });

                var videoEntity = _unitOfWork.Mapper.Map<Video>(videoModel);
                if (videoEntity != null)
                {
                    _unitOfWork.VideoRepository.Add(videoEntity);
                    _unitOfWork.Commits();

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
        public IActionResult UpdateVideo(Guid id, VideoDto updatedVideoModel)
        {
            try
            {
                var existingVideoEntity = _unitOfWork.VideoRepository.GetById<Video>(id);

                // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
                if (existingVideoEntity == null)
                    return StatusCode(StatusCodes.Status404NotFound,
                        new Response { Status = "Thất bại", Message = "ID không tồn tại. Vui lòng kiểm tra lại" });

                existingVideoEntity.VideoTitle = updatedVideoModel.VideoTitle;
                existingVideoEntity.Path = updatedVideoModel.Path;

                _unitOfWork.VideoRepository.Update(existingVideoEntity);

                _unitOfWork.Commits();

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
        public IActionResult DeleteVideo(Guid id)
        {
            var videoEntity = _unitOfWork.VideoRepository.GetById<VideoDto>(id);

            if (videoEntity == null)
                return NotFound();

            _unitOfWork.VideoRepository.Delete(id);
            _unitOfWork.Commits();

            return Ok(new { message = "Xóa thành công" });
        }

        [HttpGet("search")]
        public IActionResult SearchVideos([FromQuery] string videoKey)
        {
            var videos = _unitOfWork.VideoRepository.Search<VideoDto>(video =>
                video.VideoTitle != null &&
                video.VideoTitle.Contains(videoKey));
            if (videos == null || !videos.Any())
            {
                return StatusCode(StatusCodes.Status404NotFound,
                    new Response { Status = "Thất bại", Message = $"Không có Video chứa từ khoá {videoKey}" });
            }
            return Ok(videos);
        }
    }
}

