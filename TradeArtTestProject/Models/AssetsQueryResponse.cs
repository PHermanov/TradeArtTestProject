using System.Text.Json.Serialization;

namespace TradeArtTestProject.Models;

public class AssetsQueryResponse
{
    [JsonPropertyName("assets")] 
    public Asset[] Assets { get; set; } = Array.Empty<Asset>();
}

public class Asset
{
    [JsonPropertyName("assetSymbol")]
    public string AssetSymbol { get; set; } = string.Empty;
}
