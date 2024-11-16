namespace TaskManagerAPI.Core.Common;

/// <summary>
/// Represents the result of an operation, containing information about success or failure and an optional error message.
/// </summary>
public class Result
{
    protected Result(bool isSuccess, string errorMessage)
    {
        IsSuccess = isSuccess;
        ErrorMessage = errorMessage;
    }

    public bool IsSuccess { get; }
    public string ErrorMessage { get; }

    /// <summary>
    /// Creates a new Result instance indicating a successful operation.
    /// </summary>
    /// <returns>A Result instance with IsSuccess set to true and ErrorMessage set to null.</returns>
    public static Result Success()
    {
        return new Result(true, null);
    }

    /// <summary>
    /// Creates a new Result instance indicating a failed operation.
    /// </summary>
    /// <param name="errorMessage">A description of the error that caused the operation to fail.</param>
    /// <returns>A Result instance with IsSuccess set to false and ErrorMessage populated with the provided error message.</returns>
    public static Result Failure(string errorMessage)
    {
        return new Result(false, errorMessage);
    }
}

/// <summary>
/// Represents the outcome of an operation, providing information on whether it succeeded or failed, along with an optional error message.
/// </summary>
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