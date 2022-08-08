using TradeArtTestProject.Communication;

namespace TradeArtTestProject.Services.Interfaces;

public interface IProcessDataService
{
    Task<ServiceResult<string>> StartProcess();
}