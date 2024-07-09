namespace OpenMovies.TestingSuite.Helpers;

public abstract class InMemoryDatabaseFixture<TDbContext> : IAsyncLifetime
    where TDbContext : DbContext
{
    protected IFixture Fixture { get; private set; }
    protected TDbContext DbContext { get; private set; }

    protected InMemoryDatabaseFixture()
    {
        var options = new DbContextOptionsBuilder<TDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        DbContext = (Activator.CreateInstance(typeof(TDbContext), options) as TDbContext)!;

        Fixture = new Fixture();
        Fixture.Behaviors.Add(new OmitOnRecursionBehavior());
    }

    public async Task InitializeAsync()
    {
        await DbContext.Database.EnsureCreatedAsync();
    }

    public async Task DisposeAsync()
    {
        DbContext.Dispose();
        await DbContext.Database.EnsureDeletedAsync();
    }
}