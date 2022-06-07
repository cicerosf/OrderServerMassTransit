
using MassTransit;
using Newtonsoft.Json;
using Shared.Model;

public class TicketConsumer : IConsumer<Ticket>
{
    private readonly ILogger<TicketConsumer> _logger;

    public TicketConsumer(ILogger<TicketConsumer> logger)
    {
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<Ticket> context)
    {
        await Console.Out.WriteLineAsync(context.Message.UserName);

        var jsonMessage = JsonConvert.SerializeObject(context.Message);
        Console.WriteLine($"OrderCreated message: {jsonMessage}");

        _logger.LogInformation($"new message received: " +
            $"{context.Message.UserName} - {context.Message.Location}");
    }
}