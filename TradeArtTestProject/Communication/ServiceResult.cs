namespace TradeArtTestProject.Communication;

public class ServiceResult<TData>
{
    public bool Success { get; init; }

    public TData? Data { get; init; }

    public string? ErrorMessage { get; init; }
}

