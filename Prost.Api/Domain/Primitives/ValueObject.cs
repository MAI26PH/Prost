namespace Prost.Api.Domain.Primitives;

public interface IValueObject;

public abstract record ValueObject<TValue> :
    IValueObject
    where TValue : notnull
{
    public TValue Value { get; }

    protected ValueObject(TValue value)
    {
        Value = value;
    }

    public static implicit operator TValue(ValueObject<TValue> valueObject) =>
        valueObject.Value;

    public override string ToString() =>
        Value.ToString() ?? string.Empty;
}
