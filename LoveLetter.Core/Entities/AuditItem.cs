using LoveLetter.Core.Queries;
using Microsoft.Data.SqlClient;

namespace LoveLetter.Core.Entities
{
    public class AuditItem : DomainEntity
    {
        public Guid Id { get; private set; }

        public string PlayerNickname { get; private set; } = string.Empty;

        public short PlayerNumber { get; private set; }

        public string Message { get; private set; } = string.Empty;

        public DateTime Timestamp { get; private set; }

        public static bool Append(Guid gameStateId, Player player, string message, SqlConnection connection)
        {
            var playerNickname = player.NickName;
            var command = AuditQuery.Insert(gameStateId, playerNickname, player.PlayerNumber, message);
            
            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText = command;
                cmd.ExecuteNonQuery();
            }

            return true;
        }

        public static bool Append(Guid gameStateId, string nickname, string message, SqlConnection connection)
        {
            var command = AuditQuery.Insert(gameStateId, nickname, message);

            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText = command;
                cmd.ExecuteNonQuery();
            }

            return true;
        }
    }
}
