public sealed class FilterParameters
{
    public decimal MinPrice { get; set; } = 0;
    public decimal MaxPrice { get; set; } = decimal.MaxValue;
    public Boolean LowStock { get; set; } = false;
}