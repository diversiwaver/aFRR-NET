﻿using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;

namespace DataAccessLayer.DataAccess;
internal class SignalDataAccess : BaseDataAccess<Signal>, ISignalDataAccess
{
    internal SignalDataAccess(string connectionString) : base(connectionString)
    {
        InsertCommand = """
            SET XACT_ABORT ON;

            BEGIN TRANSACTION; 

            INSERT INTO Signal (FromUtc, ToUtc, Price, CurrencyId, QuantityMw, DirectionId) OUTPUT INSERTED.Id VALUES (@FromUtc, @ToUtc, @Price, @CurrencyId, @QuantityMw, @DirectionId);

            DECLARE @GeneratedId INT;
            SET @GeneratedId = SCOPE_IDENTITY();

            INSERT INTO BidSignalMember (BidId, SignalId) VALUES (0, @GeneratedId)

            COMMIT; 
            """;
    }
}
