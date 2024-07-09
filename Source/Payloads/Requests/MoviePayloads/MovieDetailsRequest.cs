namespace OpenMovies.WebApi.Payloads;

public sealed record MovieDetailsRequest : IRequest<Response<Movie>>
{
    public int MovieId { get; set; }
}