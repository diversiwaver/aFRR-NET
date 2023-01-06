using BaseDataAccess;
using BaseDataAccess.DataAccess;
using DataAccessLayer.DataAccess;

namespace DataAccessLayer;
public static class DataAccessFactory
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

    public static T GetDataAccess<T>(HttpClient client) where T : class
    {
        switch (typeof(T).Name)
        {
            case "IPrioritizationDataAccess": return new PrioritizationDataAccess(client) as T;
            case "IRemoteControlDataAccess": return new RemoteControlDataAccess(client) as T;
            default:
                throw new ArgumentException($"Unknown type {typeof(T).FullName}");
        }
    }
}