
namespace Core.GameTheory
{
    public class Card
    {
        public Suit Suit { get; set; }
        public int Value { get; set; }

        public string ValueName
        {
            get
            {
                return Value switch
                {
                    2 => "Two",
                    3 => "Three",
                    4 => "Four",
                    5 => "Five",
                    6 => "Six",
                    7 => "Seven",
                    8 => "Eight",
                    9 => "Nine",
                    10 => "Ten",
                    11 => "Jack",
                    12 => "Queen",
                    13 => "King",
                    14 => "Ace",
                    _ => throw new Exception("Invalid Value: " + Value),
                };
            }
        }

        public string ValueNameShort
        {
            get
            {
                return Value switch
                {
                    2 => "2",
                    3 => "3",
                    4 => "4",
                    5 => "5",
                    6 => "6",
                    7 => "7",
                    8 => "8",
                    9 => "9",
                    10 => "T",
                    11 => "J",
                    12 => "Q",
                    13 => "K",
                    14 => "A",
                    _ => throw new Exception("Invalid Value: " + Value),
                };
            }
        }

        public Card()
        {
        }

        public Card(int value, Suit suit)
        {
            Value = value;
            Suit = suit;
        }

        public Card(string shortValue, string shortSuit)
        {
            Value = shortValue.ToLower() switch
            {
                "2" => 2,
                "3" => 3,
                "4" => 4,
                "5" => 5,
                "6" => 6,
                "7" => 7,
                "8" => 8,
                "9" => 9,
                "t" => 10,
                "j" => 11,
                "q" => 12,
                "k" => 13,
                "a" => 14,
                _ => throw new Exception("Invalid value: " + shortValue),
            };
            Suit = shortSuit.ToLower() switch
            {
                "c" => Suit.Clubs,
                "d" => Suit.Diamonds,
                "h" => Suit.Hearts,
                "s" => Suit.Spades,
                _ => throw new Exception("Invalid suit: " + shortSuit),
            };
        }

        public override string ToString()
        {
            return String.Format("{0} of {1}", ValueName, Suit);
        }

        public string ToShortString()
        {
            return String.Format("{0}{1}", ValueNameShort, Suit.ToString()[..1]);
        }
    }
}
