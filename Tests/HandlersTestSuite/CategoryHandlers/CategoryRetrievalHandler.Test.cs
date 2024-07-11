namespace OpenMovies.TestingSuite.HandlersTestSuite.CategoryHandlers;

public sealed class CategoryRetrievalHandlerTest
{
    private readonly Mock<ICategoryRepository> _categoryRepositoryMock;
    private readonly Mock<ILogger<CategoryRetrievalHandler>> _loggerMock;
    private readonly IRequestHandler<CategoryRetrievalRequest, Response<Category>> _handler;

    public CategoryRetrievalHandlerTest()
    {
        _categoryRepositoryMock = new Mock<ICategoryRepository>();
        _loggerMock = new Mock<ILogger<CategoryRetrievalHandler>>();

        _handler = new CategoryRetrievalHandler(
            _categoryRepositoryMock.Object,
            _loggerMock.Object
        );
    }

    [Fact(DisplayName = "Given valid category retrieval request, should retrieve category successfully")]
    public async Task GivenValidCategoryRetrievalRequest_ShouldRetrieveCategorySuccessfully()
    {
        const int categoryId = 1;

        var expectedCategoryName = "Action";
        var category = new Category { Id = categoryId, Name = expectedCategoryName };

        _categoryRepositoryMock.Setup(repository => repository.GetByIdAsync(categoryId))
            .ReturnsAsync(category);

        var result = await _handler.Handle(new CategoryRetrievalRequest { CategoryId = categoryId }, default);

        _categoryRepositoryMock.Verify(repository => repository.GetByIdAsync(categoryId), Times.Once);

        Assert.NotNull(result.Data);
        Assert.Equal(expectedCategoryName, result.Data.Name);
        Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        Assert.Equal("category retrieved successfully", result.Message);
    }

    [Fact(DisplayName = "Given invalid category retrieval request (non-existing category), should return not found")]
    public async Task GivenInvalidCategoryRetrievalRequest_ShouldReturnNotFound()
    {
        const int categoryId = 999;

        #pragma warning disable CS8600 // disable CS8600 because it needs to be null since in this scenario no category is found.
        _categoryRepositoryMock
            .Setup(repository => repository.GetByIdAsync(categoryId))
            .ReturnsAsync((Category)null);

        var result = await _handler.Handle(new CategoryRetrievalRequest { CategoryId = categoryId }, default);

        _categoryRepositoryMock.Verify(repository => repository.GetByIdAsync(categoryId), Times.Once);

        Assert.Null(result.Data);
        Assert.False(result.IsSuccess);
        Assert.Equal(StatusCodes.Status404NotFound, result.StatusCode);
        Assert.Equal("category not found", result.Message);
    }
}