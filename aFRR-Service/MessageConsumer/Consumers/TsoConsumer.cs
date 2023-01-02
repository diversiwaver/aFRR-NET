﻿using MassTransit;

namespace MessageConsumer.Consumers;

public class TsoConsumer : IConsumer<TSOSignal>
{
    private readonly ILogger<TsoConsumer> _logger;

    public TsoConsumer(ILogger<TsoConsumer> logger)
    {
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<TSOSignal> context)
    {
        _logger.LogInformation("Message Sent at: {Time}", context.Message.ReceivedUTC);
        _logger.LogInformation("Received a new message with id: {Id} and Quantity: {Quantity}", context.Message.SignalId, context.Message.QuantityMw);
        _logger.LogInformation("Current Time: {Time}", DateTimeOffset.Now.ToString("dd-MM-yyyy HH:mm:ss.fff"));
    }
}

