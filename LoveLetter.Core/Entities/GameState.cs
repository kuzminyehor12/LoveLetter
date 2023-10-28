using LoveLetter.Core.Constants;
using LoveLetter.Core.Queries;
using System.Data;
using System.Text.Json;

namespace LoveLetter.Core.Entities
{
    public class GameState : DomainEntity
    {
        public Guid Id { get; private set; }

        public IEnumerable<Player> Players { get; private set; } = Enumerable.Empty<Player>();

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
            return card;
        }

        public void SetWinner(short winnerPlayerNumber)
        {
            WinnerPlayerNumber = winnerPlayerNumber;
            EndDate = DateTime.Now;
            Save(WinnerPlayerNumber, EndDate);
        }

        public void EndTurn(Card currentCard)
        {
            var player = Players.FirstOrDefault(p => p.PlayerNumber == TurnPlayerNumber);

            if (player is not null)
            {
                player.CurrentCard = new Card(currentCard);
            }

            if (TurnPlayerNumber == Constraints.MAX_PLAYER_NUMBER)
            {
                TurnPlayerNumber = 1;
            }
            else
            {
                TurnPlayerNumber++;
            }

            Save(TurnPlayerNumber, Players);
        }
    }
}
