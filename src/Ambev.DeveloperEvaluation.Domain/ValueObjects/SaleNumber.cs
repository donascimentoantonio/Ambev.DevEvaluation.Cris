namespace Ambev.DeveloperEvaluation.Domain.ValueObjects;

/// <summary>
/// Value Object representing a unique order identifier.
/// </summary>
public readonly struct SaleNumber
{
    public string Value { get; }

    public SaleNumber()
    {
        Value = GenerateSaleNumber();
    }

    public SaleNumber(string value)
    {
        Value = value;
    }

    private static string GenerateSaleNumber()
    {
        var random = new Random();
        return random.Next(1000000, 9999999).ToString();
    }

    public override string ToString() => Value;
}
