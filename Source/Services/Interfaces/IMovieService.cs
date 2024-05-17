using OpenMovies.DTOs;
using OpenMovies.Models;

namespace OpenMovies.Services;

public interface IMovieService
{
    Task<Movie> GetMovieById(int id);
    Task<IEnumerable<Movie>> GetAllMovies();
    Task CreateMovie(Movie movie);
    Task DeleteMovie(int movieId);
    Task UpdateMovie(Movie updatedMovie);
    Task<IEnumerable<Movie>> SearchMovies(string? name = null, int? releaseYear = null, int? categoryId = null);
}