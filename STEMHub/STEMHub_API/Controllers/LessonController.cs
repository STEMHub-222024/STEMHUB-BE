using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using STEMHub.STEMHub_Data.Entities;
using STEMHub.STEMHub_Services.Constants;
using STEMHub.STEMHub_Services;
using STEMHub.STEMHub_Data.DTO;
using STEMHub.STEMHub_Services.Services.Service;
using STEMHub.STEMHub_Services.Interfaces;

namespace STEMHub.STEMHub_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LessonController : BaseController
    {
        
        public LessonController(UnitOfWork unitOfWork) : base(unitOfWork)
        {
            
        }

        [HttpGet]
        public async Task<IActionResult> GetAllLesson()
        {
            var lesson =await _unitOfWork.LessonRepository.GetAllAsync<LessonDto>();
            return Ok(lesson);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetLesson(Guid id)
        {
            var lesson = await _unitOfWork.LessonRepository.GetByIdAsync<LessonDto>(id);

            if (lesson == null)
                return StatusCode(StatusCodes.Status404NotFound,
                    new Response { Status = "Thất bại", Message = "ID không tồn tại. Vui lòng kiểm tra lại" });

            return Ok(lesson);
        }

        [HttpPost]
        public async Task<IActionResult> CreateLesson(LessonDto? lessonModel)
        {
            try
            {
                if (lessonModel == null)
                    return StatusCode(StatusCodes.Status400BadRequest,
                        new Response { Status = "Thất bại", Message = "Gửi request thất bại!" });

                var lessonEntity = _unitOfWork.Mapper.Map<Lesson>(lessonModel);
                if (lessonEntity != null)
                {
                    await _unitOfWork.LessonRepository.AddAsync(lessonEntity);
                    await _unitOfWork.CommitAsync();

                    var lessonDto = _unitOfWork.Mapper.Map<LessonDto>(lessonEntity);

                    return CreatedAtAction(nameof(GetLesson), new { id = lessonDto!.LessonId }, lessonDto);
                }
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status400BadRequest,
                    new Response { Status = "Thất bại", Message = "Không có ID chủ đề trùng khớp!" });
            }

            return StatusCode(StatusCodes.Status500InternalServerError,
                new Response { Status = "Thất bại", Message = "Không thể tạo lesson!" });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateLesson(Guid id, LessonDto updatedLessonModel)
        {
            try
            {
                var existingLessonEntity = await _unitOfWork.LessonRepository.GetByIdAsync<Lesson>(id);

                // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
                if (existingLessonEntity == null)
                    return StatusCode(StatusCodes.Status404NotFound,
                        new Response { Status = "Thất bại", Message = "ID không tồn tại. Vui lòng kiểm tra lại" });

                existingLessonEntity.LessonName = updatedLessonModel.LessonName;

                await _unitOfWork.LessonRepository.UpdateAsync(existingLessonEntity);

                await _unitOfWork.CommitAsync();

                return Ok(new { message = "Cập nhật thành công" });
            }
            catch (Exception)
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
        public async Task<IActionResult> DeleteLesson(Guid id)
        {
            var lessonEntity = await _unitOfWork.LessonRepository.GetByIdAsync<LessonDto>(id);

            if (lessonEntity == null)
                return NotFound();

            await _unitOfWork.LessonRepository.DeleteAsync(id);
            await _unitOfWork.CommitAsync();

            return Ok(new { message = "Xóa thành công" });
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchLessons([FromQuery] string lessonKey)
        {
            var lessons =await _unitOfWork.LessonRepository.SearchAsync<LessonDto>(lesson =>
                lesson.LessonName != null &&
                lesson.LessonName.Contains(lessonKey));
            if (!lessons.Any())
            {
                return StatusCode(StatusCodes.Status404NotFound,
                    new Response { Status = "Thất bại", Message = $"Không có Bài Học chứa từ khoá {lessonKey}" });
            }
            return Ok(lessons);
        }
    }
}

