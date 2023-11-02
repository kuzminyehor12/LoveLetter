using LoveLetter.Core.Utils;

namespace LoveLetter.Core.Entities
{
    public class Deck : DomainEntity
    {
        public Queue<Card> Cards { get; private set; } = new Queue<Card>();

        public int CardsCount => Cards.Count;

        public Deck()
        {

        }

        public Deck(IEnumerable<short> cardTypes)
        {
            Cards = new Queue<Card>(cardTypes.Select(type => new Card((CardType)type)));
        }

        public void Enqueue(Card card)
        {
            Cards.Enqueue(card);
        }

        public Card Dequeue()
        {
            return Cards.Dequeue();
        }
    }
}
