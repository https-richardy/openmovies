namespace OpenMovies.WebApi.Payloads;

public record Response<TData>
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public TData? Data { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Message { get; set; }

    public int StatusCode { get; set; } = 200;
    public bool IsSuccess => StatusCode >= 200 && StatusCode < 300;

    public Response() { }

    public Response(TData? data, int statusCode, string? message)
    {
        Data = data;
        StatusCode = statusCode;
        Message = message;
    }
}