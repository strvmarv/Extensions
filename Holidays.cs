using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Extensions
{
    // Credit: Unknown
    public static class Holidays
    {
        public static DateTime Easter(int year)
        {
            //This algorithm is a C# conversion of the one described at
            //http://www.assa.org.au/edm.html#Computer
            int firstDigit;
            int remain19;
            int temp;

            int tableA;
            int tableB;
            int tableC;
            int tableD;
            int tableE;

            int d; //day of easter
            int m; //month of easter

            firstDigit = year / 100;
            remain19 = year % 19;

            temp = (firstDigit - 15) / 2 + 202 - 11 * remain19;

            int[] substract1 = { 21, 24, 25, 27, 28, 29, 30, 31, 32, 34, 35, 38 };
            var subtract1List = new List<int>(substract1);
            if (subtract1List.Contains(firstDigit))
            {
                temp = temp - 1;
            }

            int[] substract2 = { 33, 36, 37, 39, 40 };
            var subtract2List = new List<int>(substract2);
            if (subtract2List.Contains(firstDigit))
            {
                temp = temp - 2;
            }

            temp = temp % 30;
            tableA = temp + 21;
            if (temp == 29)
            {
                tableA = tableA - 1;
            }
            if (temp == 28 &&
                remain19 > 10)
            {
                tableA = tableA - 1;
            }
            //find the next Sunday
            tableB = (tableA - 19) % 7;

            tableC = (40 - firstDigit) % 4;
            if (tableC == 3)
            {
                tableC++;
            }
            if (tableC > 1)
            {
                tableC++;
            }

            temp = year % 100;
            tableD = (temp + temp / 4) % 7;

            tableE = ((20 - tableB - tableC - tableD) % 7) + 1;
            d = tableA + tableE;

            if (d > 31)
            {
                d = d - 31;
                m = 4;
            }
            else
            {
                m = 3;
            }
            return new DateTime(year, m, d);
        }

        public static DateTime GetFirstNonHolidayWeekday(DateTime date, string weekday)
        {
            var newDate = date;
            newDate = newDate.AddDays(1);
            while ((newDate.DayOfWeek.ToString() != weekday) || (IsHoliday(newDate)))
            {
                newDate = newDate.AddDays(1);
            }
            return newDate;
        }

        public static DateTime GetShortestSpanToNonHolidayWeekday(DateTime date, string weekday)
        {
            DateTime returnValue;
            var originalDate = date;

            // Setup Past Date
            var pastDate = date;
            pastDate = pastDate.AddDays(-1);
            while ((pastDate.DayOfWeek.ToString() != weekday) || (IsHoliday(pastDate)))
            {
                pastDate = pastDate.AddDays(-1);
            }

            // Setup Future Date
            var futureDate = date;
            futureDate = futureDate.AddDays(1);
            while ((futureDate.DayOfWeek.ToString() != weekday) || (IsHoliday(futureDate)))
            {
                futureDate = futureDate.AddDays(1);
            }

            // Check to see which span is shortest and return that date
            var pastSpan = originalDate - pastDate;
            var futureSpan = futureDate - originalDate;

            if (pastSpan < futureSpan)
            {
                returnValue = pastDate;
            }
            else
            {
                returnValue = futureDate;
            }

            return returnValue;
        }

        public static DateTime GetShortestSpanToWeekday(DateTime date, string weekday)
        {
            DateTime returnValue;
            var originalDate = date;

            // Setup Past Date
            var pastDate = date;
            pastDate = pastDate.AddDays(-1);
            while (pastDate.DayOfWeek.ToString() != weekday)
            {
                pastDate = pastDate.AddDays(-1);
            }

            // Setup Future Date
            var futureDate = date;
            futureDate = futureDate.AddDays(1);
            while (futureDate.DayOfWeek.ToString() != weekday)
            {
                futureDate = futureDate.AddDays(1);
            }

            // Check to see which span is shortest and return that date
            var pastSpan = originalDate - pastDate;
            var futureSpan = futureDate - originalDate;

            if (pastSpan < futureSpan)
            {
                returnValue = pastDate;
            }
            else
            {
                returnValue = futureDate;
            }

            return returnValue;
        }

        public static bool IsHoliday(this DateTime currentDate)
        {
            int year = currentDate.Year;
            var holidays = new List<DateTime>();
            //New Years
            holidays.Add(new DateTime(year, 1, 1));
            //Martin Luther King Day
            holidays.Add(MlkDay(year));
            holidays.Add(PresidentsDay(year));
            //holidays.Add(Easter(year));
            holidays.Add(MemorialDay(year));
            //Independence Day
            holidays.Add(new DateTime(year, 7, 4));
            holidays.Add(LaborDay(year));
            holidays.Add(Thanksgiving(year));
            holidays.Add(Thanksgiving2(year));
            //X-mas
            holidays.Add(new DateTime(year, 12, 25));

            foreach (var d in (from h in holidays where h.Month == currentDate.Month && h.Day == currentDate.Day select h)) return true;
            return false;
        }

        public static DateTime LaborDay(int year)
        {
            var laborDay = DateTime.MinValue;
            var sept = new List<DateTime>();
            for (int count = 1; count < 31; count++)
            {
                sept.Add(new DateTime(year, 9, count));
            }

            var monday = DayOfWeek.Monday;
            //use LINQ query to find all Mondays and return in ascending order to get the first Monday of the month
            //use First extension to get one Monday, the first one

            laborDay = (from d in sept where d.DayOfWeek == monday orderby d.Day ascending select d).First();
            return laborDay;
        }

        public static DateTime MemorialDay(int year)
        {
            var memorialDay = DateTime.MinValue;
            var may = new List<DateTime>();
            for (int count = 1; count < 32; count++)
            {
                may.Add(new DateTime(year, 5, count));
            }

            var monday = DayOfWeek.Monday;
            //use LINQ query to find all Mondays and return in descending order to get the last Monday of the month
            //use First extension to get 1 Monday; the last one because of descending sort
            memorialDay = (from d in may where d.DayOfWeek == monday orderby d.Day descending select d).First();
            return memorialDay;
        }

        public static DateTime MlkDay(int year)
        {
            var mlkDay = DateTime.MinValue;
            var jan = new List<DateTime>();
            for (int count = 1; count < 32; count++)
            {
                jan.Add(new DateTime(year, 1, count));
            }

            var monday = DayOfWeek.Monday;
            //Martin Luther King Day is the 3rd Monday of the month
            //use LINQ query to find all Mondays and return in ascending order
            //Use a combination of Take and Last to get the 3rd Monday
            mlkDay = (from d in jan where d.DayOfWeek == monday orderby d.Day ascending select d).Take(3).Last();
            return mlkDay;
        }

        public static DateTime PresidentsDay(int year)
        {
            var leapYear = new DateTime(year, 2, 1);

            var presidentsDay = DateTime.MinValue;
            var feb = new List<DateTime>();
            for (int count = 1; count < 28; count++)
            {
                feb.Add(new DateTime(year, 2, count));
            }

            var monday = DayOfWeek.Monday;
            //use LINQ query to find all Mondays and return in ascending order
            //Presidents Day is the 3rd Monday of the month
            //Use a combination of Take and Last to get the 3rd Monday
            presidentsDay = (from d in feb where d.DayOfWeek == monday orderby d.Day ascending select d).Take(3).Last();
            return presidentsDay;
        }

        public static DateTime Thanksgiving(int year)
        {
            var thanksgiving = DateTime.MinValue;
            var november = new List<DateTime>();
            for (int count = 1; count < 31; count++)
            {
                november.Add(new DateTime(year, 11, count));
            }
            var thursdays = DayOfWeek.Thursday;

            //Use LINQ query to find all the thursdays of the month of November.
            //Thanksgiving is on the 4th thursday
            //Use the combination of Take and Last to get the 4th thursday of the month
            thanksgiving = (from d in november where d.DayOfWeek == thursdays orderby d.Day ascending select d).Take(4).Last();
            return thanksgiving;
        }

        public static DateTime Thanksgiving2(int year)
        {
            var thanksgiving = DateTime.MinValue;
            var november = new List<DateTime>();
            for (int count = 1; count < 31; count++)
            {
                november.Add(new DateTime(year, 11, count));
            }
            var thursdays = DayOfWeek.Thursday;

            //Use LINQ query to find all the thursdays of the month of November.
            //Thanksgiving is on the 4th thursday
            //Use the combination of Take and Last to get the 4th thursday of the month
            thanksgiving = (from d in november where d.DayOfWeek == thursdays orderby d.Day ascending select d).Take(4).Last().AddDays(1);
            return thanksgiving;
        }
    }
}
