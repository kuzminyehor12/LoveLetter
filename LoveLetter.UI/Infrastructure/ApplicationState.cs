using LoveLetter.Core.Entities;
using LoveLetter.Core.Utils;
using Microsoft.Data.SqlClient;

namespace LoveLetter.UI.Infrastructure
{
    public class ApplicationState
    {
        private static ApplicationState? _applicationState;

        private ApplicationState()
        {
            CurrentLobby = null;
            CurrentGameState = null;
            CardEvents = null;
            ApplicationEvents = new ApplicationEvents();
        }

        public static ApplicationState Instance
        {
            get
            {
                if (_applicationState is null)
                {
                    _applicationState = new ApplicationState();
                }

                return _applicationState;
            }
        }

        public Lobby? CurrentLobby { get; set; }

        public GameState? CurrentGameState { get; set; }

        public Player? CurrentPlayer { get; set; }

        public CardEvents? CardEvents { get; set; }

        public ApplicationEvents ApplicationEvents { get; set; }

        public SqlConnection Connection { get; set; }
    }
}
