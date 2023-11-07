using LoveLetter.Core.Adapters;
using LoveLetter.Core.Exceptions;
using LoveLetter.Core.Queries;
using LoveLetter.Core.Utils;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Numerics;
using System.Xml.Serialization;

namespace LoveLetter.Core.Entities
{
    public class GameState : DomainEntity
    {
        private DomainSqlDataAdapter _adapter = new GameStateSqlAdapter();

        public Guid Id { get; private set; }

        [XmlElement("Players")]
        public List<Player> Players { get; private set; } = new List<Player>();

        public Deck Deck { get; private set; } = new Deck();

        public short TurnPlayerNumber { get; private set; }

        public short? WinnerPlayerNumber { get; private set; }

        public DateTime StartDate { get; private set; }

        public DateTime? EndDate { get; private set; }

        public Deck CardHistory { get; private set; }

        public bool Locked { get; private set; }

        public GameState(SqlDataReader reader)
        {
            if (reader.Read())
            {
                var xmlSerializer = new XmlSerializer(typeof(PlayersList));
                Id = reader.GetGuid(0);
                Players = xmlSerializer.Deserialize(reader.GetXmlReader(1)) as List<Player> ?? PlayersList.Empty();
                Deck = string.IsNullOrEmpty(reader.GetString(2)) ? new Deck() : 
                    new Deck(reader.GetString(2).Split(',').Select(cardType => short.Parse(cardType)));
                TurnPlayerNumber = reader.GetInt16(3);
                WinnerPlayerNumber = reader.IsDBNull(4) ? null : reader.GetInt16(4);
                StartDate = reader.GetDateTime(5);
                EndDate = reader.IsDBNull(6) ? null : reader.GetDateTime(6);
                CardHistory = string.IsNullOrEmpty(reader.GetString(7)) ? new Deck() : new Deck(reader.GetString(7).Split(',').Select(cardType => short.Parse(cardType)));
                Locked = reader.GetBoolean(8);
            }
            else
            {
                throw new NotExistingEntityException();
            }
        }

        public GameState UseAdapter(DomainSqlDataAdapter adapter)
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

        public bool Save((string ColumnName, object ColumnValue) column)
        {
            try
            {
                var columnResult = ParseUtils.ParseValues(column).SingleOrDefault();

                if (string.IsNullOrEmpty(columnResult.ColumnValue))
                {
                    return false;
                }

                var command = GameStateQuery.UpdateColumn(Id, columnResult);
                _adapter.SaveChanges(command);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool Save(SqlTransaction transaction, params (string ColumnName, object ColumnValue)[] columns)
        {
            try
            {
                var stringValues = ParseUtils.ParseValues(columns);
                var command = GameStateQuery.UpdateColumns(Id, stringValues.ToArray());
                _adapter.SaveChanges(command, transaction);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }


        public bool Save(params (string ColumnName, object ColumnValue)[] columns)
        {
            try
            {
                var stringValues = ParseUtils.ParseValues(columns);
                var command = GameStateQuery.UpdateColumns(Id, stringValues.ToArray());
                _adapter.SaveChanges(command);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool InTurn(short playerNumber) =>
            TurnPlayerNumber == playerNumber;

        public Card TakeCard(Player player)
        {
            var card = Deck.Dequeue();
            Save((nameof(Locked), true));

            if (player is not null)
            {
                var playerToUpdate = Players.FirstOrDefault(p => p.PlayerNumber == player.PlayerNumber);
                if (playerToUpdate is not null)
                {
                    var updatePlayerCard = (SqlTransaction transaction) =>
                    {
                        playerToUpdate.CurrentCard = card;
                        playerToUpdate.Available = true;
                        var resultOk = Save(transaction, (nameof(Deck), Deck), (nameof(Players), Players));

                        if (resultOk)
                        {
                            AuditItem.Append(Id, player, nameof(TakeCard), _adapter.Connection, transaction);
                        }
                    };

                    _adapter.DoAsTransaction(updatePlayerCard, IsolationLevel.ReadCommitted);
                }
            }

            Save((nameof(Locked), false));
            return card;
        }

        public void PopulateCardHistory(Card card)
        {
            CardHistory.Enqueue(card);
            Save((nameof(CardHistory), CardHistory));
        }

        public void Win(short winnerPlayerNumber)
        {
            WinnerPlayerNumber = winnerPlayerNumber;
            EndDate = DateTime.Now;
            var resultOk = Save((nameof(WinnerPlayerNumber), WinnerPlayerNumber), (nameof(EndDate), EndDate));
            var player = Players.FirstOrDefault(p => p.PlayerNumber == WinnerPlayerNumber);

            if (player is not null && resultOk)
            {
                AuditItem.Append(Id, player, nameof(Win), _adapter.Connection);
            }
        }

        public void Lose(Player loser)
        {
            var playerToRemove = Players.FirstOrDefault(p => loser.PlayerNumber == p.PlayerNumber);

            if (playerToRemove is not null)
            {
                Players.Remove(playerToRemove);
                var resultOk = Save((nameof(Players), Players));

                if (resultOk)
                {
                    AuditItem.Append(Id, loser, nameof(Lose), _adapter.Connection);
                }
            }
        }

        public void EndTurn(Card currentCard)
        {
            var player = Players.FirstOrDefault(p => p.PlayerNumber == TurnPlayerNumber);

            if (player is not null)
            {
                player.CurrentCard = new Card(currentCard);
                if (TurnPlayerNumber >= Players.Count)
                {
                    TurnPlayerNumber = 1;
                }
                else
                {
                    TurnPlayerNumber++;
                }

                ReindexPlayers();
                var resultOk = Save((nameof(TurnPlayerNumber), TurnPlayerNumber), (nameof(Players), Players));

                if (resultOk)
                {
                    AuditItem.Append(Id, player, nameof(EndTurn), _adapter.Connection);
                }
            }
        }

        public void ResetTarget(Player currentPlayer, Player opponent)
        {
            var opponentsLeft = Players.Where(p => p.PlayerNumber != currentPlayer.PlayerNumber && p.PlayerNumber != opponent.PlayerNumber);
            opponent = opponentsLeft.ElementAt(new Random().Next(0, opponentsLeft.Count()));
        }

        public Card SwapCards(short currentPlayerNumber, Player opponent)
        {
            var currentPlayer = Players.FirstOrDefault(p => p.PlayerNumber == currentPlayerNumber);

            if (currentPlayer is not null)
            {
                var currentPlayerCard = currentPlayer.CurrentCard;
                currentPlayer.CurrentCard = new Card(opponent.CurrentCard);
                opponent.CurrentCard = new Card(currentPlayerCard);
                var resultOk = Save((nameof(Players), Players));

                if (resultOk)
                {
                    AuditItem.Append(Id, currentPlayer, nameof(SwapCards), _adapter.Connection);
                }

                return new Card(currentPlayer.CurrentCard);
            }

            return new Card();
        }

        private void ReindexPlayers() =>
            Players.ForEach(p => p.PlayerNumber = (short)(Players.IndexOf(p) + 1));
    }
}
