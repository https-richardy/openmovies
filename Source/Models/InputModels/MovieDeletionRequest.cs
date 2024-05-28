namespace OpenMovies.WebApi.Models.InputModels;

public sealed record MovieDeletionRequest : IRequest<OperationResult>
{
    public int MovieId { get; set; }
}