namespace OpenMovies.TestingSuite.RepositoriesTestSuite;

public sealed class CategoryRepositoryTest : InMemoryDatabaseFixture<AppDbContext>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly Mock<ILogger<CategoryRepository>> _logger;

    public CategoryRepositoryTest()
    {
        _logger = new Mock<ILogger<CategoryRepository>>();

        _categoryRepository = new CategoryRepository(
            dbContext: DbContext,
            logger: _logger.Object
        );
    }

    [Fact(DisplayName = "Given a valid category, should save successfully in the database")]
    public async Task GivenValidCategory_ShouldSaveSuccessfullyInTheDatabase()
    {
        var category = Fixture.Create<Category>();

        var result = await _categoryRepository.SaveAsync(category);
        var savedCategory = await DbContext.Categories.FindAsync(category.Id);

        Assert.NotNull(savedCategory);
        Assert.True(result.IsSuccess);

        Assert.Equal(category.Id, savedCategory.Id);
        Assert.Equal(category.Name, savedCategory.Name);
    }

    [Fact(DisplayName = "Given a valid category, should update successfully in the database")]
    public async Task GivenValidCategory_ShouldUpdateSuccessfullyInTheDatabase()
    {
        var category = Fixture.Create<Category>();

        await DbContext.Categories.AddAsync(category);
        await DbContext.SaveChangesAsync();

        category.Name = "Updated Category Name";

        var result = await _categoryRepository.UpdateAsync(category);
        var updatedCategory = await DbContext.Categories.FindAsync(category.Id);

        Assert.True(result.IsSuccess);
        Assert.NotNull(updatedCategory);
        Assert.Equal(category.Name, updatedCategory.Name);
    }

    [Fact(DisplayName = "Given a valid category, should delete successfully from the database")]
    public async Task GivenValidCategory_ShouldDeleteSuccessfullyFromTheDatabase()
    {
        var category = Fixture.Create<Category>();

        await DbContext.Categories.AddAsync(category);
        await DbContext.SaveChangesAsync();

        var result = await _categoryRepository.DeleteAsync(category);
        var deletedCategory = await DbContext.Categories.FindAsync(category.Id);

        Assert.True(result.IsSuccess);
        Assert.Null(deletedCategory);
    }

    [Fact(DisplayName = "Given a valid predicate, should find all matching categories")]
    public async Task GivenValidPredicate_ShouldFindAllMatchingCategories()
    {
        var categories = Fixture.CreateMany<Category>(5).ToList();

        categories[0].Name = "Action";
        categories[1].Name = "Action";
        categories[2].Name = "Drama";

        await DbContext.Categories.AddRangeAsync(categories);
        await DbContext.SaveChangesAsync();

        var foundCategories = await _categoryRepository.FindAllAsync(category => category.Name == "Action");

        Assert.Equal(2, foundCategories.Count());
    }

    [Fact(DisplayName = "Given a valid predicate, should find a single category")]
    public async Task GivenValidPredicate_ShouldFindSingleCategory()
    {
        var category = Fixture.Create<Category>();

        await DbContext.Categories.AddAsync(category);
        await DbContext.SaveChangesAsync();

        var foundCategory = await _categoryRepository.FindSingleAsync(c => c.Id == category.Id);

        Assert.NotNull(foundCategory);
        Assert.Equal(category.Id, foundCategory.Id);
        Assert.Equal(category.Name, foundCategory.Name);
    }

    [Fact(DisplayName = "Should fetch all categories")]
    public async Task ShouldFetchAllCategories()
    {
        var categories = Fixture.CreateMany<Category>(5).ToList();

        await DbContext.Categories.AddRangeAsync(categories);
        await DbContext.SaveChangesAsync();

        var foundCategories = await _categoryRepository.GetAllAsync();

        Assert.Equal(categories.Count, foundCategories.Count());
    }

    [Fact(DisplayName = "Given a valid id, should fetch a category by id")]
    public async Task GivenValidId_ShouldFetchCategoryById()
    {
        var category = Fixture.Create<Category>();

        await DbContext.Categories.AddAsync(category);
        await DbContext.SaveChangesAsync();

        var foundCategory = await _categoryRepository.GetByIdAsync(category.Id);

        Assert.NotNull(foundCategory);
        Assert.Equal(category.Id, foundCategory.Id);
        Assert.Equal(category.Name, foundCategory.Name);
    }

    [Fact(DisplayName = "Should fetch categories in pages")]
    public async Task ShouldFetchCategoriesInPages()
    {
        var categories = Fixture.CreateMany<Category>(10).ToList();

        await DbContext.Categories.AddRangeAsync(categories);
        await DbContext.SaveChangesAsync();

        var pageNumber = 1;
        var pageSize = 5;

        var pagedCategories = await _categoryRepository.PagedAsync(pageNumber, pageSize);

        Assert.Equal(pageSize, pagedCategories.Count());
    }

    [Fact(DisplayName = "Given a valid predicate, should fetch categories in pages")]
    public async Task GivenValidPredicate_ShouldFetchCategoriesInPages()
    {
        var categories = Fixture.CreateMany<Category>(10).ToList();

        categories[0].Name = "Action";
        categories[1].Name = "Action";
        categories[2].Name = "Drama";

        await DbContext.Categories.AddRangeAsync(categories);
        await DbContext.SaveChangesAsync();

        var pageNumber = 1;
        var pageSize = 5;

        var pagedCategories = await _categoryRepository.PagedAsync(c => c.Name == "Action", pageNumber, pageSize);

        Assert.Equal(2, pagedCategories.Count());
    }
}