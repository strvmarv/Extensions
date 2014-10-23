using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Extensions
{
    public static class DateTimeExtensions
    {
        public enum DateRange
        {
            Today = 1,
            Yesterday = 2,
            ThisWeek = 3,
            LastWeek = 4,
            ThisMonth = 5,
            LastMonth = 6,
            ThisYear = 7,
            LastYear = 8,
        }

        public static double DateTimeToUnixTimeStamp(DateTime dateTime)
        {
            return (dateTime - new DateTime(1970, 1, 1).ToLocalTime()).TotalSeconds;
        }

        public static DateTime FirstDayOfYear(int? year = null)
        {
            if (year == null) year = DateTime.Now.Year;

            return DateTime.Parse("01/01/" + year);
        }

        public static string GetDateFilenameString()
        {
            return
                DateTime.Now.Year.ToString()
                + DateTime.Now.Month.ToString().PadLeft(2, '0')
                + DateTime.Now.Day.ToString().PadLeft(2, '0');
        }

        public static DateRangeResult GetDateRange(DateRange dateRange, DateTime sourceDate)
        {
            var range = new DateRangeResult();

            switch (dateRange)
            {
                // Today
                case DateRange.Today:
                    range.StartDate = sourceDate.Date;
                    range.EndDate = range.StartDate.AddDays(1).Date;
                    break;

                // Yesterday
                case DateRange.Yesterday:
                    range.StartDate = sourceDate.AddDays(-1).Date;
                    range.EndDate = range.StartDate.AddDays(1).Date;
                    break;

                // This Week
                case DateRange.ThisWeek:
                    var thisWeekToday = sourceDate;
                    var thisWeekStartDelta = DayOfWeek.Sunday - thisWeekToday.DayOfWeek;
                    var thisWeekEndDelta = DayOfWeek.Saturday - thisWeekToday.DayOfWeek;
                    range.StartDate = thisWeekToday.AddDays(thisWeekStartDelta).Date;
                    range.EndDate = thisWeekToday.AddDays(thisWeekEndDelta + 1).Date;
                    break;

                // Last Week
                case DateRange.LastWeek:
                    var lastWeekToday = sourceDate.AddDays(-7);
                    var lastWeekStartDelta = DayOfWeek.Sunday - lastWeekToday.DayOfWeek;
                    var lastWeekEndDelta = DayOfWeek.Saturday - lastWeekToday.DayOfWeek;
                    range.StartDate = lastWeekToday.AddDays(lastWeekStartDelta).Date;
                    range.EndDate = lastWeekToday.AddDays(lastWeekEndDelta + 1).Date;
                    break;

                // This Month
                case DateRange.ThisMonth:
                    var thisMonthToday = sourceDate;
                    range.StartDate = new DateTime(thisMonthToday.Year, thisMonthToday.Month, 1).Date;
                    range.EndDate = new DateTime(thisMonthToday.Year, thisMonthToday.Month + 1, 1).Date;
                    break;

                // Last Month
                case DateRange.LastMonth:
                    var lastMonthToday = sourceDate.AddMonths(-1);
                    range.StartDate = new DateTime(lastMonthToday.Year, lastMonthToday.Month, 1).Date;
                    range.EndDate = new DateTime(lastMonthToday.Year, lastMonthToday.Month + 1, 1).Date;
                    break;

                // This Year
                case DateRange.ThisYear:
                    var thisYearToday = sourceDate;
                    range.StartDate = new DateTime(thisYearToday.Year, 1, 1).Date;
                    range.EndDate = new DateTime(thisYearToday.Year + 1, 1, 1).Date;
                    break;

                // Last Year
                case DateRange.LastYear:
                    var lastYearToday = sourceDate.AddYears(-1);
                    range.StartDate = new DateTime(lastYearToday.Year, 1, 1).Date;
                    range.EndDate = new DateTime(lastYearToday.Year + 1, 1, 1).Date;
                    break;

                default:
                    throw new ArgumentException("Date range " + dateRange.ToString() + " not found.");
            }

            return range;
        }

        public static string GetDateTimeFilenameString()
        {
            return
                DateTime.Now.Year.ToString()
                + DateTime.Now.Month.ToString().PadLeft(2, '0')
                + DateTime.Now.Day.ToString().PadLeft(2, '0')
                + DateTime.Now.Hour.ToString().PadLeft(2, '0')
                + DateTime.Now.Minute.ToString().PadLeft(2, '0')
                + DateTime.Now.Second.ToString().PadLeft(2, '0');
        }

        public static string GetTimeFilenameString()
        {
            return
                DateTime.Now.Hour.ToString().PadLeft(2, '0')
                + DateTime.Now.Minute.ToString().PadLeft(2, '0')
                + DateTime.Now.Second.ToString().PadLeft(2, '0');
        }

        public static DateTime LastDayOfYear(int? year = null)
        {
            if (year == null) year = DateTime.Now.Year;

            return DateTime.Parse("12/31/" + year);
        }

        public static double ToJulianDate(this DateTime date)
        {
            return date.ToOADate() + 2415018.5;
        }

        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            var dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }
    }

    public class DateRangeResult
    {
        public DateTime EndDate { get; set; }
        public DateTime StartDate { get; set; }
    }
}
