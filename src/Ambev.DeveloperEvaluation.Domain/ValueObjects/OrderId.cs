namespace Ambev.DeveloperEvaluation.Domain.ValueObjects;

/// <summary>
/// Value Object representing a unique order identifier.
/// </summary>
public readonly struct OrderId
{
    public string Value { get; }

    public OrderId()
    {
        Value = GenerateOrderId();
    }

    public OrderId(string value)
    {
        Value = value;
    }

    private static string GenerateOrderId()
    {
        var random = new Random();
        return random.Next(1000000, 9999999).ToString();
    }

    public override string ToString() => Value;
}
