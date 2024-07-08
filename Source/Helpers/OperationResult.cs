namespace OpenMovies.WebApi.Helpers;

public struct OperationResult
{
    public bool IsSuccess { get; init; }
    public string Message { get; init; }
    public List<string> Errors { get; set; } = new List<string>();

    public OperationResult()
    {
        /* Object initialization */
    }

    public OperationResult(bool isSuccess, string message)
    {
        IsSuccess = isSuccess;
        Message = message;
    }

    public static OperationResult Success(string message = "Operation completed successfully")
    {
        return new OperationResult(isSuccess: true, message: message);
    }

    public static OperationResult Failure(string message = "Operation failed")
    {
        return new OperationResult(isSuccess: false, message: message);
    }

    public static OperationResult Failure(IEnumerable<string> errors, string message = "Operation failed")
    {
        return new OperationResult(isSuccess: false, message: message)
        {
            Errors = errors.ToList()
        };
    }

    public void AddError(string error)
    {
        Errors.Add(error);
    }

    public void AddErrors(IEnumerable<string> errors)
    {
        Errors.AddRange(errors);
    }
}