using System.Text.Json.Serialization;

namespace TradeArtTestProject.Models;

public class MarketsQueryResponse
{
    [JsonPropertyName("markets")]
    public Market[] Markets { get; set; } = Array.Empty<Market>();
}

public class Market
{
    [JsonPropertyName("baseSymbol")]
    public string BaseSymbol { get; set; } = string.Empty;

    [JsonPropertyName("marketSymbol")]
    public string MarketSymbol { get; set; } = string.Empty;

    [JsonPropertyName("ticker")]
    public Ticker? Ticker { get; set; }
}

public class Ticker
{
    [JsonPropertyName("lastPrice")]
    public string LastPrice { get; set; } = string.Empty;
}