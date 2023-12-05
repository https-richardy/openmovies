# pragma warning disable CS8618

public class CategoryRepositoryTests : IAsyncLifetime
{
    private DbContextOptions<AppDbContext> _options;
    private AppDbContext _dbContext;

    public async Task InitializeAsync()
    {
        _options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _dbContext = new AppDbContext(_options);
        await _dbContext.Database.EnsureCreatedAsync();
    }

    public async Task DisposeAsync()
    {
        await _dbContext.Database.EnsureDeletedAsync();
        _dbContext.Dispose();
    }

    [Fact]
    public async Task AddAsync_ShouldAddCategoryInDatabase()
    {
        var categoryRepository = new CategoryRepository(_dbContext);
        var category = new Category { Id = 1, Name = "Action" };

        await categoryRepository.AddAsync(category);
        var result = await _dbContext.Categories.FirstOrDefaultAsync(c => c.Id == category.Id);

        Assert.NotNull(result);
        Assert.Equal(category.Id, result.Id);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllCategories()
    {
        var categoryRepository = new CategoryRepository(_dbContext);
        var categories = new List<Category>
        {
            new Category { Id = 1, Name = "Action" },
            new Category { Id = 2, Name = "Drama" }
        };

        await _dbContext.Categories.AddRangeAsync(categories);
        await _dbContext.SaveChangesAsync();

        var result = await categoryRepository.GetAllAsync();

        Assert.NotNull(result);
        Assert.Equal(categories.Count, result.Count());
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnCategoryById()
    {
        var categoryRepository = new CategoryRepository(_dbContext);
        var category = new Category { Id = 1, Name = "Action" };

        await _dbContext.Categories.AddAsync(category);
        await _dbContext.SaveChangesAsync();

        var result = await categoryRepository.GetByIdAsync(1);

        Assert.NotNull(result);
        Assert.Equal(category.Id, result.Id);
    }

    [Fact]
    public async Task GetAllAsync_WithPredicate_ShouldReturnFilteredCategories()
    {
        var categoryRepository = new CategoryRepository(_dbContext);
        var categories = new List<Category>
        {
            new Category { Id = 1, Name = "Action" },
            new Category { Id = 2, Name = "Drama" }
        };

        await _dbContext.Categories.AddRangeAsync(categories);
        await _dbContext.SaveChangesAsync();

        var result = await categoryRepository.GetAllAsync(c => c.Name == "Action");

        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal("Action", result.First().Name);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateCategory()
    {
        var categoryRepository = new CategoryRepository(_dbContext);
        var category = new Category { Id = 1, Name = "Action" };

        await _dbContext.Categories.AddAsync(category);
        await _dbContext.SaveChangesAsync();

        category.Name = "Updated";

        await categoryRepository.UpdateAsync(category);
        var updatedCategory = await _dbContext.Categories.FindAsync(1);

        Assert.NotNull(updatedCategory);
        Assert.Equal("Updated", updatedCategory.Name);
    }

    [Fact]
    public async Task DeleteAsync_ShouldDeleteCategory()
    {
        var categoryRepository = new CategoryRepository(_dbContext);
        var category = new Category { Id = 1, Name = "Action" };

        await _dbContext.Categories.AddAsync(category);
        await _dbContext.SaveChangesAsync();

        await categoryRepository.DeleteAsync(category);
        var result = await _dbContext.Categories.FindAsync(1);

        Assert.Null(result);
    }
}
