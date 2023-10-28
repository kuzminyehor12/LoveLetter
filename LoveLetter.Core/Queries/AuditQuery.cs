using LoveLetter.Core.Constants;

namespace LoveLetter.Core.Queries
{
    public static class AuditQuery
    {
        public static string Insert(Guid gameStateId, string playerNickname, short playerNumber, string message) =>
            $"INSERT INTO {Tables.Audit} (Id, PlayerNickname, PlayerNumber, Message, GameStateId, Timestamp) " +
            $"VALUES ({Guid.NewGuid()}, {playerNickname}, {playerNumber}, {message}, {gameStateId}, {DateTime.Now})";

        public static string Insert(Guid gameStateId, string playerNickname, string message) =>
            $"INSERT INTO {Tables.Audit} (Id, PlayerNickname, PlayerNumber, Message, GameStateId, Timestamp) " +
            $"VALUES ({Guid.NewGuid()}, {playerNickname}, 0, {message}, {gameStateId}, {DateTime.Now})";

        public static string SelectAll(Guid gameStateId) => 
            $"SELECT * FROM {Tables.Audit} WHERE GameStateId={gameStateId} ORDER BY Timestamp DESC";
    }
}
