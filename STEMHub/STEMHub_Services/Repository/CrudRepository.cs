using AutoMapper;
using Microsoft.EntityFrameworkCore;
using STEMHub.STEMHub_Data.Data;
using STEMHub.STEMHub_Services.Interfaces;
using System.Linq.Expressions;

namespace STEMHub.STEMHub_Services.Repository
{
    public class CrudRepository<T> : ICrudRepository<T> where T : class
    {
        private readonly STEMHubDbContext _context;
        private readonly IMapper _mapper;

        public CrudRepository(STEMHubDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<TDto> GetByIdAsync<TDto>(Guid id)
        {
            var entity = await _context.Set<T>().FindAsync(id);
            if (entity != null)
            {
                return MapToDto<TDto>(entity);
            }
            return default!;
        }

        public async Task<IEnumerable<TDto>> GetAllAsync<TDto>()
        {
            var entities = await _context.Set<T>().ToListAsync();
            return entities.Select(entity => MapToDto<TDto>(entity));
        }

        public async Task AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
        }

        public Task UpdateAsync(T entity)
        {
            _context.Set<T>().Update(entity);
            return Task.CompletedTask;
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await _context.Set<T>().FindAsync(id);
            if (entity != null)
            {
                _context.Set<T>().Remove(entity);
            }
        }

        private TDto MapToDto<TDto>(T entity)
        {
            return _mapper.Map<TDto>(entity)!;
        }

        public async Task<IEnumerable<TDto>> SearchAsync<TDto>(Expression<Func<T, bool>> predicate)
        {
            var entities = await _context.Set<T>().Where(predicate).ToListAsync();
            return entities.Select(entity => MapToDto<TDto>(entity));
        }
    }
}
