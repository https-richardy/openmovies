namespace OpenMovies.WebApi.Helpers;

public sealed class PaginationHelper<T>
{
    /// <summary>
    /// Gets the total count of items.
    /// </summary>
    public int Count { get; private set; }

    /// <summary>
    /// Gets the current page number.
    /// </summary>
    public int CurrentPage { get; private set; }

    /// <summary>
    /// Gets the URL for the next page, or null if no next page.
    /// </summary> 
    public string Next { get; private set; }

    /// <summary>
    /// Gets the URL for the previous page, or null if no previous page.
    /// </summary>
    public string Previous { get; private set; }

    /// <summary>
    /// Gets the collection of items on the current page.
    /// </summary>
    public IEnumerable<T> Results { get; private set; } = new List<T>();

    public PaginationHelper(IEnumerable<T> data, int pageNumber, int pageSize, HttpContext httpContext)
    {
        Count = data.Count();
        CurrentPage = pageNumber;

        int totalPages = (int)Math.Ceiling(Count / (double)pageSize);

        Next = CalculateNextUrl(httpContext.Request.Path, pageNumber, totalPages);
        Previous = CalculatePreviousUrl(httpContext.Request.Path, pageNumber);

        Results = data.Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();
    }

    #pragma warning disable CS8603
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