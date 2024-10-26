using Microsoft.EntityFrameworkCore;
using STEMHub.STEMHub_Data.Data;
using STEMHub.STEMHub_Data.DTO;
using STEMHub.STEMHub_Data.Entities;
using STEMHub.STEMHub_Services.Interfaces;

namespace STEMHub.STEMHub_Services.Services.Chatbot
{
    public class ChatbotService : IChatbotService
    {
        private readonly STEMHubDbContext _context;

        public ChatbotService(STEMHubDbContext context)
        {
            _context = context;
        }

        public async Task<ChatbotRes> GetAnswerAsync(string question)
        {
            // Convert question to lowercase for case-insensitive search
            var normalizedQuestion = question.ToLower().Trim();

            // Search for similar questions using LIKE operator
            var matchingQuestion = await _context.Questions
                .Where(q => q.IsActive &&
                           EF.Functions.Like(q.Content.ToLower(), $"%{normalizedQuestion}%"))
                .OrderByDescending(q => q.QuestionSearches.Count) // Prioritize frequently searched questions
                .FirstOrDefaultAsync();

            if (matchingQuestion == null)
            {
                return new ChatbotRes
                {
                    Answer = "I'm sorry, I couldn't find an answer to your question.",
                    Found = false
                };
            }

            // Log the search
            var questionSearch = new QuestionSearch
            {
                QuestionId = matchingQuestion.QuestionId,
                SearchedDate = DateTime.UtcNow
            };

            _context.QuestionSearches.Add(questionSearch);
            await _context.SaveChangesAsync();

            return new ChatbotRes
            {
                Answer = matchingQuestion.Answer,
                Found = true
            };
        }
    }
}
