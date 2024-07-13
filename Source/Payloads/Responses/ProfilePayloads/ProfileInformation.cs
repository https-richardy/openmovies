namespace OpenMovies.WebApi.Payloads;

public sealed record ProfileInformation
{
    public int Id { get; init; }
    public string DisplayName { get; init; }
    public string Avatar { get; init; }
}