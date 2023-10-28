using LoveLetter.Core.Constants;
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
            ApplicationState.Instance.ApplicationEvents.OnGameStopped += ApplicationEvents_OnGameStopped;
            InitializeComponent();
        }

        private void ApplicationEvents_OnGameStopped(object? sender, EventArgs e)
        {
            Close();
        }

        private void GameStartBtn_Click(object sender, EventArgs e)
        {
            try
            {
                var lobby = ApplicationState.Instance.CurrentLobby;
                var player = ApplicationState.Instance.CurrentPlayer;

                if (lobby is null)
                {
                    throw new NullReferenceException(nameof(lobby));
                }

                if (player is null)
                {
                    throw new NullReferenceException(nameof(player));
                }

                var resultOk = lobby.Start(player.NickName);

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

        private void QuitLobbyBtn_Click(object sender, EventArgs e)
        {
            try
            {
                var lobby = ApplicationState.Instance.CurrentLobby;

                if (lobby is null)
                {
                    throw new NullReferenceException(nameof(lobby));
                }

                lobby.Close();

                if (ApplicationState.Instance.ApplicationEvents is not null)
                {
                    ApplicationState.Instance.ApplicationEvents.OnGameStoppedHandler(e);
                }
            }
            catch (Exception ex)
            {
                this.ThrowError(ex);
            }
        }

        private void WaitingRoomForm_Load(object sender, EventArgs e)
        {
            RefreshPlayersList();
        }

        private void RefreshPlayersList()
        {
            try
            {
                var lobby = ApplicationState.Instance.CurrentLobby;

                if (lobby is null)
                {
                    throw new NullReferenceException(nameof(lobby));
                }

                lobby = Lobby.Fetch(lobby.Id);

                if (lobby.Players.Count == Constraints.MAX_PLAYER_NUMBER)
                {
                    PollingTimer.Enabled = false;
                    return;
                }
                else if (WaitingRoomListView.Items.Count != lobby.Players.Count)
                {
                    WaitingRoomListView.Items.Clear();
                    lobby.Players.ForEach(p => WaitingRoomListView.Items.Add(p));
                    WaitingRoomListView.Refresh();
                }
            }
            catch (Exception ex)
            {
                this.ThrowError(ex);
            }
        }

        private void PollingTimer_Tick(object sender, EventArgs e)
        {
            PollingTimer.Stop();
            RefreshPlayersList();
            PollingTimer.Start();
        }

        private void WaitingRoomForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            PollingTimer.Stop();
        }
    }
}
