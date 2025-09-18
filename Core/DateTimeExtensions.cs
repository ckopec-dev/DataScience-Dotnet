using System.Reflection;

namespace Core
{
    public static class DateTimeExtensions
    {
        public static bool IsUSStockMarketOpen(this DateTime date)
        {
            // The U.S.stock market is closed on: New Year's Day, Martin Luther King, Jr. Day, Presidents Day (Washington's Birthday), Good Friday, Memorial Day, Independence Day, Labor Day, Thanksgiving Day, and Christmas Day.
            // If a holiday falls on a weekend, the stock market may close on the previous Friday, as is often the case for Good Friday and Easter.
            // Create boolean IsUSStockMarketHoliday datetime extension.Also include Saturdays and Sundays.

            if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
                return false;

            // If it isn't in this list, the assumption is a true result. As of creation, the list only goes back to 2012.
            Stream? mrs = Assembly.GetExecutingAssembly().GetManifestResourceStream("Core.Data.USStockMarketHolidays.txt") ?? throw new Exception("Resource not found: USStockMarketHolidays.txt");
            using StreamReader sr = new(mrs);

            while(!sr.EndOfStream)
            {
                string? holiday = sr.ReadLine();
                if (holiday != null)
                {
                    DateTime h = Convert.ToDateTime(holiday);

                    if (date == h)
                        return false;
                }
            }

            return true;
        }

        public static bool IsLeapYear(this DateTime date)
        {
            int year = date.Year;

            bool result;

            if (year % 4 == 0)
            {
                result = true;

                if (year % 100 == 0)
                {
                    result = false;

                    if (year % 400 == 0)
                    {
                        result = true;
                    }
                }
            }
            else
            {
                result = false;
            }

            return result;
        }

        public static bool IsGregorianDate(this DateTime date)
        {
            if (date < new DateTime(1582, 10, 15))
                return true;
            else
                return false;
        }

        public static decimal DayNumber(this DateTime dateTime)
        {
            if (dateTime.Month <= 2)
            {
                int num = dateTime.Month - 1;

                if (dateTime.IsLeapYear())
                    num *= 62;
                else
                    num *= 63;
                return Math.DivRem(num, 2, out _) + dateTime.Day;
            }
            else
            {
                int num = dateTime.Month + 1;

                int result = (int)Math.Floor(num * 30.6m);

                if (dateTime.IsLeapYear())
                    result -= 62;
                else
                    result -= 63;

                return result + dateTime.Day;
            }
        }

        public static decimal DayPercentage(this DateTime dateTime, double millisecondsPerDay)
        {
            // Given the time of day, convert that to a percentage of the total time in a day. The specific date is irrelevant.
            // E.g. 6 am = .25, 12 pm = .5, 6 pm = .75, 12 am = 0

            DateTime dt = dateTime.AddDays(1d);

            DateTime endOfDay = new(dt.Year, dt.Month, dt.Day);

            // The time left in the day.
            TimeSpan ts = endOfDay - dateTime;

            // Total milliseconds in day: 8.64e+7
            double diff = (millisecondsPerDay - ts.TotalMilliseconds) / millisecondsPerDay;

            return (decimal)diff;
        }

        public static DateTime ToDateTime(this long UnixEpoch)
        {
            return UnixEpoch.ToDateTime(-5); // default is EST.
        }

        public static DateTime ToDateTime(this long UnixEpoch, int TimeZoneOffset)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            DateTime dt = epoch.AddSeconds(UnixEpoch);
            return dt.AddHours(TimeZoneOffset);
        }
    }
}
