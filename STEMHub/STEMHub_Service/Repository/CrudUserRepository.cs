using AutoMapper;
using Microsoft.EntityFrameworkCore;
using STEMHub.STEMHub_Data.Data;
using STEMHub.STEMHub_Service.Interfaces;

namespace STEMHub.STEMHub_Service.Repository
{
    public class CrudUserRepository<T> : ICrudUserRepository<T> where T : class
    {
        private readonly STEMHubDbContext _context;
        private readonly IMapper _mapper;

        public CrudUserRepository(STEMHubDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<TDto> GetByIdUserAsync<TDto>(string id)
        {
            var entity = await _context.Set<T>().FindAsync(id);
            if (entity != null)
            {
                return MapToDto<TDto>(entity);
            }
            return default!;
        }

        public Task UpdateUserAsync(T entity)
        {
            _context.Set<T>().Update(entity);
            return Task.CompletedTask;
        }

        public async Task DeleteUserAsync(string id)
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
    }
}
