using LoveLetter.Core.Utils;

namespace LoveLetter.Core.Entities
{
    public class Card : DomainEntity
    {
        public CardType CardType { get; set; }

        public Card() { }

        public Card(CardType cardType)
        {
            CardType = cardType;
        }

        public void Effect(CardEvents eventContext, CardEventArgs args)
        {
            switch (CardType)
            {
                case CardType.Guard:
                    eventContext.OnGuardPicked(args); 
                    break;
                case CardType.Priest:
                    eventContext.OnPriestPicked(args);
                    break;
                case CardType.Baron:
                    eventContext.OnBaronPicked(args);
                    break;
                case CardType.Handmaid:
                    eventContext.OnHandmaidPicked(args);
                    break;
                case CardType.Prince:
                    eventContext.OnPrincePicked(args);
                    break;
                case CardType.King:
                    eventContext.OnKingPicked(args);
                    break;
                case CardType.Countess:
                    eventContext.OnCountessPicked(args);
                    break;
                case CardType.Princess:
                    eventContext.OnPrincessPicked(args);
                    break;
            }
        }

        public static implicit operator CardType(Card card) => card.CardType;

        public static bool operator <(Card card1, Card card2) => card1.CardType < card2.CardType;

        public static bool operator >(Card card1, Card card2) => card1.CardType > card2.CardType;

        public static bool operator ==(Card card1, Card card2) => card1.CardType == card2.CardType;

        public static bool operator !=(Card card1, Card card2) => card1.CardType != card2.CardType;

        public static bool operator <=(Card card1, Card card2) => card1.CardType <= card2.CardType;

        public static bool operator >=(Card card1, Card card2) => card1.CardType <= card2.CardType;

        public override bool Equals(object? obj)
        {
            if (obj is null)
            {
                return false;
            }

            return this == (Card)obj;
        }

        public override int GetHashCode() => CardType.GetHashCode();
    }
}
