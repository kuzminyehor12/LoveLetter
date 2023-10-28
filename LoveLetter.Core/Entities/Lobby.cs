using LoveLetter.Core.Adapters;
using LoveLetter.Core.Constants;
using LoveLetter.Core.Exceptions;
using LoveLetter.Core.Queries;
using LoveLetter.Core.Utils;
using Microsoft.Data.SqlClient;

namespace LoveLetter.Core.Entities
{
    public class Lobby : DomainEntity
    {
        private ISqlDataAdapter _adapter = new LobbySqlAdapter();

        public Guid Id { get; private set; }

        public LobbyStatus Status { get; private set; }

        public List<string> Players { get; private set; } = new List<string>();

        public Lobby(SqlDataReader reader)
        {
            if (reader.Read())
            {
                Id = Guid.Parse(reader[0]?.ToString() ?? string.Empty);
                Status = (LobbyStatus)Convert.ToInt16(reader[1]);
                Players = reader.IsDBNull(2) || string.IsNullOrEmpty(reader[2].ToString()) ? 
                    new List<string>() 
                    : reader[2].ToString().Split(',').ToList();
            }
        }

        public Lobby UseAdapter(ISqlDataAdapter adapter)
        {
            _adapter = adapter;
            return this;
        }

        public static Lobby CreateNew(string hostNickname, SqlConnection connection)
        {
            if (string.IsNullOrEmpty(hostNickname))
            {
                hostNickname = "Player " + 1;
            }

            var command = LobbyQuery.Insert(hostNickname, out var lobbyId);
            var adapter = new LobbySqlAdapter(connection);
            adapter.SaveChanges(command);
            return ((Lobby)adapter.Populate(LobbyQuery.SelectById(lobbyId))).UseAdapter(adapter);
        }

        public static Lobby Fetch(Guid lobbyId, SqlConnection connection)
        {
            var command = LobbyQuery.SelectById(lobbyId);
            var adapter = new LobbySqlAdapter(connection);
            return (Lobby)adapter.Populate(command);
        }

        public static Lobby Join(Guid lobbyId, string nickname, SqlConnection connection)
        {
            var lobby = Fetch(lobbyId, connection);

            if (lobby is null)
            {
                throw new NullReferenceException();
            }

            if (string.IsNullOrEmpty(nickname))
            {
                nickname = "Player " + lobby.Players.Count + 1;
            }

            if (lobby.Players.Count == Constraints.MAX_PLAYER_NUMBER)
            {
                throw new FullLobbyException();
            }

            lobby.Players.Add(nickname);
            var command = LobbyQuery.UpdatePlayers(lobbyId, lobby.Players);
            AuditItem.Append(lobbyId, nickname, nameof(Join), connection);
            return lobby;
        }

        public bool Leave(string nickname)
        {
            Players.Remove(nickname);
            var command = LobbyQuery.UpdatePlayers(Id, Players);
            AuditItem.Append(Id, nickname, nameof(Leave), _adapter.Connection);
            return true;
        }

        public bool Start(string nickname)
        {
            if (Players.Count < 2 || Players.Count > Constraints.MAX_PLAYER_NUMBER)
            {
                return false;
            }

            var command = LobbyQuery.Start(Id);
            AuditItem.Append(Id, nickname, nameof(Start), _adapter.Connection);
            return true;
        }

        public bool Close()
        {
            var command = LobbyQuery.Close(Id);
            AuditItem.Append(Id, nameof(Lobby), nameof(Close), _adapter.Connection);
            return true;
        }
    }
}
