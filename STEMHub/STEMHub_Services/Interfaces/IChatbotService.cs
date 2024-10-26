using STEMHub.STEMHub_Data.DTO;

namespace STEMHub.STEMHub_Services.Interfaces
{
    public interface IChatbotService
    {
        Task<ChatbotRes> GetAnswerAsync(string question);
        Task<List<QuestionDto>> GetTopQuestionsAsync(int top, DateTime fromDate);
        Task<QuestionDto> AddQuestionAsync(CreateQuestionDto questionDto);
    }
}
