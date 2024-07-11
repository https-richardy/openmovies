namespace OpenMovies.TestingSuite.HandlersTestSuite.CategoryHandlers;

public sealed class GetCategoriesHandlerTest
{
    private readonly Mock<ICategoryRepository> _categoryRepositoryMock;
    private readonly Mock<ILogger<GetCategoriesHandler>> _loggerMock;
    private readonly IRequestHandler<GetCategoriesRequest, Response<IEnumerable<Category>>> _handler;

    public GetCategoriesHandlerTest()
    {
        _categoryRepositoryMock = new Mock<ICategoryRepository>();
        _loggerMock = new Mock<ILogger<GetCategoriesHandler>>();

        _handler = new GetCategoriesHandler(
            categoryRepository: _categoryRepositoryMock.Object,
            logger: _loggerMock.Object
        );
    }

    [Fact(DisplayName = "Given valid get categories request, should retrieve all categories successfully")]
    public async Task GivenValidGetCategoriesRequest_ShouldRetrieveAllCategoriesSuccessfully()
    {
        var categories = new List<Category>
        {
            new Category { Id = 1, Name = "Action" },
            new Category { Id = 2, Name = "Comedy" }
        };

        _categoryRepositoryMock.Setup(repository => repository.GetAllAsync()
            ).ReturnsAsync(categories);

        var result = await _handler.Handle(new GetCategoriesRequest(), default);

        _categoryRepositoryMock.Verify(repository => repository.GetAllAsync(), Times.Once);

        Assert.NotNull(result.Data);

        Assert.Equal(categories.Count, result.Data.Count());
        Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        Assert.Equal("categories retrieved successfully", result.Message);
    }
}