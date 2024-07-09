namespace OpenMovies.WebApi.Payloads;

public sealed record GetMoviesRequest : IRequest<PaginationHelper<Movie>>
{
    public int Page { get; init; } = 1;
    public int PageSize { get; init; } = 10;

    public string? Title { get; init; }
    public int? Year { get; init; }
}