namespace OpenMovies.WebApi.Payloads;

public sealed record GetCategoriesRequest : IRequest<Response<IEnumerable<Category>>>
{
    /* empty */
}