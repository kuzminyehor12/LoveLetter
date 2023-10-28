using Newtonsoft.Json.Linq;
using System.Text.Json.Serialization;

namespace LoveLetter.Core.Entities
{
    public class Deck : DomainEntity
    {
        [JsonPropertyName("Cards")]
        public Queue<Card> Cards { get; private set; } = new Queue<Card>();
        public int CardsCount => Cards.Count;

        public static Deck Populate(string json)
        {
            var jsonObject = JObject.Parse(json);
            return new Deck
            {
                Cards = jsonObject["Cards"]?.ToObject<Queue<Card>>() ?? new Queue<Card>()
            };
        }

        public Card Dequeue()
        {
            return Cards.Dequeue();
        }
    }
}
