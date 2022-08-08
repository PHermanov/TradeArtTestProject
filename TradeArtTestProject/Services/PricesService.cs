using GraphQL;
using GraphQL.Client.Abstractions;
using TradeArtTestProject.Communication;
using TradeArtTestProject.Models;
using TradeArtTestProject.Services.Interfaces;

namespace TradeArtTestProject.Services;

public class PricesService : ServiceBase, IPricesService
{
    private const int AssetsCount = 100;
    private const int BatchSize = 20;

    private readonly IGraphQLClient _client;

    public PricesService(IGraphQLClient client)
        => _client = client;

    public async Task<ServiceResult<PriceViewModel>> GetPricesAsync()
    {
        var assetsResult = await QueryAssetsAsync();

        if (!assetsResult.Success)
        {
            return ErrorResult<PriceViewModel>(assetsResult.ErrorMessage!);
        }

        var assets = assetsResult.Data;

        if (assets == null || !assets.Any())
        {
            return ErrorResult<PriceViewModel>("No Assets found");
        }

        var marketsResult = await QueryMarketsForAssetNamesAsync(assets.Select(s => s.AssetSymbol).ToArray());

        var markets = marketsResult.Data;

        if (markets == null || !markets.Any())
        {
            return ErrorResult<PriceViewModel>("No Markets found");
        }

        return SuccessResult(FetchPrices(assets, markets, marketsResult.ErrorMessage));
    }

    public async Task<ServiceResult<Asset[]>> QueryAssetsAsync()
    {
        var assetsRequest = new GraphQLRequest
        {
            Query = @"
                query PageAssets {
                    assets(sort: {marketCapRank: ASC}) {
                        assetSymbol
                  }
                }"
        };

        var assetsResponse = await _client.SendQueryAsync<AssetsQueryResponse>(assetsRequest);

        return assetsResponse.IsOk()
            ? SuccessResult(assetsResponse.Data.Assets.Take(AssetsCount).ToArray())
            : ErrorResult<Asset[]>(assetsResponse.Errors != null
                ? string.Join("; ", assetsResponse.Errors.Select(e => e.Message))
                : "Unknown error");
    }

    public async Task<ServiceResult<Market[]>> QueryMarketsForAssetNamesAsync(string[] assetNames)
    {
        const string queryPattern = @" 
            query Markets
            {
              markets(
                sort: { baseSymbol: ASC }
                filter: {
                    baseSymbol: {_in: [{symbols}]}, 
                    quoteSymbol: {_eq: ""EUR""}
                }) 
                {
                    baseSymbol
                    marketSymbol
                    ticker {
                        lastPrice
                    }
                }
            }";

        var markets = new List<Market>();
        var errors = new List<string>();

        for (var batchPosition = 0; batchPosition < assetNames.Length; batchPosition += BatchSize)
        {
            var quotedSymbols = assetNames
                .Skip(batchPosition)
                .Take(BatchSize)
                .Select(s => $"\"{s}\"")
                .ToArray();

            var marketsRequest = new GraphQLRequest
            {
                Query = queryPattern.Replace("{symbols}", string.Join(",", quotedSymbols))
            };

            var marketsResponse = await _client.SendQueryAsync<MarketsQueryResponse>(marketsRequest);

            if (marketsResponse.IsOk())
            {
                // Given API doesn't allow filtering on ticker
                // "message": "Field \"ticker\" is not defined by type MarketFilter.",
                // So we have to do it here
                markets.AddRange(marketsResponse.Data.Markets.Where(m => m.Ticker != null));
            }
            else
            {
                // It is possible to face an error in one batch and data in another
                // so we need to keep both errors and the data
                if (marketsResponse.Errors != null)
                {
                    errors.AddRange(marketsResponse.Errors.Select(e => e.Message));
                }
                else
                {
                    errors.Add("Unknown error");
                }
            }
        }

        return errors.Any()
            ? CompletedWithErrorsResult(markets.ToArray(), string.Join("; ", errors.Distinct()))
            : SuccessResult(markets.ToArray());
    }

    private PriceViewModel FetchPrices(Asset[] assets, Market[] markets, string? errors = "")
    {
        var priceViewModel = new PriceViewModel();

        foreach (var asset in assets.Select(a => a.AssetSymbol))
        {
            var currencyModel = new CurrencyModel
            {
                CurrencyName = asset,
                Markets = markets.Where(m => string.Equals(m.BaseSymbol, asset, StringComparison.OrdinalIgnoreCase))
                    .Select(m => new MarketModel
                    {
                        Market = m.MarketSymbol,
                        Price = decimal.Parse(m.Ticker!.LastPrice)
                    }).ToArray()
            };

            priceViewModel.Data.Add(currencyModel);
        }

        if (!string.IsNullOrEmpty(errors))
        {
            priceViewModel.Errors = errors;
        }

        return priceViewModel;
    }
}

