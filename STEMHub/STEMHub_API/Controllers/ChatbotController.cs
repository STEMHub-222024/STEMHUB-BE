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
    }
}
