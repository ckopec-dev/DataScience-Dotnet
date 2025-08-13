using Experimental.MathParser;

namespace UnitTests
{
    [TestClass]
    public class MathParserUnitTests
    {
        [TestMethod]
        public void TokenizerTest()
        {
            var testString = "10 + 20 - 30.123";
            var t = new Tokenizer(new StringReader(testString));

            // "10"
            Assert.AreEqual(Token.Number, t.Token);
            Assert.AreEqual(10, t.Number);
            t.NextToken();

            // "+"
            Assert.AreEqual(Token.Add, t.Token);
            t.NextToken();

            // "20"
            Assert.AreEqual(Token.Number, t.Token);
            Assert.AreEqual(20, t.Number);
            t.NextToken();

            // "-"
            Assert.AreEqual(Token.Subtract, t.Token);
            t.NextToken();

            // "30.123"
            Assert.AreEqual(Token.Number, t.Token);
            Assert.AreEqual(30.123, t.Number);
            t.NextToken();
        }
    }
}