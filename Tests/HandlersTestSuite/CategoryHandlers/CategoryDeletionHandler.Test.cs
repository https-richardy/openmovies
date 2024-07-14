namespace OpenMovies.TestingSuite.HandlersTestSuite.CategoryHandlers;

public sealed class CategoryDeletionHandlerTest
{
    private readonly Mock<ICategoryRepository> _categoryRepositoryMock;
    private readonly Mock<ILogger<CategoryDeletionHandler>> _loggerMock;
    private readonly IRequestHandler<CategoryDeletionRequest, Response> _handler;
    private readonly IFixture _fixture;

    public CategoryDeletionHandlerTest()
    {
        _categoryRepositoryMock = new Mock<ICategoryRepository>();
        _loggerMock = new Mock<ILogger<CategoryDeletionHandler>>();

        _fixture = new Fixture();
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        _handler = new CategoryDeletionHandler(
            _categoryRepositoryMock.Object,
            _loggerMock.Object
        );
    }

    [Fact(DisplayName = "Given valid category deletion request, should delete category successfully")]
    public async Task GivenValidCategoryDeletionRequestShouldDeleteCategorySuccessFully()
    {
        const int categoryIdentifier = 1;

        var request = new CategoryDeletionRequest { CategoryId = categoryIdentifier };
        var category = new Category { Id = categoryIdentifier };

        _categoryRepositoryMock
            .Setup(repository => repository.GetByIdAsync(request.CategoryId))
            .ReturnsAsync(category);

        var result = await _handler.Handle(request, default);

        _categoryRepositoryMock.Verify(repository => repository.GetByIdAsync(request.CategoryId), Times.Once);
        _categoryRepositoryMock.Verify(repository => repository.DeleteAsync(category), Times.Once);

        Assert.True(result.IsSuccess);
        Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        Assert.Equal("category deleted successfully", result.Message);
    }

    [Fact(DisplayName = "Given invalid category deletion request (non-existing category), should return not found")]
    public async Task GivenInvalidCategoryDeletionRequestShouldReturnNotFound()
    {
        const int categoryIdentifier = 999;
        var request = new CategoryDeletionRequest { CategoryId = categoryIdentifier };

        #pragma warning disable CS8600 // disable CS8600 because it needs to be null since in this scenario no category is found.
        _categoryRepositoryMock
            .Setup(repository => repository.GetByIdAsync(request.CategoryId))
            .ReturnsAsync((Category)null);

        var result = await _handler.Handle(request, default);

        _categoryRepositoryMock.Verify(repository => repository.GetByIdAsync(request.CategoryId), Times.Once);

        Assert.False(result.IsSuccess);
        Assert.Equal(StatusCodes.Status404NotFound, result.StatusCode);
        Assert.Equal("category not found", result.Message);
    }
}