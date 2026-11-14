using System.Diagnostics.CodeAnalysis;

namespace Prost.Api.Domain.Primitives;

public enum ErrorType
{
    Failure,
    Unexpected,
    Validation,
    Conflict,
    NotFound,
    Unauthorized,
    Forbidden
}

[SuppressMessage(
    "Naming",
    "CA1716: Identifiers should not match keywords")]
public readonly record struct Nothing;

[SuppressMessage(
    "Naming",
    "CA1716: Identifiers should not match keywords")]
public readonly record struct Error
{
    public static readonly Nothing Nothing;

    public string Code { get; }

    public string Description { get; }

    public ErrorType Type { get; }

    private Error(string code, string description, ErrorType type)
    {
        Code = code;
        Description = description;
        Type = type;
    }

    public static Error Failure(
        string code = "Failure",
        string description = "A 'Failure' error has occurred.") =>
        new(code, description, ErrorType.Failure);

    public static Error Unexpected(
        string code = "Unexpected",
        string description = "A 'Unexpected' error has occurred.") =>
        new(code, description, ErrorType.Unexpected);

    public static Error Validation(
        string code = "Validation",
        string description = "A 'Validation' error has occurred.") =>
        new(code, description, ErrorType.Validation);

    public static Error Conflict(
        string code = "Conflict",
        string description = "A 'Conflict' error has occurred.") =>
        new(code, description, ErrorType.Conflict);

    public static Error NotFound(
        string code = "NotFound",
        string description = "A 'Not Found' error has occurred.") =>
        new(code, description, ErrorType.NotFound);

    public static Error Unauthorized(
        string code = "Unauthorized",
        string description = "A 'Unauthorized' error has occurred.") =>
        new(code, description, ErrorType.Unauthorized);

    public static Error Forbidden(
        string code = "Forbidden",
        string description = "A 'Forbidden' error has occurred.") =>
        new(code, description, ErrorType.Forbidden);
}

public interface IErrorOr;

public readonly union ErrorOr<TValue>(TValue, IReadOnlyList<Error>) :
    IErrorOr
    where TValue : notnull
{
    public bool IsSuccess => this is TValue;

    public bool IsError => this is IReadOnlyList<Error>;

    public IReadOnlyList<Error> Errors => this switch
    {
        IReadOnlyList<Error> errors => errors,
        TValue => throw new InvalidOperationException(
            "Cannot access errors when outcome is success.")
    };

    public TValue Unwrap() => this switch
    {
        TValue value => value,
        IReadOnlyList<Error> => throw new InvalidOperationException(
            "Cannot unwrap value when outcome is error.")
    };

    public TNextValue Match<TNextValue>(Func<TValue, TNextValue> onValue, Func<IReadOnlyList<Error>, TNextValue> onError) => this switch
    {
        TValue value => onValue(value),
        IReadOnlyList<Error> errors => onError(errors),
    };

    public static implicit operator ErrorOr<TValue>(Error error) => new([error]);

    public static implicit operator TValue(ErrorOr<TValue> errorOr) => errorOr switch
    {
        TValue value => value,
        IReadOnlyList<Error> => throw new InvalidOperationException(
            "Cannot access the value when outcome is error.")
    };
}
