using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer;

internal static class Configuration
{
    internal const string CONNECTION_STRING = """
        Data Source=hildur.ucn.dk;
        Initial Catalog=dmai0920_1086320;
        Persist Security Info=True;
        User ID=dmai0920_1086320;
        Password=Password1!
        """;
}
