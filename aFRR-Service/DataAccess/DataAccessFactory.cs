using DataAccessLayer.DataAccess;

namespace DataAccessLayer;
public class DataAccessFactory
{
    public static T GetDataAccess<T>(string connectionString) where T : class
    {
        switch (typeof(T).Name)
        {
            case "IBookingDataAccess": return new SignalDataAccess(connectionString) as T;
            case "ICinemaDataAccess": return new BidDataAccess(connectionString) as T;
            default:
                break;
        }
        throw new ArgumentException($"Unknown type {typeof(T).FullName}");
    }
}

