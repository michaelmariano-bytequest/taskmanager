namespace TaskManagerAPI.Core.Common;

public class Result
{
    protected Result(bool isSuccess, string errorMessage)
    {
        IsSuccess = isSuccess;
        ErrorMessage = errorMessage;
    }

    public bool IsSuccess { get; }
    public string ErrorMessage { get; }

    public static Result Success()
    {
        return new Result(true, null);
    }

    public static Result Failure(string errorMessage)
    {
        return new Result(false, errorMessage);
    }
}

public class Result<T> : Result
{
    protected Result(T value, bool isSuccess, string errorMessage)
        : base(isSuccess, errorMessage)
    {
        Value = value;
    }

    public T Value { get; }

    public static Result<T> Success(T value)
    {
        return new Result<T>(value, true, null);
    }

    public static Result<T> Failure(string errorMessage)
    {
        return new Result<T>(default, false, errorMessage);
    }
}