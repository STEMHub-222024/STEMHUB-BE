using Microsoft.AspNetCore.Mvc;
using STEMHub.STEMHub_Data.DTO;
using STEMHub.STEMHub_Data.Entities;
using STEMHub.STEMHub_Services;
using System.Security.Claims;

namespace STEMHub.STEMHub_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LikeController : BaseController
    {
        public LikeController(UnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

        [HttpPost("{articleId}/toggleLike")]
        public async Task<IActionResult> ToggleLike(Guid articleId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                return Unauthorized();
            }

            // Kiểm tra nếu đã like
            var like = (await _unitOfWork.LikeRepository.SearchAsync<Like>(l =>
                l.NewspaperArticleId == articleId && l.UserId == userId)).FirstOrDefault();

            if (like == null)
            {
                // Nếu chưa like thì thêm like mới
                like = new Like
                {
                    NewspaperArticleId = articleId,
                    UserId = userId
                };

                await _unitOfWork.LikeRepository.AddAsync(like);
                await _unitOfWork.CommitAsync();

                return Ok(new { message = "Article liked successfully!", liked = true });
            }
            else
            {
                // Nếu đã like thì xóa
                await _unitOfWork.LikeRepository.DeleteAsync(like.LikeId);
                await _unitOfWork.CommitAsync();

                return Ok(new { message = "Article unliked successfully!", liked = false });
            }
        }

        [HttpGet("{articleId}/totalLikes")]
        public async Task<IActionResult> GetTotalLikes(Guid articleId)
        {
            var count = await _unitOfWork.LikeRepository.CountAsync(l => l.NewspaperArticleId == articleId);
            return Ok(new { count });
        }

        [HttpGet("{articleId}/isLiked")]
        public async Task<IActionResult> IsLiked(Guid articleId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                return Unauthorized();
            }

            var isLiked = (await _unitOfWork.LikeRepository.SearchAsync<Like>(l =>
                l.NewspaperArticleId == articleId && l.UserId == userId)).Any();

            return Ok(new { liked = isLiked });
        }


    }
}
