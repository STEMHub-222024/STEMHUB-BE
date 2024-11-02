﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using STEMHub.STEMHub_Data.Entities;
using STEMHub.STEMHub_Services.Constants;
using STEMHub.STEMHub_Services;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using STEMHub.STEMHub_Data.Data;
using STEMHub.STEMHub_Services.Interfaces;
using STEMHub.STEMHub_Services.Services;
using STEMHub.STEMHub_Data.DTO;
using STEMHub.STEMHub_Services.Services.Service;
using System.Security.Claims;

namespace STEMHub.STEMHub_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewspaperArticleController : BaseController
    {

        private readonly STEMHubDbContext _context;
        private readonly IPaginationService<NewspaperArticleDto> _paginationService;
        private readonly ISearchService _searchService;

        public NewspaperArticleController(UnitOfWork unitOfWork, STEMHubDbContext context, IPaginationService<NewspaperArticleDto> paginationService, ISearchService searchService) : base(unitOfWork)
        {
            _context = context;
            _paginationService = paginationService;
            _searchService = searchService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllNewspaperArticle()
        {
            try
            {
                var newspaperArticles = await _unitOfWork.NewspaperArticleRepository.GetAllAsync<NewspaperArticleDto>();

                var articlesWithComments = new List<object>();

                foreach (var article in newspaperArticles)
                {
                    var totalComments = await _unitOfWork.CommentRepository.CountAsync(c => c.NewspaperArticleId == article.NewspaperArticleId);
                    var totalLikes = await _unitOfWork.LikeRepository.CountAsync(l => l.NewspaperArticleId == article.NewspaperArticleId);
                    articlesWithComments.Add(new
                    {
                        Article = article,
                        TotalComments = totalComments,
                        TotalLikes = totalLikes
                    });
                }

                return Ok(articlesWithComments);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetNewspaperArticle(Guid id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var newspaperArticle =await _unitOfWork.NewspaperArticleRepository.GetByIdAsync<NewspaperArticleDto>(id);
            var view = await _unitOfWork.NewspaperArticleRepository.GetByIdAsync<NewspaperArticle>(id);
            if (newspaperArticle == null)
                return StatusCode(StatusCodes.Status404NotFound,
                    new Response { Status = "Thất bại", Message = "ID không tồn tại. Vui lòng kiểm tra lại" });

            var totalLikes = await _unitOfWork.LikeRepository.CountAsync(l => l.NewspaperArticleId == id);
            var totalComments = await _unitOfWork.CommentRepository.CountAsync(c => c.NewspaperArticleId == id);
            bool isLiked = false;
            if (userId != null)
            {
                isLiked = (await _unitOfWork.LikeRepository.SearchAsync<Like>(l =>
                    l.NewspaperArticleId == id && l.UserId == userId)).Any();
            }

            view.View++;
            await _unitOfWork.CommitAsync();
            return Ok(new
            {
                Article = newspaperArticle,
                TotalLikes = totalLikes,
                Liked = isLiked,
                TotalComments = totalComments
            });
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
            catch (Exception)
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
                existingNewspaperArticleEntity.Image = updatedNewspaperArticleModel.Image;
                existingNewspaperArticleEntity.Description_NA = updatedNewspaperArticleModel.Description_NA;
                existingNewspaperArticleEntity.Markdown = updatedNewspaperArticleModel.Markdown;
                existingNewspaperArticleEntity.HtmlContent = updatedNewspaperArticleModel.HtmlContent;
                

                await _unitOfWork.NewspaperArticleRepository.UpdateAsync(existingNewspaperArticleEntity);
                await _unitOfWork.CommitAsync();

                return Ok(new { message = "Cập nhật thành công" });
            }
            catch (Exception e)
            {
                return StatusCode(500, "Internal Server Error");
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
            await _searchService.UpdateSearchKeywordAsync(newspaperArticleKey);
            return Ok(newspaperArticles);
        }

        [HttpGet("paged")]
        public async Task<IActionResult> GetPagedNewspaperArticles([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var queryable = _context.NewspaperArticle
                    .Select(newpaperArticle => new NewspaperArticleDto()
                    {
                        NewspaperArticleId = newpaperArticle.NewspaperArticleId,
                        Title = newpaperArticle.Title,
                        Markdown = newpaperArticle.Markdown,
                        HtmlContent = newpaperArticle.HtmlContent,
                        UserId = newpaperArticle.UserId
                    });

                var (newpaperArticles, totalCount, totalPages) = await _paginationService.GetPagedDataAsync(queryable, page, pageSize);

                var paginationMetadata = new
                {
                    totalCount,
                    totalPages,
                    currentPage = page,
                    pageSize
                };

                Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(paginationMetadata));

                return Ok(newpaperArticles);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }

    }
}

