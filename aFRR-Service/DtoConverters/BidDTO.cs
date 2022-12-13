namespace WebAPI.DTOs;   

public class BidDTO
{
    public int Id { get; set; }
    public Guid ExternalId { get; set; }
    public DateTime FromUtc { get; set; }
    public DateTime ToUtc { get; set; }
    public decimal QuantityMw { get; set; }
    public decimal Price { get; set; }
    public int CurrencyId { get; set; }
}
