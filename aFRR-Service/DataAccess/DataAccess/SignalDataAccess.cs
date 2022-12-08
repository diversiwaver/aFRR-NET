using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;

namespace DataAccessLayer.DataAccess;
internal class SignalDataAccess : BaseDataAccess<Signal>, ISignalDataAccess
{
    public SignalDataAccess(string connectionString) : base(connectionString)
    {
    }
}
