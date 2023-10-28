using LoveLetter.Core.Constants;

namespace LoveLetter.Core.Queries
{
    public static class AuditQuery
    {
        public static string Insert(Guid gameStateId, string playerNickname, short playerNumber, string message) =>
            $"INSERT INTO {Tables.Audit} (Id, PlayerNickname, PlayerNumber, Message, GameStateId) " +
            $"VALUES ({Guid.NewGuid()}, {playerNickname}, {playerNumber}, {message}, {gameStateId})";
    }
}
