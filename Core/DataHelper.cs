using SkiaSharp;
using System.Reflection;

namespace Core
{
    public class DataHelper
    {
        public static List<DateOnly> USStockMarketHolidays
        {
            get
            {
                Stream? mrs = Assembly.GetExecutingAssembly().GetManifestResourceStream("Core.Data.USStockMarketHolidays.txt") ?? throw new ResourceNotFoundException();
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

        public static Dictionary<string, string> RnaCodonTable
        {
            get
            {
                Stream? mrs = Assembly.GetExecutingAssembly().GetManifestResourceStream("Core.Data.RnaCodonTable.txt") ?? throw new ResourceNotFoundException();
                using StreamReader sr = new(mrs);

                Dictionary<string, string> dic = [];

                while(!sr.EndOfStream)
                {
                    string? line = sr.ReadLine();

                    if (line != null)
                    {
                        string[] split = line.Split(" ");
                        dic.Add(split[0], split[1]);
                    }
                }

                return dic;
            }
        }
    }
}
