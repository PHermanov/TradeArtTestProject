using TradeArtTestProject.Communication;

namespace TradeArtTestProject.Services.Interfaces;

public interface IShaService
{
    Task<ServiceResult<string>> CalculateShaAsync(string filePath);
}