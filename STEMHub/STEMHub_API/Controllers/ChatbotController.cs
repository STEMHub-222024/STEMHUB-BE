using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using STEMHub.STEMHub_Data.DTO;
using STEMHub.STEMHub_Services.Interfaces;

namespace STEMHub.STEMHub_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatbotController : ControllerBase
    {
        private readonly IChatbotService _chatbotService;
        private readonly ILogger<ChatbotController> _logger;

        public ChatbotController(IChatbotService chatbotService, ILogger<ChatbotController> logger)
        {
            _chatbotService = chatbotService;
            _logger = logger;
        }

        [HttpPost("ask")]
        [ProducesResponseType(typeof(ChatbotRes), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Ask([FromBody] ChatbotReq request)
        {
            if (string.IsNullOrWhiteSpace(request?.Question))
            {
                return BadRequest("Question cannot be empty");
            }

            try
            {
                var response = await _chatbotService.GetAnswerAsync(request.Question);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing chatbot request");
                return StatusCode(500, "An error occurred while processing your request");
            }
        }

        [HttpGet("top-questions/last7days")]
        [ProducesResponseType(typeof(List<QuestionDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetTopQuestionsLast7Days()
        {
            var fromDate = DateTime.UtcNow.AddDays(-7);
            var topQuestions = await _chatbotService.GetTopQuestionsAsync(5, fromDate);
            return Ok(topQuestions);
        }

        [HttpGet("top-questions/last30days")]
        [ProducesResponseType(typeof(List<QuestionDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetTopQuestionsLast30Days()
        {
            var fromDate = DateTime.UtcNow.AddDays(-30);
            var topQuestions = await _chatbotService.GetTopQuestionsAsync(5, fromDate);
            return Ok(topQuestions);
        }

        [HttpPost("add")]
        [ProducesResponseType(typeof(QuestionDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddQuestion([FromBody] CreateQuestionDto questionDto)
        {
            if (string.IsNullOrWhiteSpace(questionDto?.Content) || string.IsNullOrWhiteSpace(questionDto?.Answer))
            {
                return BadRequest("Question content and answer cannot be empty");
            }

            try
            {
                var result = await _chatbotService.AddQuestionAsync(questionDto);
                return CreatedAtAction(nameof(AddQuestion), new { id = result.QuestionId }, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding question");
                return StatusCode(500, "An error occurred while adding the question");
            }
        }
    }
}
