using OpenMovies.Models;
using OpenMovies.Repositories;

namespace OpenMovies.Services;

public class MovieServices
{
    private readonly MovieRepository _movieRepository;
    private readonly CategoryRepository _categoryRepository;
    private readonly DirectorRepository _directorRepository;


    public MovieServices(
        MovieRepository movieRepository,
        CategoryRepository categoryRepository,
        DirectorRepository directorRepository)
    {
        _movieRepository = movieRepository;
        _categoryRepository = categoryRepository;
        _directorRepository = directorRepository;
    }

    public async Task<Movie> GetMovieById(int id)
    {
        var movie = await _movieRepository.GetAsync(m => m.Id == id);
        if (movie == null)
            throw new InvalidOperationException($"Movie with ID '{id}' not found.");

        return movie;
    }

    public async Task<IEnumerable<Movie>> GetAllMovies()
    {
        return await _movieRepository.GetAllAsync(m => true);
    }

    public async Task CreateMovie(Movie movie)
    {
        var existingMovie = await _movieRepository.GetAsync(m => m.Title == movie.Title);
        if (existingMovie != null)
            throw new InvalidOperationException("A film with the same title already exists.");

        var director = await _directorRepository.GetAsync(d => d.Id == movie.Director.Id);
        if (director == null)
            throw new InvalidOperationException("The film's director could not be found.");

        var category = await _categoryRepository.GetAsync(c => c.Id == movie.Category.Id);
        if (category == null)
            throw new InvalidOperationException("The movie category was not found.");

        await _movieRepository.AddAsync(movie);
    }

    public async Task DeleteMovie(int movieId)
    {
        var movie = await _movieRepository.GetAsync(m => m.Id == movieId);
        if (movie == null)
            throw new InvalidOperationException($"Movie with ID '{movieId}' not found.");

        await _movieRepository.DeleteAsync(movie);
    }
}