using System.Reflection;

namespace Core
{
    class DataHelper
    {
        public static List<DateOnly> USStockMarketHolidays
        {
            get
            {
                Stream? mrs = Assembly.GetExecutingAssembly().GetManifestResourceStream("Core.Data.USStockMarketHolidays.txt") ?? throw new Exception("Resource not found: hamm.txt");
                using StreamReader sr = new(mrs);

                List<DateOnly> dates = [];
                string? line;

                while ((line = sr.ReadLine()) != null)
                {
                    dates.Add(DateOnly.Parse(line.Trim()));
                }

                return dates;
            }
        }
}
}
