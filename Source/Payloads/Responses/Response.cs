namespace OpenMovies.WebApi.Payloads;

public record Response
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Message { get; set; }

    public int StatusCode { get; set; }
    public bool IsSuccess => StatusCode >= 200 && StatusCode < 300;

    public Response() { }

    public Response(int statusCode, string? message)
    {
        StatusCode = statusCode;
        Message = message;
    }
}