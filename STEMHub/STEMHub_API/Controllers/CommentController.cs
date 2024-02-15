using Microsoft.AspNetCore.Mvc;
using STEMHub.STEMHub_Data.Entities;
using STEMHub.STEMHub_Service.Constants;
using STEMHub.STEMHub_Service.DTO;
using STEMHub.STEMHub_Service;

namespace STEMHub.STEMHub_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : BaseController
    {
        public CommentController(UnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

        [HttpGet]
        public IActionResult GetAllComment()
        {
            var comment = _unitOfWork.CommentRepository.GetAll<CommentDto>();
            return Ok(comment);
        }

        [HttpGet("{id}")]
        public IActionResult GetComment(Guid id)
        {
            var comment = _unitOfWork.CommentRepository.GetById<CommentDto>(id);

            if (comment == null)
                return StatusCode(StatusCodes.Status404NotFound,
                    new Response { Status = "Thất bại", Message = "ID không tồn tại. Vui lòng kiểm tra lại" });

            return Ok(comment);
        }

        [HttpPost]
        public IActionResult CreateComment(CommentDto commentModel)
        {
            try
            {
                if (commentModel == null)
                    return StatusCode(StatusCodes.Status400BadRequest,
                        new Response { Status = "Thất bại", Message = "Gửi request thất bại!" });

                var commentEntity = _unitOfWork.Mapper.Map<Comment>(commentModel);
                if (commentEntity != null)
                {
                    _unitOfWork.CommentRepository.Add(commentEntity);
                    _unitOfWork.Commits();

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
        public IActionResult UpdateComment(Guid id, CommentDto updatedCommentModel)
        {
            try
            {
                var existingCommentEntity = _unitOfWork.CommentRepository.GetById<Comment>(id);

                if (existingCommentEntity == null)
                    return StatusCode(StatusCodes.Status404NotFound,
                        new Response { Status = "Thất bại", Message = "ID không tồn tại. Vui lòng kiểm tra lại" });

                existingCommentEntity.Content_C = updatedCommentModel.Content_C;
                existingCommentEntity.Rate = updatedCommentModel.Rate;

                _unitOfWork.CommentRepository.Update(existingCommentEntity);

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
        public IActionResult DeleteComment(Guid id)
        {
            var commentEntity = _unitOfWork.CommentRepository.GetById<CommentDto>(id);

            if (commentEntity == null)
                return NotFound();

            _unitOfWork.CommentRepository.Delete(id);
            _unitOfWork.Commits();

            return Ok(new { message = "Xóa thành công" });
        }
    }
}
