using TradeArtTestProject.Services.Interfaces;
using Xunit;

namespace TradeArtTestProject.Tests;

public class PricesServiceTests
{
    private readonly IPricesService _pricesService;

    public PricesServiceTests(IPricesService pricesService)
        => _pricesService = pricesService;

    [Fact]
    public async Task QueryAssets_ReturnsResults()
    {
        var assetsResult = await _pricesService.QueryAssetsAsync();

        Assert.True(assetsResult.Success);
        Assert.NotNull(assetsResult.Data);
        Assert.NotEmpty(assetsResult.Data);
    }

    [Theory]
    [InlineData("BTC", "ETH")]
    [InlineData("DOGE")]
    public async Task QueryMarkets_ReturnsResults(params string[] assetNames)
    {
        var marketsResult = await _pricesService.QueryMarketsForAssetNamesAsync(assetNames);

        Assert.True(marketsResult.Success);
        Assert.NotNull(marketsResult.Data);
        Assert.NotEmpty(marketsResult.Data);
    }
}
