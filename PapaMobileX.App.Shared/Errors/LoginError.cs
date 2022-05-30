using PapaMobileX.App.Shared.Enums;
using PapaMobileX.Shared.Results.Errors;

namespace PapaMobileX.App.Shared.Errors;

public class LoginError : Error
{
    private LoginError(LoginErrorType loginErrorType, string message)
    {
        LoginErrorType = loginErrorType;
        Message = message;
    }

    public LoginErrorType LoginErrorType { get; }

    public override string Message { get; }

    public static LoginError WrongCredentials()
    {
        return new(LoginErrorType.InvalidCredentials, "Login failed. Invalid credentials.");
    }

    public static LoginError ServerNotFound()
    {
        return new(LoginErrorType.ServerNotFound, "Login failed. Server not found.");
    }

    public static LoginError InvalidUriFormat()
    {
        return new(LoginErrorType.InvalidUri, "Login failed. Given address is in invalid format.");
    }

    public static LoginError Timeout()
    {
        return new(LoginErrorType.Timeoout, "Login failed. Timeout.");
    }

    public static LoginError OtherError()
    {
        return new(LoginErrorType.Other, "Login failed.");
    }
}