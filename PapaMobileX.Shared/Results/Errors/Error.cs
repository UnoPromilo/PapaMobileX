namespace PapaMobileX.Shared.Results.Errors;

public class Error
{
    public Error(string message)
    {
        Message = message;
    }

    protected Error() { }

    public virtual string Message { get; }
}