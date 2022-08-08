using TradeArtTestProject.Communication;
using TradeArtTestProject.Models;

namespace TradeArtTestProject.Services.Interfaces;

public interface IPricesService
{
    Task<ServiceResult<PriceViewModel>> GetPricesAsync();
    Task<ServiceResult<Asset[]>> QueryAssetsAsync();
    Task<ServiceResult<Market[]>> QueryMarketsForAssetNamesAsync(string[] assetsNames);
}