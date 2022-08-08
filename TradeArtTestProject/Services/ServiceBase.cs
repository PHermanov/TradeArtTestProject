using TradeArtTestProject.Communication;

namespace TradeArtTestProject.Services;

public class ServiceBase
{
    protected ServiceResult<TData> ErrorResult<TData>(string errorMessage)
        => new() { Success = false, ErrorMessage = errorMessage };

    protected ServiceResult<TData> SuccessResult<TData>(TData data)
        => new() { Success = true, Data = data };

    protected ServiceResult<TData> CompletedWithErrorsResult<TData>(TData data, string errorMessage)
        => new() { Success = true, Data = data, ErrorMessage = errorMessage };
}
