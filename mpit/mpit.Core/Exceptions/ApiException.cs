namespace mpit.mpit.Core.Exceptions;

public class ApiException(string message, int statusCode) : ApplicationException(message)
{
    public int StatusCode { get; } = statusCode;
}
