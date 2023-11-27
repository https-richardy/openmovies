using FluentValidation;
using OpenMovies.Models;
using OpenMovies.Repositories;
using OpenMovies.Validators;

namespace OpenMovies.Services;

public class DirectorService
{
    private readonly DirectorRepository _directorRepository;

    public DirectorService(DirectorRepository directorRepository)
    {
        _directorRepository = directorRepository;
    }

    public async Task CreateDirector(Director director)
    {
        var validation = new DirectorValidation();
        var validationResult = await validation.ValidateAsync(director);

        if (!validationResult.IsValid)
            throw new ValidationException("Validation failed.", validationResult.Errors);

        var existingDirector = await _directorRepository.GetAsync(d => d.FirstName == director.FirstName && d.LastName == director.LastName);
        if (existingDirector != null)
            throw new InvalidOperationException("A director with the same name already exists.");

        await _directorRepository.AddAsync(director);
    }

    public async Task<IEnumerable<Director>> GetAllDirectors()
    {
        return await _directorRepository.GetAllAsync();
    }

    public async Task<Director> GetDirectorById(int id)
    {
        var retrievedDirector = await _directorRepository.GetByIdAsync(id);
        if (retrievedDirector == null)
            throw new InvalidOperationException($"Director with ID '{id}' not found.");

        return retrievedDirector;
    }

    public async Task UpdateDirector(Director director)
    {
        var validation = new DirectorValidation();
        var validationResult = await validation.ValidateAsync(director);

        if (!validationResult.IsValid)
            throw new ValidationException("Validation failed.", validationResult.Errors);

        var existingDirector = await _directorRepository.GetByIdAsync(director.Id);
        if (existingDirector == null)
            throw new InvalidOperationException("Director not found.");

        await _directorRepository.UpdateAsync(director);
    }

    public async Task DeleteDirector(int directorId)
    {
        var retrievedDirector = await GetDirectorById(directorId);
        await _directorRepository.DeleteAsync(retrievedDirector);
    }
}