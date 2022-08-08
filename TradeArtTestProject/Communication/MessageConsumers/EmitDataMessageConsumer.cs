using SlimMessageBus;
using TradeArtTestProject.Communication.Messages;

namespace TradeArtTestProject.Communication.MessageConsumers;

public class EmitDataMessageConsumer : IConsumer<EmitDataMessage>
{
    private readonly IMessageBus _bus;

    public EmitDataMessageConsumer(IMessageBus bus)
        => _bus = bus;


    // Function B
    public async Task OnHandle(EmitDataMessage message, string path)
    {
        // has a processing delay of this data of 0.1 second and returns true, when it has finished
        await Task.Delay(100);
        await _bus.Publish(new ProcessedDataMessage { Result = true });
    }
}

