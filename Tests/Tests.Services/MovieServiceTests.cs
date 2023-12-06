using System.Linq.Expressions;
using AutoFixture;

namespace OpenMovies.Services.Tests;
public class MovieServiceTests
{
    [Fact]
    public async Task GetMovieById_ValidId_ReturnsMovieWithEmbeddedTrailers()
    {
        var movieRepositoryMock = new Mock<IMovieRepository>();
        var categoryRepositoryMock = new Mock<ICategoryRepository>();
        var directorRepositoryMock = new Mock<IDirectorRepository>();

        var movieService = new MovieService(
            movieRepositoryMock.Object,
            categoryRepositoryMock.Object,
            directorRepositoryMock.Object);

        var movie = new Movie("Movie 1", DateTime.Now, "Synopsis", It.IsAny<Director>(), It.IsAny<Category>());
        var trailers = new List<Trailer>()
        {
            new Trailer(TrailerType.Official, TrailerPlataform.Youtube, "https://youtube.com/watch?v=example", movie)
        };

        movie.Trailers = trailers;

        movieRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<Expression<Func<Movie, bool>>>()))
            .ReturnsAsync(movie);

        var result = await movieService.GetMovieById(1);

        Assert.NotNull(result);

        # pragma warning disable CS8604, xUnit2000
        Assert.Single(result.Trailers);
        Assert.Equal(movie.Trailers.First().Link, "https://www.youtube.com/embed/example");
    }

    [Fact]
    public async Task GetAllMovies_ReturnsAllMovies()
    {
        var movieRepositoryMock = new Mock<IMovieRepository>();
        var categoryRepositoryMock = new Mock<ICategoryRepository>();
        var directorRepositoryMock = new Mock<IDirectorRepository>();

        var movieService = new MovieService(
            movieRepositoryMock.Object,
            categoryRepositoryMock.Object,
            directorRepositoryMock.Object);

        var fixture = new Fixture();
        fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        var expectedMovies = fixture.CreateMany<Movie>().ToList();

        movieRepositoryMock.Setup(repo => repo.GetAllMoviesAsync())
            .ReturnsAsync(expectedMovies);

        var result = await movieService.GetAllMovies();

        Assert.Equal(expectedMovies, result);
    }

    [Fact]
    public async Task GetMovieById_InvalidId_ReturnsNull()
    {
        var fixture = new Fixture();
        var movieRepositoryMock = new Mock<IMovieRepository>();
        var categoryRepositoryMock = new Mock<ICategoryRepository>();
        var directorRepositoryMock = new Mock<IDirectorRepository>();

        var movieService = new MovieService(
            movieRepositoryMock.Object,
            categoryRepositoryMock.Object,
            directorRepositoryMock.Object);

        var invalidMovieId = -1;

        # pragma warning disable CS8620, CS8600
        movieRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<Expression<Func<Movie, bool>>>()))
            .ReturnsAsync((Movie)null);

        var result = await movieService.GetMovieById(invalidMovieId);
        Assert.Null(result);
    }
}