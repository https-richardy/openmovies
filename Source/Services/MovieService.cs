using FluentValidation;
using OpenMovies.DTOs;
using OpenMovies.Models;
using OpenMovies.Repositories;
using OpenMovies.Validators;

namespace OpenMovies.Services;

public class MovieService
{
    private readonly IMovieRepository _movieRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IDirectorRepository _directorRepository;


    public MovieService(
        IMovieRepository movieRepository,
        ICategoryRepository categoryRepository,
        IDirectorRepository directorRepository)
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

        #pragma warning disable CS8602
        movie.Trailers.ForEach(t => t.Link = t.GenerateEmbeddedLink());

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

        var director = await _directorRepository.GetAsync(d => d.Id == updatedMovie.Director.Id);
        if (director == null)
            throw new InvalidOperationException("The film's director could not be found.");

        var category = await _categoryRepository.GetAsync(c => c.Id == updatedMovie.Category.Id);
        if (category == null)
            throw new InvalidOperationException("The movie category was not found.");

        existingMovie.Title = updatedMovie.Title;
        existingMovie.ReleaseDateOf = updatedMovie.ReleaseDateOf;
        existingMovie.Synopsis = updatedMovie.Synopsis;
        existingMovie.Director = director;
        existingMovie.Category = category;
        existingMovie.Trailers = updatedMovie.Trailers;

        await _movieRepository.UpdateAsync(existingMovie);
    }

    public async Task<IEnumerable<Movie>> SearchMovies(string? name = null, int? releaseYear = null, int? categoryId = null)
    {
        return await _movieRepository.SearchAsync(name, releaseYear, categoryId);
    }

    public List<Trailer> CreateTrailers(List<TrailerDTO> trailersDTOs, Movie movie)
    {
        var trailers = new List<Trailer>();

        foreach (var trailerDTO in trailersDTOs)
        {
            var trailer = new Trailer(trailerDTO.Type, trailerDTO.Plataform, trailerDTO.Link, movie);
            trailers.Add(trailer);
        }

        return trailers;
    }

    public async Task AddTrailersToMovie(Movie movie, List<Trailer> trailers)
    {
        movie.Trailers = trailers;
        await _movieRepository.AddTrailersAsync(movie, trailers);
    }
}