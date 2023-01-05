using DataAccessLayer.Interfaces;
using MassTransit;
using TSOMessageHub.DTOs;
using aFRRService.DTOs;
using aFRRService.DTOs.DTOConverters;

namespace MessageConsumer.Consumers;

public class TsoSignalConsumer : IConsumer<TSOSignal>
{
    private readonly ILogger<TsoSignalConsumer> _logger;
    private readonly ISignalDataAccess _signalDataAccess;

    public TsoSignalConsumer(ISignalDataAccess signalDataAccess, ILogger<TsoSignalConsumer> logger)
    {
        _logger = logger;
        _signalDataAccess = signalDataAccess;
    }

    public async Task Consume(ConsumeContext<TSOSignal> context)
    {
        var tsoSignal = context.Message;
        _logger.LogInformation("Message Sent at: {Time}", tsoSignal.ReceivedUTC);
        _logger.LogInformation("Received a new message with id: {Id} and Quantity: {Quantity}", tsoSignal.SignalId, tsoSignal.QuantityMw);
        _logger.LogInformation("Current Time: {Time}", DateTimeOffset.Now.ToString("dd-MM-yyyy HH:mm:ss.fff"));
        SignalDTO signal = DTOConverter<TSOSignal,SignalDTO>.From(tsoSignal);
        signal.Id = tsoSignal.SignalId;
        //TODO: Use enum Direction instead
        signal.DirectionId = Convert.ToInt32(Math.Abs(signal.QuantityMw) == signal.QuantityMw);
        //TODO: Send Signal to Prioritization API and receive a populated one
        // SignalDTO signalWithAssets = PrioritizationDataAccess.GetPrioritizedSignal(signal)
        





    }
}

