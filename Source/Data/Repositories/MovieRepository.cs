namespace OpenMovies.WebApi.Data.Repositories;

public sealed class MovieRepository(AppDbContext dbContext) :
    Repository<Movie, AppDbContext>(dbContext), IMovieRepository
{
    
}