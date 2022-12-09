using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;

namespace DataAccessLayer.DataAccess;
internal class SignalDataAccess : BaseDataAccess<Signal>, ISignalDataAccess
{
    internal SignalDataAccess(string connectionString) : base(connectionString)
    {
        //TODO: Set the InsertCommand protected property of BaseDataAccess to an SQL Query that Inserts a new Signal and Inserts an entry in the signal bid member table
    }

    /*internal async override Task<int> CreateAsync(Signal signal) 
    {
        int signalId;
        string signalCommand = $"INSERT INTO Signal" +
            $"OUTPUT INSERTED.[Id]" +
            $"VALUES(@Id, @FromUtc, @ToUtc, @Price, @CurrencyId, @QuantityMw, @DirectionId)";
        try
        {
            using var connection = CreateConnection();
            signalId = await connection.QuerySingleAsync<int>(signalCommand, new {Id = signal.ID, FromUtc = signal.FromUtc, ToUtc = signal.ToUtc, Price = signal.Price, CurrencyId = signal.CurrencyId, QuantityMw = signal.QuantityMw, DirectionId = signal.DirectionId});
        }
    }*/
}
