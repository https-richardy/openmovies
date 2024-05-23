namespace OpenMovies.WebApi.Models.InputModels;

public sealed record MovieDeletionRequest : IRequest<MovieDeletionResponse>
{
    public int MovieId { get; set; }
}