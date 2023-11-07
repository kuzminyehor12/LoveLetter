using LoveLetter.Core.Constants;
using LoveLetter.Core.Entities;
using LoveLetter.Core.Queries;
using LoveLetter.Core.Utils;
using Microsoft.Data.SqlClient;
using System.Data;

namespace LoveLetter.UI.Infrastructure
{
    public static class DataGridUtils
    {
        public static DataTable GetLobbyDataTable(SqlConnection connection)
        {
            DataTable lobbies = new DataTable();
            lobbies.Columns.Add(nameof(Lobby.Id));
            lobbies.Columns.Add(nameof(Lobby.Status));
            lobbies.Columns.Add(nameof(Lobby.Players));
            lobbies.Columns.Add("PlayersCount");
            var command = LobbyQuery.SelectAll();
            
            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText = command;
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var players = reader.IsDBNull(2) ? string.Empty : reader.GetString(2);
                        lobbies.Rows.Add(
                            reader.GetGuid(0),
                            (LobbyStatus)reader.GetInt16(1),
                            players,
                            (players?.Split(',').Length ?? 0) + "/" + Constraints.MAX_PLAYER_NUMBER);
                    }
                }
            }

            return lobbies;
        }

        public static DataTable LoadAudit(Guid gameStateId, SqlConnection connection)
        {
            DataTable audit = new DataTable();
            audit.Columns.Add(nameof(AuditItem.Message));
            var command = AuditQuery.SelectAll(gameStateId);

            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText = command;
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var message = CreateAuditItem(reader);

                        if (!string.IsNullOrEmpty(message))
                        {
                            audit.Rows.Add(message);
                        }
                    }
                }
            }

            return audit;

            string CreateAuditItem(SqlDataReader reader) =>
                reader.IsDBNull(4) ? string.Empty : 
                    reader.GetString(4) + $" by player {reader.GetInt16(3)}({reader.GetString(2)}) at {reader.GetDateTime(5)}";
        }
    }
}
