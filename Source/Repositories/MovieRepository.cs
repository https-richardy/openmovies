using System.Linq.Expressions;
using OpenMovies.Data;
using OpenMovies.Models;
using Microsoft.EntityFrameworkCore;

namespace OpenMovies.Repositories;

public class MovieRepository : IRepository<Movie>
{
    private readonly AppDbContext _dbContext;

    public MovieRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(Movie entity)
    {
        await _dbContext.Movies.AddAsync(entity);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(Movie entity)
    {
        _dbContext.Movies.Remove(entity);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<Movie>> GetAllAsync()
    {
        return await _dbContext.Movies
        .Include(m => m.Director)
        .Include(m => m.Category)
        .Include(m => m.Trailers)
        .ToListAsync();
    }

    # pragma warning disable CS8625
    public async Task<IEnumerable<Movie>> GetAllAsync(Expression<Func<Movie, bool>> predicate = null)
    {
        if (predicate == null)
            return await _dbContext.Movies.ToListAsync();

        return await _dbContext.Movies.Where(predicate).ToListAsync();
    }

    # pragma warning restore
    public async Task<Movie> GetAsync(Expression<Func<Movie, bool>> predicate)
    {
        # pragma warning disable CS8603
        return await _dbContext.Movies
            .Include(m => m.Director)
            .Include(m => m.Category)
            .Include(m => m.Trailers)
            .FirstOrDefaultAsync(predicate);
    }

    public async Task<Movie> GetByIdAsync(int id)
    {
        return await _dbContext.Movies.FindAsync(id);
    }

    # pragma warning restore
    public async Task UpdateAsync(Movie entity)
    {
        _dbContext.Entry(entity).State = EntityState.Modified;
        await _dbContext.SaveChangesAsync();
    }

    #pragma warning disable CS8602
    public async Task AddTrailersAsync(Movie movie, List<Trailer> trailers)
    {
        if (movie == null)
            throw new ArgumentNullException(nameof(movie));

        movie.Trailers.AddRange(trailers);
        await _dbContext.SaveChangesAsync();
    }
}