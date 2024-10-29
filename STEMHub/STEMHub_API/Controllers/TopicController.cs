
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using STEMHub.STEMHub_Data.Data;
using STEMHub.STEMHub_Data.DTO;
using STEMHub.STEMHub_Data.Entities;
using STEMHub.STEMHub_Services;
using STEMHub.STEMHub_Services.Constants;
using STEMHub.STEMHub_Services.Interfaces;
using STEMHub.STEMHub_Services.Services.Service;
using System.Text.Json;

namespace STEMHub.STEMHub_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TopicController : BaseController
    {
        private readonly STEMHubDbContext _context;
        private readonly IPaginationService<TopicDto> _paginationService;
        private readonly ISearchService _searchService;

        public TopicController(UnitOfWork unitOfWork, STEMHubDbContext context, IPaginationService<TopicDto> paginationService, ISearchService searchService) : base(unitOfWork)
        {
            _context = context;
            _paginationService = paginationService;
            _searchService = searchService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTopic()
        {
            var topic = await _unitOfWork.TopicRepository.GetAllAsync<TopicDto>();
            return Ok(topic);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTopic(Guid id)
        {
            var topicDto = await _unitOfWork.TopicRepository.GetByIdAsync<TopicDto>(id);
            var topic = await _unitOfWork.TopicRepository.GetByIdAsync<Topic>(id);
            if (topicDto == null)
                return StatusCode(StatusCodes.Status404NotFound,
                    new Response { Status = "Thất bại", Message = "ID không tồn tại. Vui lòng kiểm tra lại" });

            topic.View++;
            await _unitOfWork.CommitAsync();

            return Ok(topicDto);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTopic(TopicDto topicModel)
        {
            try
            {
                if (topicModel == null)
                    return StatusCode(StatusCodes.Status400BadRequest,
                        new Response { Status = "Thất bại", Message = "Gửi request thất bại!" });

                var topicEntity = _unitOfWork.Mapper.Map<Topic>(topicModel);
                if (topicEntity != null)
                {
                    await _unitOfWork.TopicRepository.AddAsync(topicEntity);
                    await _unitOfWork.CommitAsync();

                    var topicDto = _unitOfWork.Mapper.Map<TopicDto>(topicEntity);

                    return CreatedAtAction(nameof(GetTopic), new { id = topicDto!.TopicId }, topicDto);
                }
            }
            catch (Exception e)
            {
                return StatusCode(500, "Internal Server Error");
            }
            return StatusCode(StatusCodes.Status500InternalServerError,
                new Response { Status = "Thất bại", Message = "Không thể tạo chủ đề!" });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTopic(Guid id, TopicDto updatedTopicModel)
        {
            try
            {
                var existingTopicEntity = await _unitOfWork.TopicRepository.GetByIdAsync<Topic>(id);

                if (existingTopicEntity == null)
                    return StatusCode(StatusCodes.Status404NotFound,
                        new Response { Status = "Thất bại", Message = "ID không tồn tại. Vui lòng kiểm tra lại" });

                existingTopicEntity.TopicName = updatedTopicModel.TopicName;
                existingTopicEntity.VideoReview = updatedTopicModel.VideoReview;
                existingTopicEntity.TopicImage = updatedTopicModel.TopicImage;
                existingTopicEntity.Description = updatedTopicModel.Description;

                await _unitOfWork.TopicRepository.UpdateAsync(existingTopicEntity);

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
        public async Task<IActionResult> DeleteTopic(Guid id)
        {
            var topicEntity = await _unitOfWork.TopicRepository.GetByIdAsync<TopicDto>(id);

            if (topicEntity == null)
                return NotFound();

            await _unitOfWork.TopicRepository.DeleteAsync(id);
            _unitOfWork.Commit();

            return Ok(new { message = "Xóa thành công" });
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchTopics([FromQuery] string topicKey)
        {
            var topics = await _unitOfWork.TopicRepository.SearchAsync<TopicDto>(topic =>
                topic.TopicName != null &&
                topic.TopicName.Contains(topicKey));
            if (!topics.Any())
            {
                return StatusCode(StatusCodes.Status404NotFound,
                    new Response { Status = "Thất bại", Message = $"Không có Chủ đề chứa từ khoá {topicKey}" });
            }
            await _searchService.UpdateSearchKeywordAsync(topicKey);
            return Ok(topics);
        }

        [HttpGet("suggestionsAll")]
        public async Task<IActionResult> GetSuggestionsAll(Guid stemId)
        {
            var suggestedTopics = await _context.Topic
                .OrderByDescending(p => p.View)
                .Where(p => p.View > 0 && p.STEMId == stemId)
                .ToListAsync();

            var suggestedTopicDtos = suggestedTopics.Select(topic => new TopicDto
            {
                TopicId = topic.TopicId,
                TopicName = topic.TopicName,
                TopicImage = topic.TopicImage,
                View = topic.View,
                Description = topic.Description,
                STEMId = topic.STEMId
            }).ToList();

            return Ok(suggestedTopicDtos);
        }

        [HttpGet("suggestions")]
        public async Task<IActionResult> GetSuggestions()
        {
            var suggestedTopics = await _context.Topic
                .OrderByDescending(p => p.View)
                .Where(p => p.View > 0)
                .Take(4)
                .ToListAsync();

            var suggestedTopicDtos = suggestedTopics.Select(topic => new TopicDto
            {
                TopicId = topic.TopicId,
                TopicName = topic.TopicName,
                TopicImage = topic.TopicImage,
                View = topic.View,
                Description = topic.Description,
                STEMId = topic.STEMId
            }).ToList();

            return Ok(suggestedTopicDtos);
        }

        [HttpGet("paged")]
        public async Task<IActionResult> GetPagedTopics([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var queryable = _context.Topic
                    .Select(topic => new TopicDto
                    {
                        TopicId = topic.TopicId,
                        TopicName = topic.TopicName,
                        TopicImage = topic.TopicImage,
                        View = topic.View,
                        STEMId = topic.STEMId
                    });

                var (topics, totalCount, totalPages) = await _paginationService.GetPagedDataAsync(queryable, page, pageSize);

                var paginationMetadata = new
                {
                    totalCount,
                    totalPages,
                    currentPage = page,
                    pageSize
                };

                Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(paginationMetadata));

                return Ok(topics);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }


    }
}
