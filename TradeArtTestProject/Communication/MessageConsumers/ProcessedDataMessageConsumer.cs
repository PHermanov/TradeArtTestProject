using SlimMessageBus;
using TradeArtTestProject.Communication.Messages;

namespace TradeArtTestProject.Communication.MessageConsumers;

public class ProcessedDataMessageConsumer : IConsumer<ProcessedDataMessage>
{
    private readonly IMessageResultsStorage _messageResultsStorage;

    public ProcessedDataMessageConsumer(IMessageResultsStorage messageResultsStorage)
        => _messageResultsStorage = messageResultsStorage;


    public Task OnHandle(ProcessedDataMessage message, string path)
    {
        _messageResultsStorage.Add(true);

        return Task.CompletedTask;
    }
}

