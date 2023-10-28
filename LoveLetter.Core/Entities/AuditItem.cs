using LoveLetter.Core.Queries;

namespace LoveLetter.Core.Entities
{
    public class AuditItem : DomainEntity
    {
        public Guid Id { get; private set; }

        public string PlayerNickname { get; private set; } = string.Empty;

        public short PlayerNumber { get; private set; }

        public string Message { get; private set; } = string.Empty;

        public bool Append(Guid gameStateId, Player player, string message)
        {
            var playerNickname = player.NickName ?? "Player " + player.PlayerNumber;
            var command = AuditQuery.Insert(gameStateId, playerNickname, player.PlayerNumber, message);
            return true;
        }
    }
}
