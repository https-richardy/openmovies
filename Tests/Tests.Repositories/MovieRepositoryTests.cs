# pragma warning disable CS8618, CS8602

namespace OpenMovies.Repositories.Tests;

public class MovieRepositoryTests : IAsyncLifetime
{
    private DbContextOptions<AppDbContext> _options;
    private AppDbContext _dbContext;

    public async Task InitializeAsync()
    {
        _options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _dbContext = new AppDbContext(_options);
        await _dbContext.Database.EnsureCreatedAsync();
    }

    public async Task DisposeAsync()
    {
        await _dbContext.Database.EnsureDeletedAsync();
        _dbContext.Dispose();
    }

    [Fact]
    public async Task AddAsync_ShouldAddMovieInDatabase()
    {
        var movieRepository = new MovieRepository(_dbContext);

        var category = new Category("Action");
        var movie = new Movie("Movie Title", DateTime.Now, "Synopsis", category);

        await movieRepository.AddAsync(movie);
        var result = await _dbContext.Movies.FirstOrDefaultAsync(m => m.Id == movie.Id);

        Assert.NotNull(result);
        Assert.Equal(movie.Id, result.Id);
    }

    [Fact]
    public async Task GetAllMoviesAsync_ShouldReturnAllMovies()
    {
        var movieRepository = new MovieRepository(_dbContext);

        var category = new Category("Action");
        var movies = new List<Movie>
        {
            new Movie("Movie 1", DateTime.Now, "Synopsis 1", category),
            new Movie("Movie 2", DateTime.Now, "Synopsis 2", category)
        };

        await _dbContext.Movies.AddRangeAsync(movies);
        await _dbContext.SaveChangesAsync();

        var result = await movieRepository.GetAllMoviesAsync();

        Assert.NotNull(result);
        Assert.Equal(movies.Count, result.Count());
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnMovieById()
    {
        var movieRepository = new MovieRepository(_dbContext);

        var category = new Category("Action");
        var movie = new Movie("Movie Title", DateTime.Now, "Synopsis", category);

        await _dbContext.Movies.AddAsync(movie);
        await _dbContext.SaveChangesAsync();

        var result = await movieRepository.GetByIdAsync(movie.Id);

        Assert.NotNull(result);
        Assert.Equal(movie.Id, result.Id);
    }

    [Fact]
    public async Task GetAllMoviesAsync_WithPredicate_ShouldReturnFilteredMovies()
    {
        var movieRepository = new MovieRepository(_dbContext);

        var category = new Category("Action");
        var movies = new List<Movie>
        {
            new Movie("Movie 1", DateTime.Now, "Synopsis 1", category),
            new Movie("Movie 2", DateTime.Now, "Synopsis 2", category)
        };

        _dbContext.Movies.AddRange(movies);
        await _dbContext.SaveChangesAsync();

        var result = await movieRepository.GetAllMoviesAsync(m => m.Title == "Movie 1");

        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal("Movie 1", result.First().Title);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateMovie()
    {
        var movieRepository = new MovieRepository(_dbContext);

        var category = new Category("Action");
        var movie = new Movie("Movie Title", DateTime.Now, "Synopsis", category);

        await _dbContext.Movies.AddAsync(movie);
        await _dbContext.SaveChangesAsync();

        movie.Title = "Updated Movie Title";

        await movieRepository.UpdateAsync(movie);
        var updatedMovie = await _dbContext.Movies.FindAsync(movie.Id);

        Assert.NotNull(updatedMovie);
        Assert.Equal("Updated Movie Title", updatedMovie.Title);
    }

    [Fact]
    public async Task SearchAsync_ShouldReturnFilteredMovies()
    {
        var movieRepository = new MovieRepository(_dbContext);

        var category = new Category("Action");
        var movies = new List<Movie>
        {
            new Movie("Movie 1", DateTime.Now, "Synopsis", category),
            new Movie("Movie 2", DateTime.Now, "Synopsis", category)
        };

        await _dbContext.Movies.AddRangeAsync(movies);
        await _dbContext.SaveChangesAsync();

        var result = await movieRepository.SearchAsync("Movie 1");

        Assert.NotNull(result);
        Assert.Equal("Movie 1", result.First().Title);
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemoveMovieFromDatabase()
    {
        var movieRepository = new MovieRepository(_dbContext);

        var category = new Category("Action");
        var movie = new Movie("Movie Title", DateTime.Now, "Synopsis", category);

        await _dbContext.Movies.AddAsync(movie);
        await _dbContext.SaveChangesAsync();

        await movieRepository.DeleteAsync(movie);
        var result = await _dbContext.Movies.FindAsync(movie.Id);

        Assert.Null(result);
    }

    [Fact]
    public async Task GetAsync_ShouldRetrieveMovieBasedOnPredicate()
    {
        var movieRepository = new MovieRepository(_dbContext);

        var category = new Category("Action");
        var movie = new Movie("Movie Title", DateTime.Now, "Synopsis", category);

        await _dbContext.Movies.AddAsync(movie);
        await _dbContext.SaveChangesAsync();

        var result = await movieRepository.GetAsync(m => m.Title == "Movie Title");

        Assert.NotNull(result);
        Assert.Equal("Movie Title", result.Title);
    }

    [Fact]
    public async Task SearchAsync_WithReleaseYear_ShouldReturnFilteredMovies()
    {
        var movieRepository = new MovieRepository(_dbContext);

        var category = new Category("Action");
        var movies = new List<Movie>
        {
            new Movie("Movie 1", new DateTime(2022, 1, 1), "Synopsis 1", category),
            new Movie("Movie 2", new DateTime(2022, 1, 1), "Synopsis 2", category)
        };

        await _dbContext.Movies.AddRangeAsync(movies);
        await _dbContext.SaveChangesAsync();

        var result = await movieRepository.SearchAsync(releaseYear: 2022);

        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task SearchAsync_WithCategory_ShouldReturnFilteredMovies()
    {
        var movieRepository = new MovieRepository(_dbContext);

        var category = new Category("Action");
        var movies = new List<Movie>
        {
            new Movie("Movie 1", DateTime.Now, "Synopsis 1", category),
            new Movie("Movie 2", DateTime.Now, "Synopsis 2", category)
        };

        await _dbContext.Movies.AddRangeAsync(movies);
        await _dbContext.SaveChangesAsync();

        var result = await movieRepository.SearchAsync(categoryId: category.Id);

        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
    }
}
