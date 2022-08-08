using System.Text.Json.Serialization;

namespace TradeArtTestProject.Models;

public class PriceViewModel
{

    [JsonPropertyName("data")]
    public List<CurrencyModel> Data { get; set; } = new ();

    [JsonPropertyName("errors")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Errors = null;
}

public class CurrencyModel
{

    [JsonPropertyName("currency")]
    public string CurrencyName { get; init; } = string.Empty;

    [JsonPropertyName("markets")]
    public MarketModel[] Markets { get; set; } = Array.Empty<MarketModel>();
}

public class MarketModel
{
    [JsonPropertyName("market")]
    public string Market { get; init; } = string.Empty;

    [JsonPropertyName("price")]
    public decimal Price { get; init; }
}

