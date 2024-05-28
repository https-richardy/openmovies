namespace OpenMovies.WebApi.Services;

public sealed class MovieService : IMovieService
{
    private readonly IMovieRepository _movieRepository;

    public MovieService(IMovieRepository movieRepository)
    {
        _movieRepository = movieRepository;
    }

    public async Task<Movie> GetMovieByIdAsync(int id)
    {
        var movie = await _movieRepository.GetAsync(m => m.Id == id);
        if (movie == null)
            throw new ObjectDoesNotExistException($"Movie with ID '{id}' not found.");

        return movie;
    }

    public async Task<IEnumerable<Movie>> GetAllMoviesAsync()
    {
        return await _movieRepository.GetAllMoviesAsync();
    }

    public async Task CreateMovieAsync(Movie movie)
    {
        await _movieRepository.AddAsync(movie);
    }

    public async Task DeleteMovieAsync(int movieId)
    {
        var movie = await _movieRepository.GetAsync(m => m.Id == movieId);
        if (movie == null)
            throw new ObjectDoesNotExistException($"Movie with ID '{movieId}' not found.");

        await _movieRepository.DeleteAsync(movie);
    }

    public async Task UpdateMovieAsync(Movie updatedMovie)
    {
        var existingMovie = await _movieRepository.GetAsync(m => m.Id == updatedMovie.Id);
        if (existingMovie == null)
            throw new ObjectDoesNotExistException($"Movie with ID '{updatedMovie.Id}' not found.");

        await _movieRepository.UpdateAsync(existingMovie);
    }

    public async Task<IEnumerable<Movie>> SearchMoviesAsync(string? name = null, int? releaseYear = null, int? categoryId = null)
    {
        return await _movieRepository.SearchAsync(name, releaseYear, categoryId);
    }
}