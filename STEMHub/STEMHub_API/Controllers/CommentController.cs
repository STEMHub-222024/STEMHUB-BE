using Microsoft.AspNetCore.Mvc;
using STEMHub.STEMHub_Data.Entities;
using STEMHub.STEMHub_Services.Constants;
using STEMHub.STEMHub_Services;
using STEMHub.STEMHub_Data.DTO;
using STEMHub.STEMHub_Services.Interfaces;
using STEMHub.STEMHub_Services.Repository;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace STEMHub.STEMHub_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : BaseController
    {
        private readonly IGetAllCommentByLessonId _igetAllCommentByLessonId;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CommentController(UnitOfWork unitOfWork, IGetAllCommentByLessonId igetAllCommentByLessonId, IHttpContextAccessor httpContextAccessor) : base(unitOfWork)
        {
            _igetAllCommentByLessonId = igetAllCommentByLessonId;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllComment(Guid lessonId)
        {
            var comments = await _unitOfWork.GetAllCommentByLessonId.GetAllCommentByLessonID(lessonId);

            if (comments == null || !comments.Any())
            {
                return NotFound("Không tìm thấy bình luận nào cho bài học này.");
            }
            return Ok(comments);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetComment(Guid id)
        {
            var comment = await _unitOfWork.CommentRepository.GetByIdAsync<CommentDto>(id);

            if (comment == null)
                return StatusCode(StatusCodes.Status404NotFound,
                    new Response { Status = "Thất bại", Message = "ID không tồn tại. Vui lòng kiểm tra lại" });

            return Ok(comment);
        }

        [HttpPost]
        public async Task<IActionResult> CreateComment(CommentDto commentModel)
        {
            try
            {
                if (commentModel == null)
                    return StatusCode(StatusCodes.Status400BadRequest,
                        new Response { Status = "Thất bại", Message = "Gửi request thất bại!" });

                var commentEntity = _unitOfWork.Mapper.Map<Comment>(commentModel);
                if (commentEntity != null)
                {
                    await _unitOfWork.CommentRepository.AddAsync(commentEntity);
                    await _unitOfWork.CommitAsync();

                    var commentDto = _unitOfWork.Mapper.Map<CommentDto>(commentEntity);

                    return CreatedAtAction(nameof(GetComment), new { id = commentDto!.CommentId }, commentDto);
                }
            }
            catch (Exception e)
            {
                return StatusCode(500, "Internal Server Error");
            }
            return StatusCode(StatusCodes.Status500InternalServerError,
                new Response { Status = "Thất bại", Message = "Không thể đăng comment!" });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateComment(Guid id, CommentDto updatedCommentModel)
        {
            try
            {
                var existingCommentEntity = await _unitOfWork.CommentRepository.GetByIdAsync<Comment>(id);

                if (existingCommentEntity == null)
                    return StatusCode(StatusCodes.Status404NotFound,
                        new Response { Status = "Thất bại", Message = "ID không tồn tại. Vui lòng kiểm tra lại" });

                existingCommentEntity.Content_C = updatedCommentModel.Content_C;
                existingCommentEntity.Rate = updatedCommentModel.Rate;

                await _unitOfWork.CommentRepository.UpdateAsync(existingCommentEntity);

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
        public async Task<IActionResult> DeleteComment(Guid id)
        {
            var commentEntity = await _unitOfWork.CommentRepository.GetByIdAsync<Comment>(id);

            if (commentEntity == null)
            {
                return NotFound();
            }

            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (commentEntity.UserId != userId)
            {
                return Forbid();
            }

            await _unitOfWork.CommentRepository.DeleteAsync(commentEntity.CommentId);
            await _unitOfWork.CommitAsync();

            return Ok(new { message = "Xóa thành công" });
        }
    }
}
