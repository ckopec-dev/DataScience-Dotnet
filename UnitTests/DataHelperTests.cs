using Core;

namespace UnitTests
{
    [TestClass]
    public class DataHelperTests
    {
        [TestMethod]
        public void USStockMarketHolidaysTest()
        {
            List<DateOnly> holidays = DataHelper.USStockMarketHolidays;

            Assert.AreNotEqual(0, holidays.Count);
        }

        [TestMethod]
        public void RnaCodonTableTest()
        {
            Dictionary<string, string> dic = DataHelper.RnaCodonTable;

            Assert.AreNotEqual(0, dic.Count);
        }
    }
}
