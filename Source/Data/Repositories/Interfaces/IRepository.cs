namespace OpenMovies.WebApi.Data.Repositories;

public interface IRepository<TEntity> where TEntity : Entity
{
    Task<OperationResult> SaveAsync(TEntity entity);
    Task<OperationResult> UpdateAsync(TEntity entity);
    Task<OperationResult> DeleteAsync(TEntity entity);

    Task<TEntity?> GetByIdAsync(int id);
    Task<IEnumerable<TEntity>> GetAllAsync();

    Task<TEntity?> FindSingleAsync(Expression<Func<TEntity, bool>> predicate);
    Task<IEnumerable<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>> predicate);
    
    Task<IEnumerable<TEntity>> PagedAsync(int pageNumber, int pageSize);
    Task<IEnumerable<TEntity>> PagedAsync(Expression<Func<TEntity, bool>> predicate, int pageNumber, int pageSize);
}