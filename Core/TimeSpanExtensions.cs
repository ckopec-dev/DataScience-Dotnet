
namespace Core
{
    public static class TimeSpanExtensions
    {
        public static string ToFriendlyString(this TimeSpan ts, int precision)
        {
            string fmt = "#." + new string('0', precision);

            if (ts.TotalMilliseconds < 1000)
                return ts.TotalMilliseconds.ToString(fmt) + " millisecond(s)";
            else if (ts.TotalSeconds < 60)
                return ts.TotalSeconds.ToString(fmt) + " second(s)";
            else if (ts.TotalMinutes < 60)
                return ts.TotalMinutes.ToString(fmt) + " minute(s)";
            else
                return ts.TotalHours.ToString(fmt) + " hour(s)";
        }
    }
}
