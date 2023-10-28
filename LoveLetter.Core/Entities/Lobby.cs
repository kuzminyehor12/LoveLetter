using LoveLetter.Core.Queries;
using LoveLetter.Core.Utils;

namespace LoveLetter.Core.Entities
{
    public class Lobby : DomainEntity
    {
        public Guid Id { get; private set; }

        public LobbyStatus Status { get; private set; }

        public List<string> Players { get; private set; } = new List<string>();

        public static Lobby CreateNew(string hostNickname)
        {
            if (string.IsNullOrEmpty(hostNickname))
            {
                hostNickname = "Player " + 1;
            }

            var command = LobbyQuery.Insert(hostNickname);
            return new Lobby();
        }

        public static Lobby Fetch(Guid lobbyId)
        {
            var command = LobbyQuery.SelectById(lobbyId);
            return new Lobby();
        }

        public static IEnumerable<Lobby> FetchAll()
        {
            var command = LobbyQuery.SelectAll();
            return Enumerable.Empty<Lobby>();
        }

        public static Lobby Join(Guid lobbyId, string nickname)
        {
            var lobby = Fetch(lobbyId);

            if (lobby is null)
            {
                throw new Exception();
            }

            if (string.IsNullOrEmpty(nickname))
            {
                nickname = "Player " + lobby.Players.Count + 1;
            }

            lobby.Players.Add(nickname);
            var command = LobbyQuery.UpdatePlayers(lobbyId, lobby.Players);
            return lobby;
        }

        public bool Leave(string nickname)
        {
            Players.Remove(nickname);
            var command = LobbyQuery.UpdatePlayers(Id, Players);
            return true;
        }

        public bool Start()
        {
            if (Players.Count < 2 || Players.Count > 4)
            {
                return false;
            }

            var command = LobbyQuery.Start(Id);
            return true;
        }

        public bool Close()
        {
            var command = LobbyQuery.Close(Id);
            return true;
        }
    }
}
