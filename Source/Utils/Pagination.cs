#pragma warning disable CS8603
using OpenMovies.Models;
using OpenMovies.Repositories;

namespace OpenMovies.Utils;

public class Pagination<TEntity>
    where TEntity : Entity
{
    public int Count { get; set; }
    public string Next { get; set; }
    public string Previous { get; set; }
    public List<TEntity> Results { get; set; }

    public Pagination(IRepository<TEntity> repository, int pageNumber, int pageSize, HttpContext httpContext)
    {
        Count = repository.GetAllAsync().Result.Count();

        int totalPages = (int)Math.Ceiling(Count / (double)pageSize);
        Next = CalculateNextUrl(httpContext.Request.Path, pageNumber, totalPages);
        Previous = CalculatePreviousUrl(httpContext.Request.Path, pageNumber);

        Results = repository.GetAllAsync().Result
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();
    }

    public Pagination(List<TEntity> data, int pageNumber, int pageSize, HttpContext httpContext)
    {
        Count = data.Count;

        int totalPages = (int)Math.Ceiling(Count / (double)pageSize);
        Next = CalculateNextUrl(httpContext.Request.Path, pageNumber, totalPages);
        Previous = CalculatePreviousUrl(httpContext.Request.Path, pageNumber);

        Results = data.Skip((pageNumber - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();
    }

    private string CalculateNextUrl(PathString path, int pageNumber, int totalPages)
    {
        return pageNumber < totalPages
            ? $"{path}?={pageNumber + 1}"
            : null;
    }

    private string CalculatePreviousUrl(PathString path, int pageNumber)
    {
        return pageNumber > 1
            ? $"{path}?page={pageNumber - 1}"
            : null;
    }
}