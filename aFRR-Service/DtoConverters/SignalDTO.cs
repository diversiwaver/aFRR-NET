using System.Text.Json.Serialization;

namespace aFRRService.DTOs;

public class SignalDTO
{
    [JsonPropertyName ("id")]
    public int Id { get; set; }

    [JsonPropertyName("ReceivedUtc")]
    public DateTime ReceivedUtc { get; set; }

    [JsonPropertyName("SentUtc")]
    public DateTime SentUtc { get; set; }

    [JsonPropertyName("quantityMw")]
    public decimal QuantityMw { get; set; }

    [JsonPropertyName("direction")]
    public Direction Direction { get; set; }

    [JsonPropertyName("bidId")]
    public int BidId { get; set; }

    [JsonPropertyName("assetsToRegulate")]
    public IEnumerable<AssetDTO>? AssetsToRegulate { get; set; }
}
