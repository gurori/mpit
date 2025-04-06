namespace mpit.mpit.Application.Interfaces.Repositories;

public interface IRepository<TModel>
    where TModel : class
{
    public Task<TModel?> GetByIdAsync(Guid id);
    public Task<IList<TModel>> GetAllAsync();
    public Task<IList<TModel>> GetManyByIdsAsync(IList<Guid> ids);
    public Task DeleteAsync(Guid id);
}
