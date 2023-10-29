using LoveLetter.Core.Entities;
using System.Xml;
using System.Xml.Serialization;

namespace LoveLetter.Core.Utils
{
    public static class ParseUtils
    {
        public static List<(string ColumnName, string ColumnValue)> ParseValues(params (string ColumnName, object ColumnValue)[] columns)
        {
            var result = new List<(string, string)>();

            foreach (var column in columns)
            {
                if (column.ColumnValue is List<Player> players)
                {
                    result.Add((column.ColumnName, PlayersToXml(players)));
                }
                else if (column.ColumnValue is Deck deck)
                {
                    result.Add((column.ColumnName, ParseDeck(deck)));
                }
                else
                {
                    result.Add((column.ColumnName, column.ColumnValue?.ToString() ?? string.Empty));
                }
            }

            return result;
        }
        public static string PlayersToXml(List<Player> players)
        {
            var serializer = new XmlSerializer(typeof(List<Player>));
            var xml = string.Empty;

            using (var sw = new StringWriter())
            {
                using (XmlWriter writer = XmlWriter.Create(sw))
                {
                    serializer.Serialize(writer, players);
                    xml = sw.ToString();
                }
            }

            return xml;
        }

        public static string ParseDeck(Deck deck) =>
            string.Join(',', deck.Cards.Select(c => c.CardType));
    }
}
