using BaseDataAccess.Interfaces;
using BaseDataAccess.Models;

namespace BaseDataAccess.DataAccess;

internal class BidDataAccess : BaseDataAccess<Bid>, IBidDataAccess
{
    public BidDataAccess(string connectionString) : base(connectionString)
    {
    }
}
