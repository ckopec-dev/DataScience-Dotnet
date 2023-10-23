
namespace Core
{
    public static class DateTimeExtensions
    {
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
    }
}
