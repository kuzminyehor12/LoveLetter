using LoveLetter.Core.Constants;
using System.ComponentModel.DataAnnotations;

namespace Database.Migrations.Entities
{
    public class GameStateEntity : MigrationEntity
    {
        public LobbyEntity Lobby { get; set; } = new LobbyEntity();

        public virtual ICollection<AuditItemEntity>? AuditItems { get; set; }

        public string? Players { get; set; } = string.Empty;

        public string Deck { get; set; } = string.Empty;

        [Range(1, Constraints.MAX_PLAYER_NUMBER)]
        public short TurnPlayerNumber { get; set; }

        [Range(1, Constraints.MAX_PLAYER_NUMBER)]
        public short? WinnerPlayerNumber { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public string CardHistory { get; set; } = string.Empty;

        public bool Locked { get; set; } = false;
    }
}
