using System.Text;

namespace Core.GameTheory
{
    /// <summary>
    /// In the card game poker, a hand consists of five cards.
    /// </summary>
    public class PokerHand
    {
        #region Fields

        private bool _IsSorted;

        #endregion

        #region Properties

        public List<Card> Cards { get; set; }

        public int TotalValue
        {
            get
            {
                if (Cards == null)
                    return 0;
                else
                {
                    int ttl = 0;

                    foreach (Card c in Cards)
                        ttl += c.Value;

                    return ttl;
                }
            }
        }

        #endregion

        #region Ctors/Dtors

        public PokerHand()
        {
            Cards = [];
        }

        public PokerHand(string[] cards)
        {
            Cards = [];

            // Input is array of short name of cards.
            if (cards.Length != 5)
                throw new Exception("Invalid number of cards: " + cards.Length);

            for (int i = 0; i < cards.Length; i++)
            {
                Card c = new(cards[i][..1], cards[i].Substring(1, 1));

                Cards.Add(c);
            }
        }

        #endregion

        #region Public methods

        public override string ToString()
        {
            StringBuilder sb = new();

            foreach (Card card in Cards)
            {
                sb.AppendLine(card.ToString());
            }

            return sb.ToString();
        }

        public static string? CardsToString(List<Card> cards)
        {
            if (cards == null)
                return null;

            StringBuilder sb = new();

            foreach (Card c in cards)
            {
                sb.AppendLine(c.ToString());
            }

            return sb.ToString();
        }

        public static PokerHand? FindWinner(PokerHand hand1, PokerHand hand2)
        {
            // Return the winning hand, or null if it's a tie.

            hand1.Sort();
            hand2.Sort();

            PokerRank rank1 = hand1.GetRank();
            PokerRank rank2 = hand2.GetRank();

            if (rank1 > rank2)
            {
                return hand1;
            }
            else if (rank2 > rank1)
            {
                return hand2;
            }
            else
            {
                // Same rank, so compare values of rank.
                switch (rank1)
                {
                    case PokerRank.RoyalFlush:
                        return null;
                    case PokerRank.StraightFlush:
                        if (hand1.HighCard().Value > hand2.HighCard().Value)
                            return hand1;
                        else if (hand2.HighCard().Value > hand1.HighCard().Value)
                            return hand2;
                        else
                            return null;
                    case PokerRank.FourOfAKind:
                    case PokerRank.FullHouse:
                        if (hand1.Cards[4].Value > hand2.Cards[4].Value)
                            return hand1;
                        else if (hand2.Cards[4].Value > hand1.Cards[4].Value)
                            return hand2;
                        else
                        {
                            if (hand1.Cards[0].Value > hand2.Cards[0].Value)
                                return hand1;
                            else if (hand2.Cards[0].Value > hand1.Cards[0].Value)
                                return hand2;
                            else
                                return null;
                        }
                    case PokerRank.Flush:
                    case PokerRank.Straight:
                    case PokerRank.HighCard:
                    case PokerRank.ThreeOfAKind:
                    case PokerRank.TwoPair:
                    case PokerRank.OnePair:
                        for (int i = 4; i >= 0; i--)
                        {
                            if (hand1.Cards[i].Value > hand2.Cards[i].Value)
                                return hand1;
                            else if (hand2.Cards[i].Value > hand1.Cards[i].Value)
                                return hand2;
                        }
                        return null;
                    default:
                        throw new NotImplementedException();
                }
            }
        }

        public static List<Card> NonWinners(PokerHand completeHand, List<Card> winners)
        {
            List<Card> nonWinners = [];

            foreach (Card c in completeHand.Cards)
            {
                if (!winners.Contains(c))
                {
                    nonWinners.Add(c);
                }
            }

            if (winners.Count + nonWinners.Count != completeHand.Cards.Count)
            {
                throw new Exception(String.Format("Failure evaluating NonWinners. Winners: {0}, Non-winners: {1}", winners.Count, nonWinners.Count));
            }

            return nonWinners;
        }

        public static PokerHand Deal(Deck deck)
        {
            PokerHand p = new();

            for (int i = 0; i < 5; i++)
            {
                p.Cards.Add(deck.Cards.Dequeue());
            }

            return p;
        }

        public bool Equals(PokerHand hand)
        {
            foreach (Card c in Cards)
            {
                if (!hand.Cards.Contains(c))
                {
                    return false;
                }
            }

            return true;
        }

