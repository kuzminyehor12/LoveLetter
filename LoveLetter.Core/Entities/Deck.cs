using Newtonsoft.Json.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace LoveLetter.Core.Entities
{
    public class Deck : DomainEntity
    {
        [JsonPropertyName("Cards")]
        public Queue<Card> Cards { get; private set; } = new Queue<Card>();

        [JsonPropertyName("Count")]
        public int CardsCount { get; private set;  }

        public static Deck Populate(string json)
        {
            var jsonObject = JObject.Parse(json);
            return new Deck
            {
                Cards = jsonObject["Cards"]?.ToObject<Queue<Card>>() ?? new Queue<Card>(),
                CardsCount = jsonObject["Count"]?.Value<int>() ?? 0
            };
        }

        public Card Dequeue()
        {
            return Cards.Dequeue();
        }
    }
}
