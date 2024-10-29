using AutoMapper;
using Microsoft.EntityFrameworkCore;
using STEMHub.STEMHub_Data.Data;
using STEMHub.STEMHub_Data.Entities;
using STEMHub.STEMHub_Services.Interfaces;

namespace STEMHub.STEMHub_Services.Repository
{
    public class SearchKeywordRepository : ISearchKeywordRepository
    {

        protected readonly STEMHubDbContext _context;
        protected readonly IMapper _mapper;

        public SearchKeywordRepository(STEMHubDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Search> GetByKeywordAsync(string keyword)
        {
            return await _context.Set<Search>()
                .FirstOrDefaultAsync(k => k.SearchKeyword == keyword);
        }

        public async Task AddAsync(Search keyword)
        {
            await _context.Set<Search>().AddAsync(keyword);
        }

        public async Task UpdateAsync(Search keyword)
        {
            _context.Set<Search>().Update(keyword);
        }

        public async Task<IEnumerable<Search>> GetTopKeywordsAsync(int top)
        {
            return await _context.Set<Search>()
                .OrderByDescending(k => k.SearchCount)
                .Take(top)
                .ToListAsync();
        }
    }
}
