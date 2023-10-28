using LoveLetter.Core.Constants;
using System.ComponentModel.DataAnnotations;

namespace Database.Migrations.Entities
{
    public class AuditItemEntity : MigrationEntity
    {
        public Guid GameStateId { get; set; }

        public GameStateEntity GameState { get; set; } = new GameStateEntity();

        public string PlayerNickname { get; set; } = string.Empty;

        [Range(1, Constraints.MAX_PLAYER_NUMBER)]
        public short PlayerNumber { get; set; }

        public string Message { get; set; } = string.Empty;
    }
}
