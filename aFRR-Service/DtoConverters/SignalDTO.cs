using System.Text.Json.Serialization;

namespace aFRRService.DTOs;

public class SignalDTO
{
    [JsonPropertyName ("id")]
    public int Id { get; set; }

    [JsonPropertyName("fromUtc")]
    public DateTime FromUtc { get; set; }

    [JsonPropertyName("toUtc")]
    public DateTime ToUtc { get; set; }

    [JsonPropertyName("price")]
    public decimal Price { get; set; }

    [JsonPropertyName("currencyId")]
    public int CurrencyId { get; set; }

    [JsonPropertyName("quantityMw")]
    public decimal QuantityMw { get; set; }

    [JsonPropertyName("directionId")]
    public int DirectionId { get; set; }

    [JsonPropertyName("bidId")]
    public int BidId { get; set; }

    [JsonPropertyName("assetsToRegulate")]
    public IEnumerable<AssetDTO>? AssetsToRegulate { get; set; }
}
