using LoveLetter.Core.Constants;
using LoveLetter.Core.Queries;
using System.ComponentModel;
using System.Data;
using System.Numerics;
using System.Text.Json;

namespace LoveLetter.Core.Entities
{
    public class GameState : DomainEntity
    {
        public Guid Id { get; private set; }

        public List<Player> Players { get; private set; } = new List<Player>();

        public Deck Deck { get; private set; } = new Deck();

        public short TurnPlayerNumber { get; private set; }

        public short? WinnerPlayerNumber { get; private set; }

        public DateTime StartDate { get; private set; }

        public DateTime? EndDate { get; private set; }

        public static GameState Fetch(Guid lobbyId)
        {
            var command = GameStateQuery.SelectById(lobbyId);
            return new GameState();
        }

        public bool Save<T>(T value)
        {
            var propertyInfo = typeof(GameState).GetProperty(nameof(value));

            if (propertyInfo is null)
            {
                return false;
            }

            var command = GameStateQuery.UpdateColumn(Id, value);
            return true;
        }

        public bool Save(params object[] values)
        {
            var stringValues = values.Select(v => JsonSerializer.Serialize(v)).ToArray() ?? Array.Empty<string>();
            var command = GameStateQuery.UpdateColumns(Id, stringValues);
            return true;
        }

        public bool InTurn(short playerNumber) =>
            TurnPlayerNumber == playerNumber;

        public Card TakeCard(bool isInitial = false)
        {
            var card = Deck.Dequeue();
            Save(Deck);

            var player = Players.FirstOrDefault(p => p.PlayerNumber == TurnPlayerNumber);

            if (player is not null)
            {
                AuditItem.Append(Id, player, nameof(TakeCard));
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
                AuditItem.Append(Id, player, nameof(Win));
            }
        }

        public void Lose(Player loser)
        {
            Players.Remove(loser);
            Save(Players);
            AuditItem.Append(Id, loser, nameof(Lose));
        }

        public void EndTurn(Card currentCard)
        {
            var player = Players.FirstOrDefault(p => p.PlayerNumber == TurnPlayerNumber);

            if (player is not null)
            {
                player.CurrentCard = new Card(currentCard);

                if (TurnPlayerNumber == Players.Count)
                {
                    TurnPlayerNumber = 1;
                }
                else
                {
                    TurnPlayerNumber++;
                }

                Save(TurnPlayerNumber, Players);
                AuditItem.Append(Id, player, nameof(EndTurn));
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
                AuditItem.Append(Id, currentPlayer, nameof(SwapCards));
                AuditItem.Append(Id, opponent, nameof(SwapCards));
            }
        }
    }
}
