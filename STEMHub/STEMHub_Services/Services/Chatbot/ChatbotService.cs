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

        public async Task<List<QuestionDto>> GetTopQuestionsAsync(int top, DateTime fromDate)
        {
            var questions = await _context.Questions
           .Where(q => q.IsActive)
           .Select(q => new QuestionDto
           {
               QuestionId = q.QuestionId,
               Content = q.Content,
               Answer = q.Answer,
               CreatedDate = q.CreatedDate,
               SearchCount = q.QuestionSearches.Count(qs => qs.SearchedDate >= fromDate)
           })
           .Where(q => q.SearchCount > 0)
           .OrderByDescending(q => q.SearchCount)
           .Take(top)
           .ToListAsync();

            return questions;
        }

        public async Task<QuestionDto> AddQuestionAsync(CreateQuestionDto questionDto)
        {
            var question = new Question
            {
                QuestionId = Guid.NewGuid(),
                Content = questionDto.Content,
                Answer = questionDto.Answer,
                CreatedDate = DateTime.UtcNow,
                IsActive = true
            };

            _context.Questions.Add(question);
            await _context.SaveChangesAsync();

            return new QuestionDto
            {
                QuestionId = question.QuestionId,
                Content = question.Content,
                Answer = question.Answer,
                CreatedDate = question.CreatedDate
            };
        }
    }
}
