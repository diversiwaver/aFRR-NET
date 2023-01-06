using System.Text.Json.Serialization;

namespace aFRRService.DTOs;

public class AssetDTO
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("assetGroupId")]
    public int AssetGroupId { get; init; }

    [JsonPropertyName("location")]
    public string Location { get; init; }

    [JsonPropertyName("capacityMw")]
    public int CapacityMw { get; init; }

    [JsonPropertyName("regulationPercentage")]
    public decimal RegulationPercentage { get; set; }

    public override string ToString()
    {
        return $"PrioritizationService.DTOs.AssetDTO {{Id: {Id}, AssetGroupId: {AssetGroupId}, Location: {Location}, CapacityMw: {CapacityMw}, RegulationPercentage: {RegulationPercentage}}}";
    }
}