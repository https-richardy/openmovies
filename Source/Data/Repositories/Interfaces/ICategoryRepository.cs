using System.Linq.Expressions;

namespace OpenMovies.WebApi.Data.Repositories;

public interface ICategoryRepository
{
    Task<IEnumerable<Category>> GetAllAsync();
    Task<IEnumerable<Category>> GetAllAsync(Expression<Func<Category, bool>> predicate);

    Task<Category> GetAsync(Expression<Func<Category, bool>> predicate);
    Task<Category> GetByIdAsync(int id);

    Task AddAsync(Category category);
    Task UpdateAsync(Category category);
    Task DeleteAsync(Category category);
}