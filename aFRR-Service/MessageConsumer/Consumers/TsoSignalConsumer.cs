﻿using aFRRService.DTOs;
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
    private readonly IRemoteSignalDataAccess _remoteSignalDataAccess;
    private readonly ISignalDataAccess _signalDataAccess;

    public TsoSignalConsumer(ILogger<TsoSignalConsumer> logger, 
        IPrioritizationDataAccess prioritizationDataAccess, 
        IRemoteSignalDataAccess remoteSignalDataAccess,
        ISignalDataAccess signalDataAccess)
    {
        _logger = logger;
        _prioritizationDataAccess = prioritizationDataAccess;
        _remoteSignalDataAccess = remoteSignalDataAccess;
        _signalDataAccess = signalDataAccess;
    }

    public async Task Consume(ConsumeContext<TSOSignal> context)
    {
        var tsoSignal = context.Message;
        SignalDTO signalDTO = new()
        {
            Id = tsoSignal.SignalId,
            BidId = Int32.Parse(tsoSignal.BidId),
            ReceivedUtc = DateTime.TryParse(tsoSignal.ReceivedUTC, out var fromUtc)
            ? fromUtc : DateTime.UtcNow,
            QuantityMw = Math.Abs(tsoSignal.QuantityMw),
            Direction = tsoSignal.QuantityMw > 0 ? Direction.Up : Direction.Down
        };
        
        try
        {
            signalDTO = await _prioritizationDataAccess.GetAsync(signalDTO);
            bool signalSent = await _remoteSignalDataAccess.SendAsync(signalDTO);
            if (signalSent)
            {
                signalDTO.SentUtc = DateTime.UtcNow;
                Signal signal = DTOConverter<SignalDTO, Signal>.From(signalDTO);
                signal.DirectionId = signalDTO.Direction == Direction.Up ? 0 : 1;
                await _signalDataAccess.CreateAsync(signal);
            }
            else
            {
                _logger.LogError("Failed to send the SignalDTO to the RemoteControlAPI");
            }
        }
        catch(Exception ex)
        {
            _logger.LogError("Failed to consume Signal with id {id}", signalDTO.Id);
            _logger.LogDebug("{Message}", ex.Message);
        }
    }
}

