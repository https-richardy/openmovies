using System.Linq.Expressions;
using AutoFixture;
using FluentValidation.Results;

namespace OpenMovies.Services.Tests;
public class MovieServiceTests
{
    private readonly Fixture _fixture;
    private readonly Mock<IMovieRepository> _movieRepositoryMock;
    private readonly Mock<ICategoryRepository> _categoryRepositoryMock;
    private readonly MovieService _movieService;

    public MovieServiceTests()
    {
        _fixture = new Fixture();
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        _movieRepositoryMock = new Mock<IMovieRepository>();
        _categoryRepositoryMock = new Mock<ICategoryRepository>();

        _movieService = new MovieService(
            _movieRepositoryMock.Object,
            _categoryRepositoryMock.Object
            );
    }

    [Fact]
    public async Task GetAllMovies_ReturnsAllMovies()
    {
        var expectedMovies = _fixture.CreateMany<Movie>().ToList();

        _movieRepositoryMock.Setup(repo => repo.GetAllMoviesAsync())
            .ReturnsAsync(expectedMovies);

        var result = await _movieService.GetAllMovies();

        Assert.Equal(expectedMovies, result);
    }

    [Fact]
    public async Task GetMovieById_InvalidId_ReturnsNull()
    {
        var invalidMovieId = -1;

        # pragma warning disable CS8620, CS8600
        _movieRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<Expression<Func<Movie, bool>>>()))
            .ReturnsAsync((Movie)null);

        await Assert.ThrowsAsync<InvalidOperationException>(async () => await _movieService.GetMovieById(invalidMovieId));
    }

    [Fact]
    public async Task CreateMovie_SuccessfulCreation()
    {
        var validSynopsis = new string('x', 60);
        var category = new Category { Id = 1, Name = "Action" };

        var movie = new Movie("Valid Title", DateTime.Now, validSynopsis, category);
        _movieRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<Expression<Func<Movie, bool>>>()))
            .ReturnsAsync((Movie)null);

        _categoryRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<Expression<Func<Category, bool>>>()))
            .ReturnsAsync(_fixture.Create<Category>());

        await _movieService.CreateMovie(movie);
        _movieRepositoryMock.Verify(repo => repo.AddAsync(movie), Times.Once);
    }

    [Fact]
    public async Task CreateMovie_ValidationFailure()
    {
        var movie = _fixture.Create<Movie>();
        var validationErrors = _fixture.CreateMany<ValidationFailure>().ToList();
        var validationException = new ValidationException("Validation failed", validationErrors);

        var validationMock = new Mock<IValidator<Movie>>();
        validationMock.Setup(v => v.ValidateAsync(movie, default))
            .ReturnsAsync(new ValidationResult(validationErrors));

        await Assert.ThrowsAsync<ValidationException>(async () => await _movieService.CreateMovie(movie));

    }

    [Fact]
    public async Task CreateMovie_DuplicateTitleFailure()
    {
        var validSynopsis = new string('x', 60);
        var category = new Category { Id = 1, Name = "Action" };

        var movie = new Movie("Existing Movie", DateTime.Now, validSynopsis, category);
        var newMovie = new Movie("Existing Movie", DateTime.Now, validSynopsis, category);

        _movieRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<Expression<Func<Movie, bool>>>()))
            .ReturnsAsync(movie);

        await Assert.ThrowsAsync<InvalidOperationException>(async () => await _movieService.CreateMovie(newMovie));
    }

    [Fact]
    public async Task CreateMovie_DirectorNotFoundFailure()
    {
        var validSynopsis = new string('x', 60);
        var category = new Category { Id = 1, Name = "Action" };

        var movie = new Movie("Existing Movie", DateTime.Now, validSynopsis, category);

        _movieRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<Expression<Func<Movie, bool>>>()))
            .ReturnsAsync((Movie)null);

        await Assert.ThrowsAsync<InvalidOperationException>(async () => await _movieService.CreateMovie(movie));
    }

    [Fact]
    public async Task CreateMovie_CategoryNotFoundFailure()
    {
        var validSynopsis = new string('x', 60);
        var category = new Category { Id = 1, Name = "Action" };

        var movie = new Movie("Existing Movie", DateTime.Now, validSynopsis, category);

        _movieRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<Expression<Func<Movie, bool>>>()))
            .ReturnsAsync((Movie)null);

        _categoryRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<Expression<Func<Category, bool>>>()))
            .ReturnsAsync((Category)null);

        await Assert.ThrowsAsync<InvalidOperationException>(async () => await _movieService.CreateMovie(movie));
    }

   [Fact]
    public async Task DeleteMovie_SuccessfulDeletion()
    {
        var existingMovie = _fixture.Create<Movie>();
        _movieRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<Expression<Func<Movie, bool>>>()))
            .ReturnsAsync(existingMovie);

        await _movieService.DeleteMovie(1);
        _movieRepositoryMock.Verify(repo => repo.DeleteAsync(existingMovie), Times.Once);
    }

    [Fact]
    public async Task DeleteMovie_InvalidId_ThrowsException()
    {
        var movieId = -1;
        _movieRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<Expression<Func<Movie, bool>>>()))
            .ReturnsAsync((Movie)null);

        await Assert.ThrowsAsync<InvalidOperationException>(() => _movieService.DeleteMovie(movieId));
    }

    [Fact]
    public async Task UpdateMovie_SuccessfulUpdate()
    {
        var validSynopsis = new string('x', 60);
        var category = new Category { Id = 1, Name = "Action" };

        var movie = new Movie("Existing Movie", DateTime.Now, validSynopsis, category);

        var updatedMovie = movie;
        updatedMovie.Title = "updated";

        _movieRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<Expression<Func<Movie, bool>>>()))
            .ReturnsAsync(movie);

        _movieRepositoryMock.Setup(repo => repo.UpdateAsync(movie))
            .Returns(Task.CompletedTask);

        _categoryRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<Expression<Func<Category, bool>>>()))
            .ReturnsAsync(_fixture.Create<Category>());

        await _movieService.UpdateMovie(updatedMovie);
        _movieRepositoryMock.Verify(repo => repo.UpdateAsync(movie), Times.Once);
    }

    [Fact]
    public async Task UpdateMovie_ValidationFailure()
    {
        var invalidMovie = _fixture.Create<Movie>();
        invalidMovie.Title = "";

        await Assert.ThrowsAsync<ValidationException>(() => _movieService.UpdateMovie(invalidMovie));
    }

    [Fact]
    public async Task UpdateMovie_MovieNotFoundFailure()
    {
        var validSynopsis = new string('x', 60);
        var category = new Category { Id = 1, Name = "Action" };

        var nonExistingMovie = new Movie("Existing Movie", DateTime.Now, validSynopsis, category);
        _movieRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<Expression<Func<Movie, bool>>>()))
            .ReturnsAsync((Movie)null);

        await Assert.ThrowsAsync<InvalidOperationException>(() => _movieService.UpdateMovie(nonExistingMovie));
    }

    [Fact]
    public async Task UpdateMovie_DirectorNotFoundFailure()
    {
        var validSynopsis = new string('x', 60);
        var category = new Category { Id = 1, Name = "Action" };

        var existingMovie = new Movie("Existing Movie", DateTime.Now, validSynopsis, category);
        var updatedMovie = existingMovie;

        await Assert.ThrowsAsync<InvalidOperationException>(() => _movieService.UpdateMovie(updatedMovie));
    }

    [Fact]
    public async Task UpdateMovie_CategoryNotFoundFailure()
    {
        var validSynopsis = new string('x', 60);
        var category = new Category { Id = 1, Name = "Action" };

        var existingMovie = new Movie("Existing Movie", DateTime.Now, validSynopsis, category);
        var updatedMovie = existingMovie;

        _categoryRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<Expression<Func<Category, bool>>>()))
            .ReturnsAsync((Category)null);

        await Assert.ThrowsAsync<InvalidOperationException>(() => _movieService.UpdateMovie(updatedMovie));
    }
}