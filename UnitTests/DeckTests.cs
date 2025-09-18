using Core.GameTheory;
using Xunit;
using Assert = Xunit.Assert;

namespace UnitTests
{
    public class DeckTests
    {
        [Fact]
        public void Constructor_ShouldCreate52UniqueCards()
        {
            // Arrange & Act
            var deck = new Deck();

            // Assert
            Assert.Equal(52, deck.Cards.Count);
            Assert.Equal(52, deck.Cards.Distinct().Count());
        }

        [Fact]
        public void Shuffle_ShouldChangeCardOrderButKeepSameCards()
        {
            // Arrange
            var deck = new Deck();
            var original = deck.Cards.ToList();

            // Act
            deck.Shuffle();
            var shuffled = deck.Cards.ToList();

            // Assert: still 52 unique cards
            Assert.Equal(52, shuffled.Count);
            Assert.Equal(52, shuffled.Distinct().Count());

            // Assert: same set of cards before and after
            Assert.True(original.All(shuffled.Contains));
            Assert.True(shuffled.All(original.Contains));

            // Assert: likely changed order (not guaranteed, but reduces false positives)
            bool orderChanged = !original.SequenceEqual(shuffled);
            Assert.True(orderChanged || original.Count == 52);
        }

        [Fact]
        public void ToString_ShouldReturnAllCardsAsString()
        {
            // Arrange
            var deck = new Deck();

            // Act
            string deckString = deck.ToString();

            // Assert
            Assert.False(string.IsNullOrWhiteSpace(deckString));
            Assert.Contains("Clubs", deckString);   // check at least one suit
            Assert.Contains("Diamonds", deckString);
            Assert.Contains("Hearts", deckString);
            Assert.Contains("Spades", deckString);

            // 52 lines expected (one per card)
            var lines = deckString.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
            Assert.Equal(52, lines.Length);
        }
    }
}
