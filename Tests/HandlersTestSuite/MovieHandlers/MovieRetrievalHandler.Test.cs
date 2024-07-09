namespace OpenMovies.TestingSuite.HandlersTestSuite.MovieHandlers;

public sealed class MovieRetrievalHandlerTest
{
    private readonly Mock<IMovieRepository> _movieRepositoryMock;
    private readonly Mock<ILogger<MovieRetrievalHandler>> _loggerMock;
    private readonly Mock<IHttpContextAccessor> _contextAccessorMock;
    private readonly IRequestHandler<MovieRetrievalRequest, Response<PaginationHelper<Movie>>> _handler;

    public MovieRetrievalHandlerTest()
    {
        _movieRepositoryMock = new Mock<IMovieRepository>();
        _loggerMock = new Mock<ILogger<MovieRetrievalHandler>>();
        _contextAccessorMock = new Mock<IHttpContextAccessor>();

        _handler = new MovieRetrievalHandler(
            _movieRepositoryMock.Object,
            _loggerMock.Object,
            _contextAccessorMock.Object
        );
    }

    [Fact(DisplayName = "Given valid request with title and year, should return paginated movies")]
    public async Task GivenValidRequestWithTitleAndYear_ShouldReturnPaginatedMovies()
    {
        var request = new MovieRetrievalRequest
        {
            Title = "Test",
            Year = 2023,
            Page = 1,
            PageSize = 10
        };

        var movies = new List<Movie>
        {
            new Movie { Id = 1, Title = "Test Movie 1", ReleaseYear = 2023 },
            new Movie { Id = 2, Title = "Test Movie 2", ReleaseYear = 2023 }
        };

        var httpContext = new DefaultHttpContext();
        _contextAccessorMock.Setup(accessor => accessor.HttpContext).Returns(httpContext);

        _movieRepositoryMock.Setup(repository => repository.PagedAsync(It.IsAny<Expression<Func<Movie, bool>>>(), request.Page, request.PageSize))
            .ReturnsAsync(movies);

        var response = await _handler.Handle(request, CancellationToken.None);

        Assert.NotNull(response);
        Assert.True(response.IsSuccess);

        Assert.Equal(StatusCodes.Status200OK, response.StatusCode);
        Assert.Equal("movies retrieved successfully", response.Message);

        Assert.NotNull(response.Data);
        Assert.Equal(movies.Count, response.Data.Count);
    }

    [Fact(DisplayName = "Given valid request without title and with year, should return paginated movies")]
    public async Task GivenValidRequestWithoutTitleAndWithYear_ShouldReturnPaginatedMovies()
    {
        var request = new MovieRetrievalRequest
        {
            Title = null,
            Year = 2023,
            Page = 1,
            PageSize = 10
        };

        var movies = new List<Movie>
        {
            new Movie { Id = 1, Title = "Test Movie 1", ReleaseYear = 2023 },
            new Movie { Id = 2, Title = "Test Movie 2", ReleaseYear = 2023 }
        };

        var httpContext = new DefaultHttpContext();
        _contextAccessorMock.Setup(accessor => accessor.HttpContext).Returns(httpContext);

        _movieRepositoryMock.Setup(repo => repo.PagedAsync(It.IsAny<Expression<Func<Movie, bool>>>(), request.Page, request.PageSize))
            .ReturnsAsync(movies);

        var response = await _handler.Handle(request, CancellationToken.None);

        Assert.NotNull(response);
        Assert.True(response.IsSuccess);

        Assert.Equal(StatusCodes.Status200OK, response.StatusCode);
        Assert.Equal("movies retrieved successfully", response.Message);

        Assert.NotNull(response.Data);
        Assert.Equal(movies.Count, response.Data.Count);
    }

    [Fact(DisplayName = "Given valid request with title and without year, should return paginated movies")]
    public async Task GivenValidRequestWithTitleAndWithoutYear_ShouldReturnPaginatedMovies()
    {
        var request = new MovieRetrievalRequest
        {
            Title = "Test",
            Year = null,
            Page = 1,
            PageSize = 10
        };

        var movies = new List<Movie>
        {
            new Movie { Id = 1, Title = "Test Movie 1", ReleaseYear = 2023 },
            new Movie { Id = 2, Title = "Test Movie 2", ReleaseYear = 2023 }
        };

        var httpContext = new DefaultHttpContext();
        _contextAccessorMock.Setup(accessor => accessor.HttpContext).Returns(httpContext);

        _movieRepositoryMock.Setup(repo => repo.PagedAsync(It.IsAny<Expression<Func<Movie, bool>>>(), request.Page, request.PageSize))
            .ReturnsAsync(movies);

        var response = await _handler.Handle(request, CancellationToken.None);

        Assert.NotNull(response);
        Assert.True(response.IsSuccess);

        Assert.Equal(StatusCodes.Status200OK, response.StatusCode);
        Assert.Equal("movies retrieved successfully", response.Message);

        Assert.NotNull(response.Data);
        Assert.Equal(movies.Count, response.Data.Count);
    }

    [Fact(DisplayName = "Given valid request without title and year, should return paginated movies")]
    public async Task GivenValidRequestWithoutTitleAndYear_ShouldReturnPaginatedMovies()
    {
        var request = new MovieRetrievalRequest
        {
            Title = null,
            Year = null,
            Page = 1,
            PageSize = 10
        };

        var movies = new List<Movie>
        {
            new Movie { Id = 1, Title = "Test Movie 1", ReleaseYear = 2023 },
            new Movie { Id = 2, Title = "Test Movie 2", ReleaseYear = 2023 }
        };

        var httpContext = new DefaultHttpContext();
        _contextAccessorMock.Setup(accessor => accessor.HttpContext).Returns(httpContext);

        _movieRepositoryMock.Setup(repo => repo.PagedAsync(It.IsAny<Expression<Func<Movie, bool>>>(), request.Page, request.PageSize))
            .ReturnsAsync(movies);

        var response = await _handler.Handle(request, CancellationToken.None);

        Assert.NotNull(response);
        Assert.True(response.IsSuccess);

        Assert.Equal(StatusCodes.Status200OK, response.StatusCode);
        Assert.Equal("movies retrieved successfully", response.Message);

        Assert.NotNull(response.Data);
        Assert.Equal(movies.Count, response.Data.Count);
    }
}