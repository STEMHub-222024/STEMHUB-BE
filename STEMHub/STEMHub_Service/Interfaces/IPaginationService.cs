namespace STEMHub.STEMHub_Service.Interfaces
{
    public interface IPaginationService<TDto>
    {
        Task<(IEnumerable<TDto> data, int totalCount, int totalPages)> GetPagedDataAsync(IQueryable<TDto> queryable, int page, int pageSize);
    }

}
