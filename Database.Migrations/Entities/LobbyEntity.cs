using LoveLetter.Core.Utils;

namespace Database.Migrations.Entities
{
    public class LobbyEntity : MigrationEntity
    {
        public GameStateEntity? State { get; set; }

        public LobbyStatus Status { get; set; }

        public string Players { get; set; } = string.Empty;
    }
}
