using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;

namespace DataAccessLayer.DataAccess;

internal class BidDataAccess : BaseDataAccess<Bid>, IBidDataAccess
{
    public BidDataAccess(string connectionString) : base(connectionString)
    {
    }
}
