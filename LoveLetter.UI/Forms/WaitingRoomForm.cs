using LoveLetter.Core.Entities;
using LoveLetter.UI.Infrastructure;

namespace LoveLetter.UI.Forms
{
    public partial class WaitingRoomForm : Form
    {
        private short _yourPlayerNumber;
        public short YourPlayerNumber
        {
            get
            {
                return _yourPlayerNumber;
            }
            private set
            {
                if (value > 4)
                {
                    throw new Exception();
                }

                _yourPlayerNumber = value;
            }
        }
        public WaitingRoomForm(string playerNickname, short yourPlayerNumber)
        {
            YourPlayerNumber = yourPlayerNumber;
            ApplicationState.Instance.CurrentPlayer = new Player(yourPlayerNumber, playerNickname);
            InitializeComponent();
        }

        private void GameStartBtn_Click(object sender, EventArgs e)
        {
            try
            {
                var lobby = ApplicationState.Instance.CurrentLobby;

                if (lobby is null)
                {
                    throw new NullReferenceException(nameof(lobby));
                }
                
                var resultOk = lobby.Start();

                if (resultOk)
                {
                    ApplicationState.Instance.CurrentGameState = GameState.Fetch(lobby.Id);
                    JoinGame();
                }
                else
                {
                    this.ThrowIssue("Inappropriate amount of players.\nImpossible to start the game!");
                    ApplicationState.Instance.ApplicationEvents.OnGameStoppedHandler(e);
                }
            }
            catch (Exception ex)
            {
                this.ThrowError(ex);
            }
        }

        private void JoinGame()
        {
            new GameForm().Show();
            Close();
        }
    }
}
