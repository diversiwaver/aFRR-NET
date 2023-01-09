using BaseDataAccess.Attributes;

namespace BaseDataAccess.Models;

public class Signal
{
    [IsPrimaryKey]
    [IsAutoIncrementingID]
    public int Id { get; init; }
    public DateTime ReceivedUtc { get; init; }
    public DateTime SentUtc { get; init; }
    public decimal QuantityMw { get; init; }
    public int DirectionId { get; set; }
    [ExcludeFromDataAccess]
    public int BidId { get; init; }

    public override string ToString()
    {
        return $"Signal:{{Id={Id}, ReceivedUtc={ReceivedUtc}, SentUtc={SentUtc}, QuantityMw={QuantityMw}, DirectionId={DirectionId}, BidId={BidId}}}";
    }
}
