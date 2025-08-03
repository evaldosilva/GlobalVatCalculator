namespace Domain.VatCalculator.Validation;

public class Result
{
    private Result(bool isSuccess, Error error, string[] memberNames)
    {
        if (isSuccess && error != Error.None || isSuccess is false && error == Error.None)
            throw new ArgumentException("Invalid result creation", nameof(error));

        IsSuccess = isSuccess;
        Error = error;
        MemberNames = memberNames;
    }
    public bool IsSuccess { get; }
    public bool IsFailure { get => IsSuccess is false; }
    public Error Error { get; }
    public string[] MemberNames { get; }

    public static Result Success() => new(true, Error.None, []);
    public static Result Failure(Error error, string[] memberNames) => new(false, error, memberNames);
}