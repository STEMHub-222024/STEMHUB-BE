using STEMHub.STEMHub_Data.DTO;
using STEMHub.STEMHub_Data.Entities;
using STEMHub.STEMHub_Services.Interfaces;

namespace STEMHub.STEMHub_Services.Services.Service
{
    public class SearchService : ISearchService
    {
        private readonly UnitOfWork _unitOfWork;

        public SearchService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task UpdateSearchKeywordAsync(string lessonKey)
        {
            var keyword = await _unitOfWork.SearchKeywordRepository.GetByKeywordAsync(lessonKey);
            if (keyword != null)
            {
                keyword.SearchCount++;
                await _unitOfWork.SearchKeywordRepository.UpdateAsync(keyword);
            }
            else
            {
                var newKeyword = new Search
                {
                    SearchKeyword = lessonKey,
                    SearchCount = 1
                };
                await _unitOfWork.SearchKeywordRepository.AddAsync(newKeyword);
            }
            await _unitOfWork.CommitAsync();
        }

        public async Task<IEnumerable<SearchKeywordsDto>> GetTopKeywordsAsync(int top = 5)
        {
            var topKeywords = await _unitOfWork.SearchKeywordRepository.GetTopKeywordsAsync(top);
            return topKeywords.Select(k => new SearchKeywordsDto
            {
                SearchKeyword = k.SearchKeyword,
                SearchCount = k.SearchCount
            });
        }
    }
}
