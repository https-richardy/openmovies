namespace OpenMovies.WebApi.Services;

public class MovieService : IMovieService
{
    private readonly IMovieRepository _movieRepository;
    private readonly ICategoryRepository _categoryRepository;

    public MovieService(
        IMovieRepository movieRepository,
        ICategoryRepository categoryRepository)
    {
        _movieRepository = movieRepository;
        _categoryRepository = categoryRepository;
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
        return await _movieRepository.GetAllMoviesAsync();
    }

    public async Task CreateMovie(Movie movie)
    {
        var validation = new MovieValidation();
        var validationResult = await validation.ValidateAsync(movie);

        if (!validationResult.IsValid)
            throw new ValidationException("Validation failed", validationResult.Errors);

        var existingMovie = await _movieRepository.GetAsync(m => m.Title == movie.Title);
        if (existingMovie != null)
            throw new InvalidOperationException("A film with the same title already exists.");

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

    public async Task UpdateMovie(Movie updatedMovie)
    {
        var validation = new MovieValidation();
        var validationResult = await validation.ValidateAsync(updatedMovie);

        if (!validationResult.IsValid)
            throw new ValidationException("Validation failed", validationResult.Errors);

        var existingMovie = await _movieRepository.GetAsync(m => m.Id == updatedMovie.Id);
        if (existingMovie == null)
            throw new InvalidOperationException($"Movie with ID '{updatedMovie.Id}' not found.");

        if (existingMovie.Title != updatedMovie.Title)
        {
            var movieWithSameTitle = await _movieRepository.GetAsync(m => m.Title == updatedMovie.Title);
            if (movieWithSameTitle != null)
                throw new InvalidOperationException("A film with the updated title already exists.");
        }

        var category = await _categoryRepository.GetAsync(c => c.Id == updatedMovie.Category.Id);
        if (category == null)
            throw new InvalidOperationException("The movie category was not found.");

        existingMovie.Title = updatedMovie.Title;
        existingMovie.ReleaseYear = updatedMovie.ReleaseYear;
        existingMovie.Synopsis = updatedMovie.Synopsis;
        existingMovie.Category = category;

        await _movieRepository.UpdateAsync(existingMovie);
    }

    public async Task<IEnumerable<Movie>> SearchMovies(string? name = null, int? releaseYear = null, int? categoryId = null)
    {
        return await _movieRepository.SearchAsync(name, releaseYear, categoryId);
    }
}