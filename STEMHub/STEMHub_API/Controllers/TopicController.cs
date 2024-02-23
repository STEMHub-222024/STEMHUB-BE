
using Microsoft.AspNetCore.Mvc;
using STEMHub.STEMHub_Data.Entities;
using STEMHub.STEMHub_Service;
using STEMHub.STEMHub_Service.Constants;
using STEMHub.STEMHub_Service.DTO;

namespace STEMHub.STEMHub_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TopicController : BaseController
    {
        public TopicController(UnitOfWork unitOfWork) : base(unitOfWork)
        {

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
            var topic = await _unitOfWork.TopicRepository.GetByIdAsync<TopicDto>(id);

            if (topic == null)
                return StatusCode(StatusCodes.Status404NotFound,
                    new Response { Status = "Thất bại", Message = "ID không tồn tại. Vui lòng kiểm tra lại" });
            
            return Ok(topic);
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
                existingTopicEntity.TopicImage = updatedTopicModel.TopicImage;

                await _unitOfWork.TopicRepository.UpdateAsync(existingTopicEntity);

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

            return Ok(topics);
        }
    }
}
