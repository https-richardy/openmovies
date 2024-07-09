namespace OpenMovies.TestingSuite.HandlersTestSuite.MovieHandlers;

public sealed class MovieDetailsHandlerTest
{
    private readonly Mock<IMovieRepository> _movieRepositoryMock;
    private readonly Mock<ILogger<MovieDetailsHandler>> _loggerMock;
    private readonly MovieDetailsHandler _handler;
    private readonly Fixture _fixture;

    public MovieDetailsHandlerTest()
    {
        _movieRepositoryMock = new Mock<IMovieRepository>();
        _loggerMock = new Mock<ILogger<MovieDetailsHandler>>();

        _handler = new MovieDetailsHandler(
            movieRepository: _movieRepositoryMock.Object,
            logger: _loggerMock.Object
        );

        _fixture = new Fixture();
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
    }

    [Fact(DisplayName = "Given a valid movie id, should return movie details successfully")]
    public async Task GivenValidMovieId_ShouldReturnMovieDetailsSuccessfully()
    {
        var movieId = 1;
        var movie = _fixture.Create<Movie>();

        _movieRepositoryMock
            .Setup(repo => repo.GetByIdAsync(movieId))
            .ReturnsAsync(movie);

        var request = new MovieDetailsRequest { MovieId = movieId };

        var response = await _handler.Handle(request, CancellationToken.None);

        _movieRepositoryMock
            .Verify(repository => repository.GetByIdAsync(movieId), Times.Once);

        Assert.NotNull(response);
        Assert.Equal(StatusCodes.Status200OK, response.StatusCode);
        Assert.Equal("Movie details retrieved successfully.", response.Message);
        Assert.Equal(movie, response.Data);
    }

    [Fact(DisplayName = "Given an invalid movie id, should return 404 not found")]
    public async Task GivenInvalidMovieId_ShouldReturnNotFound()
    {
        var movieId = 1;

        #pragma warning disable CS8600 // disable CS8600 warning because in this case movie is null
        _movieRepositoryMock
            .Setup(repo => repo.GetByIdAsync(movieId))
            .ReturnsAsync((Movie)null);

        var request = new MovieDetailsRequest { MovieId = movieId };
        var response = await _handler.Handle(request, CancellationToken.None);

        _movieRepositoryMock
            .Verify(repository => repository.GetByIdAsync(movieId), Times.Once);

        Assert.NotNull(response);
        Assert.Equal(StatusCodes.Status404NotFound, response.StatusCode);
        Assert.Equal("Movie not found.", response.Message);
        Assert.Null(response.Data);
    }

    [Fact(DisplayName = "Given an exception occurs, should return 500 internal server error")]
    public async Task GivenExceptionOccurs_ShouldReturnInternalServerError()
    {
        var movieId = 1;

        _movieRepositoryMock
            .Setup(repository => repository.GetByIdAsync(movieId))
            .ThrowsAsync(new Exception("Database failure"));

        var request = new MovieDetailsRequest { MovieId = movieId };
        var response = await _handler.Handle(request, CancellationToken.None);

        _movieRepositoryMock
            .Verify(repository => repository.GetByIdAsync(movieId), Times.Once);

        Assert.NotNull(response);
        Assert.Equal(StatusCodes.Status500InternalServerError, response.StatusCode);
        Assert.Equal("Failed to retrieve movie details.", response.Message);
        Assert.Null(response.Data);
    }
}
