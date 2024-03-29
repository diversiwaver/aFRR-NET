﻿using BaseDataAccess.Interfaces;
using BaseDataAccess.Models;

namespace BaseDataAccess.DataAccess;
internal class SignalDataAccess : BaseDataAccess<Signal>, ISignalDataAccess
{
    internal SignalDataAccess(string connectionString) : base(connectionString)
    {
        InsertCommand = """
            SET XACT_ABORT ON;

            SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;

            BEGIN TRANSACTION; 

            INSERT INTO Signal (ReceivedUtc, SentUtc, QuantityMw, DirectionId) OUTPUT INSERTED.Id VALUES (@ReceivedUtc, @SentUtc, @QuantityMw, @DirectionId);

            DECLARE @GeneratedId INT;
            SET @GeneratedId = SCOPE_IDENTITY();

            INSERT INTO BidSignalMember (BidId, SignalId) VALUES (@BidId, @GeneratedId)

            COMMIT; 
            """;
    }
}
