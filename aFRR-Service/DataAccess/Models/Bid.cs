using DataAccessLayer.Attributes;

namespace DataAccessLayer.Models;

public class Bid
{
    [IsPrimaryKey]
    [IsAutoIncrementingID]
    public int Id { get; init; }
    public Guid ExternalId { get; init; }
    public DateTime FromUtc { get; init; }
    public DateTime ToUtc { get; init; }
    public decimal QuantityMw { get; init; }
    public decimal Price { get; init; }
    public int CurrencyId { get; init; }

    public override string ToString()
    {
        return $"Bid:{{Id: {Id}, ExternalId: {ExternalId}, FromUtc: {FromUtc}, ToUtc: {ToUtc}, QuantityMw: {QuantityMw}, Price: {Price}, CurrencyId: {CurrencyId}}}";
    }
}
