namespace OpenMovies.TestingSuite.RepositoriesTestSuite;

public sealed class MovieRepositoryTest : InMemoryDatabaseFixture<AppDbContext>
{
    private readonly IMovieRepository _movieRepository;
    private readonly Mock<ILogger<MovieRepository>> _logger;

    public MovieRepositoryTest()
    {
        _logger = new Mock<ILogger<MovieRepository>>();

        _movieRepository = new MovieRepository(
            dbContext: DbContext,
            logger: _logger.Object
        );
    }

    [Fact(DisplayName = "Given a valid movie, should save successfully in the database")]
    public async Task GivenValidMovie_ShouldSaveSuccessfullyInTheDatabase()
    {
        var movie = Fixture.Create<Movie>();

        var result = await _movieRepository.SaveAsync(movie);
        var savedMovie = await DbContext.Movies.FindAsync(movie.Id);

        Assert.NotNull(savedMovie);
        Assert.True(result.IsSuccess);

        Assert.Equal(movie.Id, savedMovie.Id);
        Assert.Equal(movie.Title, savedMovie.Title);
        Assert.Equal(movie.Synopsis, savedMovie.Synopsis);
        Assert.Equal(movie.ImageUrl, savedMovie.ImageUrl);
        Assert.Equal(movie.VideoSource, savedMovie.VideoSource);
        Assert.Equal(movie.ReleaseYear, savedMovie.ReleaseYear);
        Assert.Equal(movie.DurationInMinutes, savedMovie.DurationInMinutes);
        Assert.Equal(movie.Category.Id, savedMovie.Category.Id);
    }

    [Fact(DisplayName = "Given a valid movie, should update successfully in the database")]
    public async Task GivenValidMovie_ShouldUpdateSuccessfullyInTheDatabase()
    {
        var movie = Fixture.Create<Movie>();

        await DbContext.Movies.AddAsync(movie);
        await DbContext.SaveChangesAsync();

        movie.Title = "Updated Title";

        var result = await _movieRepository.UpdateAsync(movie);
        var updatedMovie = await DbContext.Movies.FindAsync(movie.Id);

        Assert.True(result.IsSuccess);
        Assert.NotNull(updatedMovie);
        Assert.Equal(movie.Title, updatedMovie.Title);
    }

    [Fact(DisplayName = "Given a valid movie, should delete successfully from the database")]
    public async Task GivenValidMovie_ShouldDeleteSuccessfullyFromTheDatabase()
    {
        var movie = Fixture.Create<Movie>();

        await DbContext.Movies.AddAsync(movie);
        await DbContext.SaveChangesAsync();

        var result = await _movieRepository.DeleteAsync(movie);
        var deletedMovie = await DbContext.Movies.FindAsync(movie.Id);

        Assert.True(result.IsSuccess);
        Assert.Null(deletedMovie);
    }

    [Fact(DisplayName = "Given a valid predicate, should find all matching movies")]
    public async Task GivenValidPredicate_ShouldFindAllMatchingMovies()
    {
        const int releaseYearToSearch = 2005;
        var movies = Fixture.CreateMany<Movie>(5).ToList();

        movies[0].ReleaseYear = releaseYearToSearch;
        movies[1].ReleaseYear = releaseYearToSearch;

        await DbContext.Movies.AddRangeAsync(movies);
        await DbContext.SaveChangesAsync();

        var foundMovies = await _movieRepository.FindAllAsync(movie => movie.ReleaseYear == releaseYearToSearch);

        Assert.Equal(2, foundMovies.Count());
    }

    [Fact(DisplayName = "Given a valid predicate, should find a single movie")]
    public async Task GivenValidPredicate_ShouldFindSingleMovie()
    {
        var movie = Fixture.Create<Movie>();

        await DbContext.Movies.AddAsync(movie);
        await DbContext.SaveChangesAsync();

        var foundMovie = await _movieRepository.FindSingleAsync(movie => movie.Id == movie.Id);

        Assert.NotNull(foundMovie);
        Assert.Equal(movie.Id, foundMovie.Id);
        Assert.Equal(movie.Title, foundMovie.Title);
        Assert.Equal(movie.Synopsis, foundMovie.Synopsis);
        Assert.Equal(movie.ImageUrl, foundMovie.ImageUrl);
        Assert.Equal(movie.VideoSource, foundMovie.VideoSource);
        Assert.Equal(movie.ReleaseYear, foundMovie.ReleaseYear);
        Assert.Equal(movie.DurationInMinutes, foundMovie.DurationInMinutes);
        Assert.Equal(movie.Category.Id, foundMovie.Category.Id);
    }

    [Fact(DisplayName = "Should fetch all movies")]
    public async Task ShouldFetchAllMovies()
    {
        var movies = Fixture.CreateMany<Movie>(5).ToList();

        await DbContext.Movies.AddRangeAsync(movies);
        await DbContext.SaveChangesAsync();

        var foundMovies = await _movieRepository.GetAllAsync();

        Assert.Equal(movies.Count, foundMovies.Count());
    }

    [Fact(DisplayName = "Given a valid id, should fetch a movie by id")]
    public async Task GivenValidId_ShouldFetchMovieById()
    {
        var movie = Fixture.Create<Movie>();

        await DbContext.Movies.AddAsync(movie);
        await DbContext.SaveChangesAsync();

        var foundMovie = await _movieRepository.GetByIdAsync(movie.Id);

        Assert.NotNull(foundMovie);
        Assert.Equal(movie.Id, foundMovie.Id);
        Assert.Equal(movie.Title, foundMovie.Title);
        Assert.Equal(movie.Synopsis, foundMovie.Synopsis);
        Assert.Equal(movie.ImageUrl, foundMovie.ImageUrl);
        Assert.Equal(movie.VideoSource, foundMovie.VideoSource);
        Assert.Equal(movie.ReleaseYear, foundMovie.ReleaseYear);
        Assert.Equal(movie.DurationInMinutes, foundMovie.DurationInMinutes);
        Assert.Equal(movie.Category.Id, foundMovie.Category.Id);
    }

    [Fact(DisplayName = "Should fetch movies in pages")]
    public async Task ShouldFetchMoviesInPages()
    {
        var movies = Fixture.CreateMany<Movie>(10).ToList();

        await DbContext.Movies.AddRangeAsync(movies);
        await DbContext.SaveChangesAsync();

        var pageNumber = 1;
        var pageSize = 5;

        var pagedMovies = await _movieRepository.PagedAsync(pageNumber, pageSize);

        Assert.Equal(pageSize, pagedMovies.Count());
    }

    [Fact(DisplayName = "Given a valid predicate, should fetch movies in pages")]
    public async Task GivenValidPredicate_ShouldFetchMoviesInPages()
    {
        const int releaseYearToSearch = 2005;
        var movies = Fixture.CreateMany<Movie>(10).ToList();

        movies[0].ReleaseYear = releaseYearToSearch;
        movies[1].ReleaseYear = releaseYearToSearch;
        movies[2].ReleaseYear = releaseYearToSearch;

        await DbContext.Movies.AddRangeAsync(movies);
        await DbContext.SaveChangesAsync();

        var pageNumber = 1;
        var pageSize = 5;

        var pagedMovies = await _movieRepository.PagedAsync(movie => movie.ReleaseYear == releaseYearToSearch, pageNumber, pageSize);

        Assert.Equal(3, pagedMovies.Count());
    }
}