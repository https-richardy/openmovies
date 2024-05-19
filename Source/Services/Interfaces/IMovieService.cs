namespace OpenMovies.WebApi.Services;

public interface IMovieService
{
    Task<Movie> GetMovieByIdAsync(int id);
    Task<IEnumerable<Movie>> GetAllMoviesAsync();
    Task CreateMovieAsync(Movie movie);
    Task DeleteMovieAsync(int movieId);
    Task UpdateMovieAsync(Movie updatedMovie);
    Task<IEnumerable<Movie>> SearchMoviesAsync(string? name = null, int? releaseYear = null, int? categoryId = null);
}