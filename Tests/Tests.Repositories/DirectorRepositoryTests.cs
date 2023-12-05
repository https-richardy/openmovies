# pragma warning disable CS8618

namespace OpenMovies.Repositories.Tests;

public class DirectorRepositoryTests : IAsyncLifetime
{
    private DbContextOptions<AppDbContext> _options;
    private AppDbContext _dbContext;

    public async Task InitializeAsync()
    {
        _options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
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
    public async Task AddAsync_ShouldAddDirectorInDatabase()
    {
        var directorRepository = new DirectorRepository(_dbContext);
        var director = new Director { Id = 1, FirstName = "John", LastName = "Doe" };

        await directorRepository.AddAsync(director);
        var result = await _dbContext.Directors.FirstOrDefaultAsync(d => d.Id == director.Id);

        Assert.NotNull(result);
        Assert.Equal(director.Id, result.Id);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllDirectors()
    {
        var directorRepository = new DirectorRepository(_dbContext);
        var directors = new List<Director>
        {
            new Director { FirstName = "John", LastName = "Doe" },
            new Director { FirstName = "Jane", LastName = "Doe" }
        };

        await _dbContext.Directors.AddRangeAsync(directors);
        await _dbContext.SaveChangesAsync();

        var result = await directorRepository.GetAllAsync();

        Assert.NotNull(result);
        Assert.Equal(directors.Count, result.Count());
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnDirectorById()
    {
        var directorRepository = new DirectorRepository(_dbContext);
        var director = new Director { Id = 1, FirstName = "John", LastName = "Doe" };

        await _dbContext.Directors.AddAsync(director);
        await _dbContext.SaveChangesAsync();

        var result = await directorRepository.GetByIdAsync(1);

        Assert.NotNull(result);
        Assert.Equal(director.Id, result.Id);
    }

    [Fact]
    public async Task GetAllAsync_WithPredicate_ShouldReturnFilteredDirectors()
    {
        var directorRepository = new DirectorRepository(_dbContext);
        var directors = new List<Director>
        {
            new Director { Id = 1, FirstName = "John", LastName = "Doe" },
            new Director { Id = 2, FirstName = "Jane", LastName = "Doe" }
        };

        await _dbContext.Directors.AddRangeAsync(directors);
        await _dbContext.SaveChangesAsync();

        var result = await directorRepository.GetAllAsync(d => d.LastName == "Doe");

        Assert.NotNull(result);
        Assert.Equal(result.First().LastName, directors[0].LastName);
        Assert.Equal(result.Last().LastName, directors[1].LastName);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateDirector()
    {
        var directorRepository = new DirectorRepository(_dbContext);
        var director = new Director { Id = 1, FirstName = "John", LastName = "Doe" };

        await _dbContext.Directors.AddAsync(director);
        await _dbContext.SaveChangesAsync();

        director.FirstName = "Jane";

        await directorRepository.UpdateAsync(director);
        var updateddirector = await _dbContext.Directors.FindAsync(1);

        Assert.NotNull(updateddirector);
        Assert.Equal("Jane", updateddirector.FirstName);
    }

    [Fact]
    public async Task DeleteAsync_ShouldDeleteDirector()
    {
        var directorRepository = new DirectorRepository(_dbContext);
        var director = new Director { Id = 1, FirstName = "John", LastName = "Doe" };

        await _dbContext.Directors.AddAsync(director);
        await _dbContext.SaveChangesAsync();

        await directorRepository.DeleteAsync(director);
        var result = await _dbContext.Categories.FindAsync(1);

        Assert.Null(result);
    }
}
