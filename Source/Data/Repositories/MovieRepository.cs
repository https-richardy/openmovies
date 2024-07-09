namespace OpenMovies.WebApi.Data.Repositories;

public sealed class MovieRepository(AppDbContext dbContext, ILogger<MovieRepository> logger) :
    Repository<Movie, AppDbContext>(dbContext, logger), IMovieRepository
{

}