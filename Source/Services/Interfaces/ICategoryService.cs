using OpenMovies.Models;

namespace OpenMovies.Services;

public interface ICategoryService
{
    Task CreateCategory(Category category);
    Task<IEnumerable<Category>> GetAllCategories();
    Task<Category> GetCategoryById(int id);
    Task UpdateCategory(Category category);
    Task DeleteCategory(int categoryId);
}