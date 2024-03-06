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
        public async Task<IActionResult> GetAllNewspaperArticle()
        {
            var newspaperArticle = await _unitOfWork.NewspaperArticleRepository.GetAllAsync<NewspaperArticleDto>();
            return Ok(newspaperArticle);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetNewspaperArticle(Guid id)
        {
            var newspaperArticle =await _unitOfWork.NewspaperArticleRepository.GetByIdAsync<NewspaperArticleDto>(id);

            if (newspaperArticle == null)
                return StatusCode(StatusCodes.Status404NotFound,
                    new Response { Status = "Thất bại", Message = "ID không tồn tại. Vui lòng kiểm tra lại" });

            return Ok(newspaperArticle);
        }

        [HttpPost]
        public async Task<IActionResult> CreateNewspaperArticle(NewspaperArticleDto? newspaperArticleModel)
        {
            try
            {
                if (newspaperArticleModel == null)
                    return StatusCode(StatusCodes.Status400BadRequest,
                        new Response { Status = "Thất bại", Message = "Gửi request thất bại!" });

                var newspaperArticleEntity = _unitOfWork.Mapper.Map<NewspaperArticle>(newspaperArticleModel);
                if (newspaperArticleEntity != null)
                {
                    await _unitOfWork.NewspaperArticleRepository.AddAsync(newspaperArticleEntity);
                    await _unitOfWork.CommitAsync();

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
        public async Task<IActionResult> UpdateNewspaperArticle(Guid id, NewspaperArticleDto updatedNewspaperArticleModel)
        {
            try
            {
                var existingNewspaperArticleEntity = await _unitOfWork.NewspaperArticleRepository.GetByIdAsync<NewspaperArticle>(id);

                // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
                if (existingNewspaperArticleEntity == null)
                    return StatusCode(StatusCodes.Status404NotFound,
                        new Response { Status = "Thất bại", Message = "ID không tồn tại. Vui lòng kiểm tra lại" });

                existingNewspaperArticleEntity.Title = updatedNewspaperArticleModel.Title;
                existingNewspaperArticleEntity.Markdown = updatedNewspaperArticleModel.Markdown;
                existingNewspaperArticleEntity.HtmlContent = updatedNewspaperArticleModel.HtmlContent;

                await _unitOfWork.NewspaperArticleRepository.UpdateAsync(existingNewspaperArticleEntity);
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
        public async Task<IActionResult> DeleteNewspaperArticle(Guid id)
        {
            var newspaperArticleEntity =await _unitOfWork.NewspaperArticleRepository.GetByIdAsync<NewspaperArticleDto>(id);

            if (newspaperArticleEntity == null)
                return NotFound();

            await _unitOfWork.NewspaperArticleRepository.DeleteAsync(id);
            await _unitOfWork.CommitAsync();

            return Ok(new { message = "Xóa thành công" });
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchNewspaperArticles([FromQuery] string newspaperArticleKey)
        {
            var newspaperArticles = await _unitOfWork.NewspaperArticleRepository.SearchAsync<NewspaperArticleDto>(newspaperArticles =>
                newspaperArticles.Title != null &&
                newspaperArticles.Title.Contains(newspaperArticleKey));
            if (!newspaperArticles.Any())
            {
                return StatusCode(StatusCodes.Status404NotFound,
                    new Response { Status = "Thất bại", Message = $"Không có Chủ đề bài viết chứa từ khoá {newspaperArticleKey}" });
            }

            return Ok(newspaperArticles);
        }
    }
}

