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
    public class LessonController : BaseController
    {
        public LessonController(UnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

        [HttpGet]
        public IActionResult GetAllLesson()
        {
            var lesson = _unitOfWork.LessonRepository.GetAll<LessonDto>();
            return Ok(lesson);
        }

        [HttpGet("{id}")]
        public IActionResult GetLesson(Guid id)
        {
            var lesson = _unitOfWork.LessonRepository.GetById<LessonDto>(id);

            if (lesson == null)
                return StatusCode(StatusCodes.Status404NotFound,
                    new Response { Status = "Thất bại", Message = "ID không tồn tại. Vui lòng kiểm tra lại" });

            return Ok(lesson);
        }

        [HttpPost]
        public IActionResult CreateLesson(LessonDto? lessonModel)
        {
            try
            {
                if (lessonModel == null)
                    return StatusCode(StatusCodes.Status400BadRequest,
                        new Response { Status = "Thất bại", Message = "Gửi request thất bại!" });

                var lessonEntity = _unitOfWork.Mapper.Map<Lesson>(lessonModel);
                if (lessonEntity != null)
                {
                    _unitOfWork.LessonRepository.Add(lessonEntity);
                    _unitOfWork.Commits();

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
        public IActionResult UpdateLesson(Guid id, LessonDto updatedLessonModel)
        {
            try
            {
                var existingLessonEntity = _unitOfWork.LessonRepository.GetById<Lesson>(id);

                // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
                if (existingLessonEntity == null)
                    return StatusCode(StatusCodes.Status404NotFound,
                        new Response { Status = "Thất bại", Message = "ID không tồn tại. Vui lòng kiểm tra lại" });

                existingLessonEntity.LessonName = updatedLessonModel.LessonName;

                _unitOfWork.LessonRepository.Update(existingLessonEntity);

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
        public IActionResult DeleteLesson(Guid id)
        {
            var lessonEntity = _unitOfWork.LessonRepository.GetById<LessonDto>(id);

            if (lessonEntity == null)
                return NotFound();

            _unitOfWork.LessonRepository.Delete(id);
            _unitOfWork.Commits();

            return Ok(new { message = "Xóa thành công" });
        }

        [HttpGet("search")]
        public IActionResult SearchLessons([FromQuery] string lessonKey)
        {
            var lessons = _unitOfWork.LessonRepository.Search<LessonDto>(lesson =>
                lesson.LessonName != null &&
                lesson.LessonName.Contains(lessonKey));
            if (lessons == null || !lessons.Any())
            {
                return StatusCode(StatusCodes.Status404NotFound,
                    new Response { Status = "Thất bại", Message = $"Không có Bài Học chứa từ khoá {lessonKey}" });
            }

            return Ok(lessons);
        }
    }
}

