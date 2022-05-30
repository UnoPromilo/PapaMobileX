using System.Diagnostics.CodeAnalysis;
using PapaMobileX.Shared.Results.Errors;

namespace PapaMobileX.Shared.Results;

public class Result<TError> where TError : Error
{
    protected Result(TError? error, bool isSuccess)
    {
        Error = error;
        IsSuccess = isSuccess;
    }

    public TError? Error { get; }

    [MemberNotNull(nameof(Error))]
    public bool IsFailed => !IsSuccess;

    public bool IsSuccess { get; }

    public static Result<TError> Ok()
    {
        return new Result<TError>(default, true);
    }

    public static Result<TError> Failed(TError error)
    {
        return new Result<TError>(error, false);
    }

    public static implicit operator Result<TError>(TError error)
    {
        return Failed(error);
    }
}

public class Result<TError, TData> : Result<TError>, IDisposable where TError : Error
{
    protected Result(TError error, TData data, bool isSuccess) : base(error, isSuccess)
    {
        Data = data;
    }

    public TData Data { get; }

    public static Result<TError, TData> Ok(TData data)
    {
        return new Result<TError, TData>(default, data, true);
    }

    public new static Result<TError, TData> Failed(TError error)
    {
        return new Result<TError, TData>(error, default, false);
    }

    public static implicit operator Result<TError, TData>(TError error)
    {
        return Failed(error);
    }

    public static implicit operator Result<TError, TData>(TData data)
    {
        return Ok(data);
    }

    public void Dispose()
    {
        if (Data is IDisposable disposable)
        {
            disposable.Dispose();
        }
    }
}