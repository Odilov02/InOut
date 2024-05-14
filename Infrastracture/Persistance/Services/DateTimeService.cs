using Application.Common.Interfaces;

namespace Infrastructure.Persistance.Services;

public class DateTimeService : IDateTimeService
{
    public DateTime NowTime()
    {
        return DateTime.UtcNow.AddHours(5);
    }
}
