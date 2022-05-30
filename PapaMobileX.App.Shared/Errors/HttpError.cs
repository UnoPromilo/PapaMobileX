using System.Net;
using PapaMobileX.Shared.Results.Errors;

namespace PapaMobileX.App.Shared.Errors;

public class HttpError : Error
{
    private readonly Exception? _requestException;

    protected HttpError(bool wasCanceled = false,
                        Exception? requestException = null,
                        HttpStatusCode? httpStatusCode = null)
    {
        WasCanceled = wasCanceled;
        _requestException = requestException;
        StatusCode = httpStatusCode;
    }

    public bool WasCanceled { get; }

    public bool HasStatusCode => StatusCode != null;

    public HttpStatusCode? StatusCode { get; }

    public static HttpError WasCancelled()
    {
        return new HttpError(true);
    }

    public static HttpError UnsuccessfulRequest(HttpStatusCode statusCode)
    {
        return new HttpError(httpStatusCode: statusCode);
    }

    public static HttpError HttpRequestException(Exception ex)
    {
        return new HttpError(false, ex);
    }
}

public class HttpError<T> : HttpError
{
    protected HttpError(bool wasCanceled = false,
                        HttpRequestException? requestException = null,
                        HttpStatusCode? httpStatusCode = null,
                        T? details = default) : base(wasCanceled, requestException, httpStatusCode)
    {
        Details = details;
    }

    public bool HasDetails => Details != null;

    public T? Details { get; }

    public new static HttpError<T> WasCancelled()
    {
        return new HttpError<T>(true);
    }

    public static HttpError<T> UnsuccessfulRequest(HttpStatusCode statusCode, T details)
    {
        return new HttpError<T>(httpStatusCode: statusCode, details: details);
    }

    public static HttpError<T> HttpRequestException(HttpRequestException ex)
    {
        return new HttpError<T>(false, ex);
    }
}