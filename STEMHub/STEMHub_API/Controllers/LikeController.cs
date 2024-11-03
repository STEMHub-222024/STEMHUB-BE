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
                return Ok(new {message = "Chưa đăng nhập! Vui lòng đăng nhập."});
            }

            var like = (await _unitOfWork.LikeRepository.SearchAsync<Like>(l =>
                l.NewspaperArticleId == articleId && l.UserId == userId)).FirstOrDefault();

            if (like == null)
            {
                like = new Like
                {
                    NewspaperArticleId = articleId,
                    UserId = userId
                };

                await _unitOfWork.LikeRepository.AddAsync(like);
                await _unitOfWork.CommitAsync();
                var count = await GetTotalLikes(articleId);
                return Ok(new { message = "Article liked successfully!", liked = true, totalLikes = count });
            }
            else
            {
                await _unitOfWork.LikeRepository.DeleteAsync(like.LikeId);
                await _unitOfWork.CommitAsync();
                var count = await GetTotalLikes(articleId);
                return Ok(new { message = "Article unliked successfully!", liked = false , totalLikes = count });
            }
        }

        [HttpGet("{articleId}/totalLikes")]
        public async Task<int> GetTotalLikes(Guid articleId)
        {
            var count = await _unitOfWork.LikeRepository.CountAsync(l => l.NewspaperArticleId == articleId);
            return count;
        }

        [HttpGet("{articleId}/isLiked")]
        public async Task<IActionResult> IsLiked(Guid articleId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                return Ok(new { message = "Chưa đăng nhập! Vui lòng đăng nhập." });
            }

            var isLiked = (await _unitOfWork.LikeRepository.SearchAsync<Like>(l =>
                l.NewspaperArticleId == articleId && l.UserId == userId)).Any();

            return Ok(new { liked = isLiked });
        }
    }
}
