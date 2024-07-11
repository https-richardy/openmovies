namespace OpenMovies.WebApi.Payloads;

public sealed record GetCategoriesRequest : IRequest<IEnumerable<Category>>
{
    /* empty */
}