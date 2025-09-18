using Core.GameTheory;
using Xunit;
using Assert = Xunit.Assert;

namespace UnitTests
{
    public class CardTests
    {
        [Theory]
        [InlineData(2, "Two", "2")]
        [InlineData(3, "Three", "3")]
        [InlineData(4, "Four", "4")]
        [InlineData(5, "Five", "5")]
        [InlineData(6, "Six", "6")]
        [InlineData(7, "Seven", "7")]
        [InlineData(8, "Eight", "8")]
        [InlineData(9, "Nine", "9")]
        [InlineData(10, "Ten", "T")]
        [InlineData(11, "Jack", "J")]
        [InlineData(12, "Queen", "Q")]
        [InlineData(13, "King", "K")]
        [InlineData(14, "Ace", "A")]
        public void ValueName_And_ValueNameShort_AreCorrect(int value, string expectedName, string expectedShort)
        {
            var card = new Card(value, Suit.Spades);
            Assert.Equal(expectedName, card.ValueName);
            Assert.Equal(expectedShort, card.ValueNameShort);
        }

        [Fact]
        public void ValueName_InvalidValue_ThrowsException()
        {
            var card = new Card(99, Suit.Hearts);
            Assert.Throws<Exception>(() => { var _ = card.ValueName; });
        }

        [Fact]
        public void ValueNameShort_InvalidValue_ThrowsException()
        {
            var card = new Card(0, Suit.Diamonds);
            Assert.Throws<Exception>(() => { var _ = card.ValueNameShort; });
        }

        [Theory]
        [InlineData("2", "c", 2, Suit.Clubs)]
        [InlineData("3", "c", 3, Suit.Clubs)]
        [InlineData("4", "c", 4, Suit.Clubs)]
        [InlineData("5", "c", 5, Suit.Clubs)]
        [InlineData("6", "c", 6, Suit.Clubs)]
        [InlineData("7", "c", 7, Suit.Clubs)]
        [InlineData("8", "c", 8, Suit.Clubs)]
        [InlineData("9", "c", 9, Suit.Clubs)]
        [InlineData("t", "d", 10, Suit.Diamonds)]
        [InlineData("J", "h", 11, Suit.Hearts)]
        [InlineData("q", "c", 12, Suit.Clubs)]
        [InlineData("k", "c", 13, Suit.Clubs)]
        [InlineData("a", "s", 14, Suit.Spades)]
        public void Constructor_ShortValues_AssignsCorrectly(string shortValue, string shortSuit, int expectedValue, Suit expectedSuit)
        {
            var card = new Card(shortValue, shortSuit);
            Assert.Equal(expectedValue, card.Value);
            Assert.Equal(expectedSuit, card.Suit);
        }

        [Fact]
        public void Constructor_ShortValue_Invalid_ThrowsException()
        {
            Assert.Throws<Exception>(() => new Card("z", "h"));
        }

        [Fact]
        public void Constructor_ShortSuit_Invalid_ThrowsException()
        {
            Assert.Throws<Exception>(() => new Card("a", "x"));
        }

        [Fact]
        public void ToString_ReturnsExpected()
        {
            var card = new Card(14, Suit.Spades);
            Assert.Equal("Ace of Spades", card.ToString());
        }

        [Fact]
        public void ToShortString_ReturnsExpected()
        {
            var card = new Card(13, Suit.Hearts);
            Assert.Equal("KH", card.ToShortString());
        }

        [Fact]
        public void DefaultConstructor_AllowsManualPropertySet()
        {
            var card = new Card { Value = 12, Suit = Suit.Diamonds };
            Assert.Equal("Queen", card.ValueName);
            Assert.Equal("Q", card.ValueNameShort);
            Assert.Equal("Queen of Diamonds", card.ToString());
            Assert.Equal("QD", card.ToShortString());
        }
    }
}
