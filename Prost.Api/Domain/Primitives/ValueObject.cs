namespace Prost.Api.Domain.Primitives;

internal abstract record ValueObject;

internal abstract record ValueObject<TValue> : ValueObject
    where TValue : notnull
{
    public TValue Value { get; }

    protected ValueObject(TValue value)
    {
        Value = value;
    }

    public override string ToString() =>
        Value.ToString() ?? string.Empty;
}
