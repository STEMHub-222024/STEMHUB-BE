using Microsoft.EntityFrameworkCore;
using STEMHub.STEMHub_Service.Constants;
using STEMHub.STEMHub_Service.Interfaces;

namespace STEMHub.STEMHub_Service.Services
{
    public class PaginationService<TDto> : IPaginationService<TDto>
    {
        public async Task<(IEnumerable<TDto> data, int totalCount, int totalPages)> GetPagedDataAsync(IQueryable<TDto> queryable, int page, int pageSize)
        {
            try
            {
                var totalCount = await queryable.CountAsync();
                var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

                var data = await queryable
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                return (data, totalCount, totalPages);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }

}
