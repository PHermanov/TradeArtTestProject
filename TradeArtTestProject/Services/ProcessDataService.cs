using SlimMessageBus;
using TradeArtTestProject.Communication;
using TradeArtTestProject.Communication.Messages;
using TradeArtTestProject.Services.Interfaces;

namespace TradeArtTestProject.Services;

public class ProcessDataService : ServiceBase, IProcessDataService
{
    private readonly IMessageBus _bus;
    private readonly IMessageResultsStorage _messageResultsStorage;

    public ProcessDataService(IMessageBus bus, IMessageResultsStorage messageResultsStorage)
    {
        _bus = bus;
        _messageResultsStorage = messageResultsStorage;
    }

    public async Task<ServiceResult<string>> StartProcess()
    {
        // Emit some data
        PushMessages();

        // Ensure all data is processed
        await Task.Delay(200);

        if (_messageResultsStorage.Count == 1000)
        {
            return _messageResultsStorage.Results.All(m => m)
                ? SuccessResult("All data processed without errors")
                : ErrorResult<string>("Some data processed with errors");
        }

        return ErrorResult<string>("Some data was not processed");
    }

    // Function A
    // Runs a loop of 1...1000 and emits some data without blocking as fast as possible
    private void PushMessages()
    {
        Parallel.ForEach(Enumerable.Range(1, 1000),
            i =>
            {
                _bus.Publish(new EmitDataMessage { Data = i });
            });
    }
}
