namespace OpenMovies.WebApi.Data.Repositories;

public sealed class CategoryRepository(
    AppDbContext dbContext,
    ILogger<CategoryRepository> logger
) : Repository<Category, AppDbContext>(dbContext, logger), ICategoryRepository
{
    
}