using DataAccessLayer.DataAccess;

namespace DataAccessLayer;
public class DataAccessFactory
{
    public static T GetDataAccess<T>(string connectionString) where T : class
    {
        switch (typeof(T).Name)
        {
            case "ISignalDataAccess": return new SignalDataAccess(connectionString) as T;
            case "IBidDataAccess": return new BidDataAccess(connectionString) as T;
            default:
                throw new ArgumentException($"Unknown type {typeof(T).FullName}");
        }
    }
}