using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Attributes;

namespace DataAccessLayer.Models;

internal class Signal
{
    [IsPrimaryKey]
    [IsAutoIncrementingID]
    public int ID { get; set; }
    public DateTime FromUtc { get; set; }
    public DateTime ToUtc { get; set; }
    public decimal Price { get; set; }
    public int CurrencyId { get; set; }
    public decimal QuantityMw { get; set; }
    public int DirectionId { get; set; }
}
