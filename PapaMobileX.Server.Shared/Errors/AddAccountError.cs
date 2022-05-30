using System.Net;
using PapaMobileX.Shared.Results.Errors;

namespace PapaMobileX.Server.Shared.Errors;

public class AddAccountError : Error
{
    private AddAccountError(HttpStatusCode statusCode, string message = "Account already exist.")
    {
        Message = message;
        StatusCode = statusCode;
    }

    public HttpStatusCode StatusCode { get; }

    public override string Message { get; }
    
    public static AddAccountError AccountAlreadyExist()
    {
        return new AddAccountError(HttpStatusCode.Conflict);
    }
}