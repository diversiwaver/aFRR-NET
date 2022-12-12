namespace WebAPI.DTOs;   

public class BidDto
{
    public int Id { get; set; }
    public int ExternalId { get; set; }
    public DateTime FromUtc { get; set; }
    public DateTime ToUtc { get; set; }
    public decimal QuantityMw { get; set; }
    public decimal Price { get; set; }
    public int CurrencyId { get; set; }
}
