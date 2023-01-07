using aFRRService.DTOs;
using aFRRService.DTOs.DTOConverters;
using BaseDataAccess.Interfaces;
using BaseDataAccess.Models;
using DataAccessLayer.Interfaces;
using MassTransit;
using TSOMessageHub.DTOs;

namespace MessageConsumer.Consumers;

public class TsoSignalConsumer : IConsumer<TSOSignal>
{
    private readonly ILogger<TsoSignalConsumer> _logger;
    private readonly IPrioritizationDataAccess _prioritizationDataAccess;
    private readonly IRemoteControlDataAccess _remoteControlDataAccess;
    private readonly ISignalDataAccess _signalDataAccess;

    public TsoSignalConsumer(ILogger<TsoSignalConsumer> logger, 
        IPrioritizationDataAccess prioritizationDataAccess, 
        IRemoteControlDataAccess remoteControlDataAccess,
        ISignalDataAccess signalDataAccess)
    {
        _logger = logger;
        _prioritizationDataAccess = prioritizationDataAccess;
        _remoteControlDataAccess = remoteControlDataAccess;
        _signalDataAccess = signalDataAccess;
    }

    public async Task Consume(ConsumeContext<TSOSignal> context)
    {
        var tsoSignal = context.Message;
        SignalDTO signalDTO = new();

        //TODO: split in different try catches and make error handling more specific
        try
        {
            signalDTO = new()
            {
                Id = tsoSignal.SignalId,
                BidId = Int32.Parse(tsoSignal.BidId),
                ReceivedUtc = DateTime.Parse(tsoSignal.ReceivedUTC),
                QuantityMw = Math.Abs(tsoSignal.QuantityMw),
                Direction = tsoSignal.QuantityMw > 0 ? Direction.Up : Direction.Down
            };

            signalDTO = await _prioritizationDataAccess.GetAsync(signalDTO);
            
        }
        catch(Exception ex)
        {
            _logger.LogError("Failed to create SignalDTO");
            _logger.LogDebug("{Message}", ex.Message);
            return;
        }

        try
        {
            bool signalSent = await _remoteControlDataAccess.SendAsync(signalDTO);
            if (!signalSent)
            {
                _logger.LogError("Failed to send the SignalDTO to the RemoteControlAPI");
                return;
            }
        }
        catch(Exception ex)
        {
            _logger.LogError("Failed to send the SignalDTO to the RemoteControlAPI");
            _logger.LogDebug("{Message}", ex.Message);
            return;
        }

        signalDTO.SentUtc = DateTime.UtcNow;
        Signal signal = DTOConverter<SignalDTO, Signal>.From(signalDTO);
        signal.DirectionId = (int)signalDTO.Direction;
        int signalId = await _signalDataAccess.CreateAsync(signal);
        



        _logger.LogInformation("Message Sent at: {Time}", context.Message.ReceivedUTC);
        _logger.LogInformation("Received a new message with id: {Id} and Quantity: {Quantity}", context.Message.SignalId, context.Message.QuantityMw);
        _logger.LogInformation("Current Time: {Time}", DateTimeOffset.Now.ToString("dd-MM-yyyy HH:mm:ss.fff"));

    }
}

