using System.Text;

namespace Core.GameTheory
{
    public class Deck
    {
        public Queue<Card> Cards { get; set; }

        public Deck()
        {
            // A deck consists of one card of every suit and value.

            Cards = new Queue<Card>();

            for (int i = 2; i <= 14; i++)
            {
                Cards.Enqueue(new Card(i, Suit.Clubs));
                Cards.Enqueue(new Card(i, Suit.Diamonds));
                Cards.Enqueue(new Card(i, Suit.Hearts));
                Cards.Enqueue(new Card(i, Suit.Spades));
            }
        }

        public void Shuffle()
        {
            List<Card> cards = [.. Cards];
            cards.RandomSort();

            Cards = new Queue<Card>();
            foreach (Card c in cards)
            {
                Cards.Enqueue(c);
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new();

            foreach (Card card in Cards)
            {
                sb.AppendLine(card.ToString());
            }

            return sb.ToString();
        }
    }
}