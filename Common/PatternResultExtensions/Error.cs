public sealed record Error
{
    public int? Code { get; init; }
    public string? Message { get; init; }
    public ErrorTypes ErrorType { get; init; }

    private Error()
    {
        Code = 500;
        Message = "Internal server error...!";
        ErrorType = ErrorTypes.InternalServerError;
    }

    private Error(int? code, string? message, ErrorTypes errorType)
    {
        Code = code;
        Message = message;
        ErrorType = errorType;
    }


    public static Error None()
        => new(null, null, ErrorTypes.None);

    public static Error NotFound(string? message = "Data not found!")
        => new(404, message, ErrorTypes.NotFound);

    public static Error BadRequest(string? message = "Bad request!")
        => new(400, message, ErrorTypes.BadRequest);

    public static Error AlreadyExist(string? message = "Already exist!")
        => new(409, message, ErrorTypes.AlreadyExist);

    public static Error Conflict(string? message = "Conflict!")
        => new(409, message, ErrorTypes.Conflict);

    public static Error InternalServerError(string? message = "Internal server error!")
        => new(500, message, ErrorTypes.InternalServerError);
}