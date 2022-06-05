using System.Net;
using PapaMobileX.Shared.Results.Errors;

namespace PapaMobileX.Server.Shared.Errors;

public class LoginError : Error
{
    private LoginError(HttpStatusCode statusCode, string message)
    {
        Message = message;
        StatusCode = statusCode;
    }

    public HttpStatusCode StatusCode { get; }

    public override string Message { get; }

    public static LoginError WrongCredentials()
    {
        return new LoginError(HttpStatusCode.NotFound, "Login failed. Invalid credentials.");
    }
}