using PapaMobileX.App.Shared.Enums;
using PapaMobileX.Shared.Results.Errors;

namespace PapaMobileX.App.Shared.Errors;

public class HubError : Error
{
    public HubErrorType ErrorType { get; }
    public override string Message { get; }

    
    private HubError(HubErrorType hubErrorType, string message)
    {
        ErrorType = hubErrorType;
        Message = message;
    }

    public static HubError HubAlreadyStarted()
    {
        return new HubError(HubErrorType.HubAlreadyStarted, "Hub has already been started.");
    }
    
    public static HubError ProblemDuringStartupAttempt()
    {
        return new HubError(HubErrorType.ProblemDuringStartupAttempt, "Failed while connecting to server.");
    }
    
    public static HubError ProblemDuringStopAttempt()
    {
        return new HubError(HubErrorType.ProblemDuringStopAttempt, "Hub has already been disposed or have never been initialized.");
    }
    
    public static HubError HubInactive()
    {
        return new HubError(HubErrorType.HubInactive, "Connection to server not established.");
    }
    
    public static HubError UnsupportedMessageType()
    {
        return new HubError(HubErrorType.UnsupportedMessageType, "This message type is not supported.");
    }
}