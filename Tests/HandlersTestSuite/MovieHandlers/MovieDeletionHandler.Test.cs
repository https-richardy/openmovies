namespace OpenMovies.TestingSuite.HandlersTestSuite.MovieHandlers;

public sealed class MovieDeletionHandlerTest
{
    private readonly Mock<IMovieRepository> _movieRepositoryMock;
    private readonly Mock<ILogger<MovieDeletionHandler>> _loggerMock;
    private readonly MovieDeletionHandler _handler;

    public MovieDeletionHandlerTest()
    {
        _movieRepositoryMock = new Mock<IMovieRepository>();
        _loggerMock = new Mock<ILogger<MovieDeletionHandler>>();

        _handler = new MovieDeletionHandler(
            movieRepository: _movieRepositoryMock.Object,
            logger: _loggerMock.Object
        );
    }

    [Fact(DisplayName = "Given a movie does not exist, should return 404 NotFound")]
    public async Task GivenMovieDoesNotExist_ShouldReturnNotFound()
    {
        var movieId = 1;

        #pragma warning disable CS8600 // disable CS8600 warning because in this case movie is null
        _movieRepositoryMock
            .Setup(repository => repository.GetByIdAsync(movieId))
            .ReturnsAsync((Movie)null);

        var request = new MovieDeletionRequest { MovieId = movieId };
        var response = await _handler.Handle(request, CancellationToken.None);

        Assert.Equal(StatusCodes.Status404NotFound, response.StatusCode);
        Assert.Equal("movie not found", response.Message);
    }

    [Fact(DisplayName = "Given a movie exists, should delete movie and return 200 OK")]
    public async Task GivenMovieExists_ShouldDeleteMovieAndReturnOk()
    {
        var movie = new Movie { Id = 1, Title = "The Matrix" };

        _movieRepositoryMock
            .Setup(repository => repository.GetByIdAsync(movie.Id))
            .ReturnsAsync(movie);

        _movieRepositoryMock
            .Setup(repository => repository.DeleteAsync(movie))
            .ReturnsAsync(new OperationResult(isSuccess: true, message: "Movie deleted successfully"));

        var request = new MovieDeletionRequest { MovieId = movie.Id };
        var response = await _handler.Handle(request, CancellationToken.None);

        Assert.Equal(StatusCodes.Status200OK, response.StatusCode);
        Assert.Equal("movie deleted successfully", response.Message);
    }

    [Fact(DisplayName = "Given an exception occurs, should return 500 InternalServerError")]
    public async Task GivenExceptionOccurs_ShouldReturnInternalServerError()
    {
        var movieId = 1;

        _movieRepositoryMock
            .Setup(repository => repository.GetByIdAsync(movieId))
            .ThrowsAsync(new Exception("Database failure"));

        var request = new MovieDeletionRequest { MovieId = movieId };
        var response = await _handler.Handle(request, CancellationToken.None);

        Assert.Equal(StatusCodes.Status500InternalServerError, response.StatusCode);
        Assert.Equal("Database failure", response.Message);
    }
}