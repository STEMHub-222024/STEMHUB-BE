
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
        public IActionResult GetAllTopic()
        {
            var topic = _unitOfWork.TopicRepository.GetAll<TopicDto>();
            return Ok(topic);
        }

        [HttpGet("{id}")]
        public IActionResult GetTopic(Guid id)
        {
            var topic = _unitOfWork.TopicRepository.GetById<TopicDto>(id);

            if (topic == null)
                return StatusCode(StatusCodes.Status404NotFound,
                    new Response { Status = "Thất bại", Message = "ID không tồn tại. Vui lòng kiểm tra lại" });
            
            return Ok(topic);
        }

        [HttpPost]
        public IActionResult CreateTopic(TopicDto topicModel)
        {
            try
            {
                if (topicModel == null)
                    return StatusCode(StatusCodes.Status400BadRequest,
                        new Response { Status = "Thất bại", Message = "Gửi request thất bại!" });

                var topicEntity = _unitOfWork.Mapper.Map<Topic>(topicModel);
                if (topicEntity != null)
                {
                    _unitOfWork.TopicRepository.Add(topicEntity);
                    _unitOfWork.Commits();

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
        public IActionResult UpdateTopic(Guid id, TopicDto updatedTopicModel)
        {
            try
            {
                var existingTopicEntity = _unitOfWork.TopicRepository.GetById<Topic>(id);

                if (existingTopicEntity == null)
                    return StatusCode(StatusCodes.Status404NotFound,
                        new Response { Status = "Thất bại", Message = "ID không tồn tại. Vui lòng kiểm tra lại" });

                existingTopicEntity.TopicName = updatedTopicModel.TopicName;
                existingTopicEntity.TopicImage = updatedTopicModel.TopicImage;

                _unitOfWork.TopicRepository.Update(existingTopicEntity);

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
        public IActionResult DeleteTopic(Guid id)
        {
            var topicEntity = _unitOfWork.TopicRepository.GetById<TopicDto>(id);

            if (topicEntity == null)
                return NotFound();

            _unitOfWork.TopicRepository.Delete(id);
            _unitOfWork.Commits();

            return Ok(new { message = "Xóa thành công" });
        }

        [HttpGet("search")]
        public IActionResult SearchTopics([FromQuery] string topicKey)
        {
            var topics = _unitOfWork.TopicRepository.Search<TopicDto>(topic =>
                topic.TopicName != null &&
                topic.TopicName.Contains(topicKey));
            if (topics == null || !topics.Any())
            {
                return StatusCode(StatusCodes.Status404NotFound,
                    new Response { Status = "Thất bại", Message = $"Không có Chủ đề chứa từ khoá {topicKey}" });
            }

            return Ok(topics);
        }
    }
}
