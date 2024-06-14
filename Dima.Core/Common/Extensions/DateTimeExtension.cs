namespace Dima.Core.Common.Extensions;

public static class DateTimeExtension
{
    public static DateTime GetFirstDay(this DateTime date, int? year = null, int? month = null)
    {
        return new DateTime(year ?? date.Year, month ?? date.Month, day: 1);
    }

    public static DateTime GetLatsDay(this DateTime date, int? year = null, int? month = null)
    {
        return new DateTime(year ?? date.Year, month ?? date.Month, day: 1)
                   .AddMonths(1).AddDays(-1);
    }
}
