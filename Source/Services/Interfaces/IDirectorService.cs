using OpenMovies.Models;

namespace OpenMovies.Services;

public interface IDirectorService
{
    Task CreateDirector(Director director);
    Task<IEnumerable<Director>> GetAllDirectors();
    Task<Director> GetDirectorById(int id);
    Task UpdateDirector(Director director);
    Task DeleteDirector(int directorId);
}
