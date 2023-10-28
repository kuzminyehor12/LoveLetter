using LoveLetter.Core.Adapters;
using LoveLetter.Core.Queries;
using Microsoft.Data.SqlClient;
using System.Data;
using System.IO;
using System.Text.Json;
using System.Xml.Serialization;

namespace LoveLetter.Core.Entities
{
    public class GameState : DomainEntity
    {
        private ISqlDataAdapter _adapter = new GameStateSqlAdapter();

        public Guid Id { get; private set; }

        [XmlElement("Players")]
        public List<Player> Players { get; private set; } = new List<Player>();

        public Deck Deck { get; private set; } = new Deck();

        public short TurnPlayerNumber { get; private set; }

        public short? WinnerPlayerNumber { get; private set; }

        public DateTime StartDate { get; private set; }

        public DateTime? EndDate { get; private set; }

        public GameState(SqlDataReader reader)
        {
            if (reader.Read())
            {
                var xmlSerializer = new XmlSerializer(typeof(List<Player>));
                Id = Guid.Parse(reader[0]?.ToString() ?? string.Empty);
                Players = xmlSerializer.Deserialize(reader.GetXmlReader(1)) as List<Player> ?? new List<Player>();
                Deck = new Deck((reader[2].ToString() ?? string.Empty).Split(',').Select(cardType => short.Parse(cardType)));
                TurnPlayerNumber = Convert.ToInt16(reader[3]);
                WinnerPlayerNumber = reader.IsDBNull(4) ? null : Convert.ToInt16(reader[4]);
                StartDate = Convert.ToDateTime(reader[5]);
                EndDate = reader.IsDBNull(6) ? null : Convert.ToDateTime(reader[6]);
            }
        }

        public GameState UseAdapter(ISqlDataAdapter adapter)
        {
            _adapter = adapter;
            return this;
        }

        public static GameState Fetch(Guid lobbyId, SqlConnection connection)
        {
            var command = GameStateQuery.SelectById(lobbyId);
            var adapter = new GameStateSqlAdapter(connection);
            return ((GameState)adapter.Populate(command)).UseAdapter(adapter);
        }

        public bool Save<T>(T value)
        {
            var propertyInfo = typeof(GameState).GetProperty(nameof(value));

            if (propertyInfo is null)
            {
                return false;
            }

            var command = GameStateQuery.UpdateColumn(Id, value);
            _adapter.SaveChanges(command);
            return true;
        }

        public bool Save(params object[] values)
        {
            var stringValues = values.Select(v => JsonSerializer.Serialize(v)).ToArray() ?? Array.Empty<string>();
            var command = GameStateQuery.UpdateColumns(Id, stringValues);
            _adapter.SaveChanges(command);
            return true;
        }

        public bool InTurn(short playerNumber) =>
            TurnPlayerNumber == playerNumber;

        public Card TakeCard()
        {
            var card = Deck.Dequeue();
            var player = Players.FirstOrDefault(p => p.PlayerNumber == TurnPlayerNumber);

            if (player is not null)
            {
                player.CurrentCard = card;
                Save(Deck, Players);
                AuditItem.Append(Id, player, nameof(TakeCard), _adapter.Connection);
            }
            
            return card;
        }

        public void Win(short winnerPlayerNumber)
        {
            WinnerPlayerNumber = winnerPlayerNumber;
            EndDate = DateTime.Now;
            Save(WinnerPlayerNumber, EndDate);

            var player = Players.FirstOrDefault(p => p.PlayerNumber == WinnerPlayerNumber);

            if (player is not null)
            {
                AuditItem.Append(Id, player, nameof(Win), _adapter.Connection);
            }
        }

        public void Lose(Player loser)
        {
            Players.Remove(loser);
            Save(Players);
            AuditItem.Append(Id, loser, nameof(Lose), _adapter.Connection);
        }

        public void EndTurn(Card currentCard)
        {
            var player = Players.FirstOrDefault(p => p.PlayerNumber == TurnPlayerNumber);

            if (TurnPlayerNumber == Players.Count)
            {
                TurnPlayerNumber = 1;
            }
            else
            {
                TurnPlayerNumber++;
            }

            ReindexPlayers();
            Save(TurnPlayerNumber, Players);

            if (player is not null)
            {
                player.CurrentCard = new Card(currentCard);
                AuditItem.Append(Id, player, nameof(EndTurn), _adapter.Connection);
            }
        }

        public void ResetTarget(Player currentPlayer, Player opponent)
        {
            var opponentsLeft = Players.Where(p => p.PlayerNumber != currentPlayer.PlayerNumber && p.PlayerNumber != opponent.PlayerNumber);
            opponent = opponentsLeft.ElementAt(new Random().Next(0, opponentsLeft.Count()));
        }

        public void SwapCards(short currentPlayerNumber, Player opponent)
        {
            var currentPlayer = Players.FirstOrDefault(p => p.PlayerNumber == currentPlayerNumber);

            if (currentPlayer is not null)
            {
                var currentPlayerCard = currentPlayer.CurrentCard;
                currentPlayer.CurrentCard = new Card(opponent.CurrentCard);
                opponent.CurrentCard = new Card(currentPlayerCard);
                Save(Players);
                AuditItem.Append(Id, currentPlayer, nameof(SwapCards), _adapter.Connection);
                AuditItem.Append(Id, opponent, nameof(SwapCards), _adapter.Connection);
            }
        }

        private void ReindexPlayers() =>
            Players.ForEach(p => p.PlayerNumber = (short)(Players.IndexOf(p) + 1));
    }
}
