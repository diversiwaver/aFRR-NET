using DataAccessLayer.Attributes;

namespace DataAccessLayer.Models;

public class Signal
{
    [IsPrimaryKey]
    [IsAutoIncrementingID]
    public int Id { get; init; }
    public DateTime FromUtc { get; init; }
    public DateTime ToUtc { get; init; }
    public decimal Price { get; init; }
    public int CurrencyId { get; init; }
    public decimal QuantityMw { get; init; }
    public int DirectionId { get; init; }
    public int BidId { get; init; }
}