        public void Sort()
        {
            if (!_IsSorted)
            {
                // Cards are ordered by winner values asc, then non-winners asc.
                // For aggregate winners (e.g. full house, the higher rank component gets sorted highest.

                PokerRank r = GetRank();
                
                switch (r)
                {
                    case PokerRank.RoyalFlush:
                    case PokerRank.StraightFlush:
                    case PokerRank.Flush:
                    case PokerRank.Straight:
                    case PokerRank.HighCard:
                        Cards = [.. Cards.OrderBy(i => i.Value)];
                        break;
                    case PokerRank.FourOfAKind:
                    case PokerRank.ThreeOfAKind:
                    case PokerRank.TwoPair:
                    case PokerRank.OnePair:
                        // The non-winner appears first, followed by the winners

                        List<Card> winners = [];
                        List<Card>? hand = FourOfAKind();
                        if (r == PokerRank.FourOfAKind && hand != null)
                            winners = [.. hand.OrderBy(static i => i.Value)];
                        hand = ThreeOfAKind();
                        if (r == PokerRank.ThreeOfAKind && hand != null)
                            winners = [.. hand.OrderBy(static i => i.Value)];
                        hand = TwoPair();
                        if (r == PokerRank.TwoPair && hand != null)
                            winners = [.. hand.OrderBy(static i => i.Value)];
                        hand = OnePair();
                        if (r == PokerRank.OnePair && hand != null)
                            winners = [.. hand.OrderBy(static i => i.Value)];

                        List<Card> nonwinners = [];
                        foreach (Card c in Cards)
                        {
                            if (!winners.Contains(c))
                                nonwinners.Add(c);
                        }
                        nonwinners = [.. nonwinners.OrderBy(i => i.Value)];
                        Cards.Clear();
                        Cards.AddRange(nonwinners);
                        Cards.AddRange(winners);
                        break;
                    case PokerRank.FullHouse:
                        // The pair appears first, followed by the three of a kind.
                        List<Card> pair = [];
                        List<Card> toak = [];

                        Cards = [.. Cards.OrderBy(i => i.Value)];

                        if (Cards[0].Value == Cards[2].Value)
                        {
                            // The three of a kind appears first.
                            toak.Add(Cards[0]);
                            toak.Add(Cards[1]);
                            toak.Add(Cards[2]);
                            pair.Add(Cards[3]);
                            pair.Add(Cards[4]);
                        }
                        else
                        {
                            // The pair is first
                            pair.Add(Cards[0]);
                            pair.Add(Cards[1]);
                            toak.Add(Cards[2]);
                            toak.Add(Cards[3]);
                            toak.Add(Cards[4]);
                        }

                        List<Card> value = [.. pair.OrderBy(i => i.Value)];
                        pair = value;
                        toak = [.. toak.OrderBy(i => i.Value)];

                        Cards.Clear();
                        Cards.AddRange(pair);
                        Cards.AddRange(toak);
                        break;
                }

                _IsSorted = true;
            }
        }

        public PokerRank GetRank()
        {
            if (RoyalFlush() != null) return PokerRank.RoyalFlush;
            if (StraightFlush() != null) return PokerRank.StraightFlush;
            if (FourOfAKind() != null) return PokerRank.FourOfAKind;
            if (FullHouse() != null) return PokerRank.FullHouse;
            if (Flush() != null) return PokerRank.Flush;
            if (Straight() != null) return PokerRank.Straight;
            if (ThreeOfAKind() != null) return PokerRank.ThreeOfAKind;
            if (TwoPair() != null) return PokerRank.TwoPair;
            if (OnePair() != null) return PokerRank.OnePair;

            return PokerRank.HighCard;
        }

        public List<Card>? OnePair()
        {
            foreach (Card c in Cards)
            {
                List<Card> pairs = Cards.Where(i => i.Value == c.Value).ToList();

                if (pairs.Count > 1)
                {
                    return pairs;
                }
            }

            return null;
        }

        public List<Card>? TwoPair()
        {
            List<Card>? onePair = OnePair();

            if (onePair == null)
            {
                // The hand doesn't contain a single pair, never mind two.
                return null;
            }

            // Now do a regular pair search excluding the cards from the first pair.
            foreach (Card c in Cards)
            {
                if (onePair.Contains(c))
                    continue;

                List<Card> pairs = Cards.Where(i => i.Value == c.Value).ToList();

                if (pairs.Count > 1)
                {
                    // For the result, add this pair to the onePair, so two pairs are returned.
                    onePair.AddRange(pairs);

                    return onePair;
                }
            }

            return null;
        }

        public List<Card>? ThreeOfAKind()
        {
            foreach (Card c in Cards)
            {
                List<Card> pairs = Cards.Where(i => i.Value == c.Value).ToList();

                if (pairs.Count > 2)
                {
                    return pairs;
                }
            }

            return null;
        }

        public List<Card>? Straight()
        {
            for (int i = 1; i < 5; i++)
            {
                if (Cards[i].Value != Cards[i - 1].Value + 1)
                    return null;
            }

            return Cards;
        }

        public List<Card>? Flush()
        {
            for (int i = 1; i < 5; i++)
            {
                if (Cards[i].Suit != Cards[0].Suit)
                    return null;
            }

            return Cards;
        }

        public List<Card>? FullHouse()
        {
            // Three of a kind and a pair.
            // Evaluation logic is similar to two pair

            List<Card>? threeOfAKind = ThreeOfAKind();

            if (threeOfAKind == null)
            {
                // The hand doesn't contain 3 of a kind.
                return null;
            }

            // Now do a pair search excluding the cards from the result already found.
            foreach (Card c in Cards)
            {
                if (threeOfAKind.Contains(c))
                    continue;

                List<Card> pairs = Cards.Where(i => i.Value == c.Value).ToList();

                if (pairs.Count > 1)
                {
                    // For the result, add to the original pair, so the three of a kind appears before the pair in the list (for later comparison purposes).
                    threeOfAKind.AddRange(pairs);

                    return threeOfAKind;
                }
            }

            return null;
        }

        public List<Card>? FourOfAKind()
        {
            foreach (Card c in Cards)
            {
                List<Card> pairs = Cards.Where(i => i.Value == c.Value).ToList();

                if (pairs.Count > 3)
                {
                    return pairs;
                }
            }

            return null;
        }

        public List<Card>? StraightFlush()
        {
            if (Straight() != null && Flush() != null)
                return Cards;
            else
                return null;
        }

        public List<Card>? RoyalFlush()
        {
            if (StraightFlush() != null && Cards[0].Value == 10)
                return Cards;
            else
                return null;
        }

        public Card HighCard()
        {
            Sort();

            return Cards[^1];
        }

        #endregion
    }
}
