using DataAccessLayer.Attributes;

namespace DataAccessLayer.Models;

public class Bid
{
    [IsPrimaryKey]
    [IsAutoIncrementingID]
    public int Id { get; set; }
    public int ExternalId { get; set; }
    public int FromUTC { get; set; }
    public int ToUTC { get; set; }
    public decimal QuantityMw { get; set; }
    public decimal Price { get; set; }
    public int CurrencyId { get; set; }
}
