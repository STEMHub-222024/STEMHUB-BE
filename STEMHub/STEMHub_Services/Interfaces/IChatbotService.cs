using STEMHub.STEMHub_Data.DTO;

namespace STEMHub.STEMHub_Services.Interfaces
{
    public interface IChatbotService
    {
        Task<ChatbotRes> GetAnswerAsync(string question);
    }
}
