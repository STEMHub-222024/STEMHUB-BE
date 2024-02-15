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
    public class NewspaperArticleController : BaseController
    {
        public NewspaperArticleController(UnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

        [HttpGet]
        public IActionResult GetAllNewspaperArticle()
        {
            var newspaperArticle = _unitOfWork.NewspaperArticleRepository.GetAll<NewspaperArticleDto>();
            return Ok(newspaperArticle);
        }

        [HttpGet("{id}")]
        public IActionResult GetNewspaperArticle(Guid id)
        {
            var newspaperArticle = _unitOfWork.NewspaperArticleRepository.GetById<NewspaperArticleDto>(id);

            if (newspaperArticle == null)
                return StatusCode(StatusCodes.Status404NotFound,
                    new Response { Status = "Thất bại", Message = "ID không tồn tại. Vui lòng kiểm tra lại" });

            return Ok(newspaperArticle);
        }

        [HttpPost]
        public IActionResult CreateNewspaperArticle(NewspaperArticleDto? newspaperArticleModel)
        {
            try
            {
                if (newspaperArticleModel == null)
                    return StatusCode(StatusCodes.Status400BadRequest,
                        new Response { Status = "Thất bại", Message = "Gửi request thất bại!" });

                var newspaperArticleEntity = _unitOfWork.Mapper.Map<NewspaperArticle>(newspaperArticleModel);
                if (newspaperArticleEntity != null)
                {
                    _unitOfWork.NewspaperArticleRepository.Add(newspaperArticleEntity);
                    _unitOfWork.Commits();

                    var newspaperArticleDto = _unitOfWork.Mapper.Map<NewspaperArticleDto>(newspaperArticleEntity);

                    return CreatedAtAction(nameof(GetNewspaperArticle), new { id = newspaperArticleDto!.NewspaperArticleId }, newspaperArticleDto);
                }
            }
            catch (Exception e)
            {
                return StatusCode(500, "Internal Server Error");
            }

            return StatusCode(StatusCodes.Status500InternalServerError,
                new Response { Status = "Thất bại", Message = "Không thể tạo newspaperArticle!" });
        }

        [HttpPut("{id}")]
        public IActionResult UpdateNewspaperArticle(Guid id, NewspaperArticleDto updatedNewspaperArticleModel)
        {
            try
            {
                var existingNewspaperArticleEntity = _unitOfWork.NewspaperArticleRepository.GetById<NewspaperArticle>(id);

                // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
                if (existingNewspaperArticleEntity == null)
                    return StatusCode(StatusCodes.Status404NotFound,
                        new Response { Status = "Thất bại", Message = "ID không tồn tại. Vui lòng kiểm tra lại" });

                existingNewspaperArticleEntity.Title = updatedNewspaperArticleModel.Title;
                existingNewspaperArticleEntity.Content_NA = updatedNewspaperArticleModel.Content_NA;
                existingNewspaperArticleEntity.Image = updatedNewspaperArticleModel.Image;

                _unitOfWork.NewspaperArticleRepository.Update(existingNewspaperArticleEntity);

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
        public IActionResult DeleteNewspaperArticle(Guid id)
        {
            var newspaperArticleEntity = _unitOfWork.NewspaperArticleRepository.GetById<NewspaperArticleDto>(id);

            if (newspaperArticleEntity == null)
                return NotFound();

            _unitOfWork.NewspaperArticleRepository.Delete(id);
            _unitOfWork.Commits();

            return Ok(new { message = "Xóa thành công" });
        }

        [HttpGet("search")]
        public IActionResult SearchNewspaperArticles([FromQuery] string newspaperArticleKey)
        {
            var newspaperArticles = _unitOfWork.NewspaperArticleRepository.Search<NewspaperArticleDto>(newspaperArticles =>
                newspaperArticles.Title != null &&
                newspaperArticles.Title.Contains(newspaperArticleKey));
            if (newspaperArticles == null || !newspaperArticles.Any())
            {
                return StatusCode(StatusCodes.Status404NotFound,
                    new Response { Status = "Thất bại", Message = $"Không có Chủ đề bài viết chứa từ khoá {newspaperArticleKey}" });
            }

            return Ok(newspaperArticles);
        }
    }
}

