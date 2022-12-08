using DataAccess.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models;

internal class Bid
{
    [IsPrimaryKey]
    [IsAutoIncrementingID]
    public int Id { get; set; }
    public int ExternalId { get; set; }
    public int FromUTC { get; set; }
    public int ToUTC { get; set; }
    public decimal QuantityMw { get; set; }
    public decimal Price { get; set; }
    public int CurrencyId { get; set; }
}
