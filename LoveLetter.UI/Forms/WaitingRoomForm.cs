using LoveLetter.Core.Constants;
using LoveLetter.Core.Entities;
using LoveLetter.Core.Exceptions;
using LoveLetter.UI.Infrastructure;

namespace LoveLetter.UI.Forms
{
    public partial class WaitingRoomForm : Form
    {
        private bool _joinedGame = false;
        private short _yourPlayerNumber;
        public short YourPlayerNumber
        {
            get
            {
                return _yourPlayerNumber;
            }
            private set
            {
                if (value > Constraints.MAX_PLAYER_NUMBER)
                {
                    throw new FullLobbyException();
                }

                _yourPlayerNumber = value;
            }
        }
        public WaitingRoomForm(string playerNickname, short yourPlayerNumber)
        {
            YourPlayerNumber = yourPlayerNumber;
            ApplicationState.Instance.CurrentPlayer = new Player(yourPlayerNumber, playerNickname);
            ApplicationState.Instance.ApplicationEvents.OnGameStopped += ApplicationEvents_WaitingRoom_OnGameStopped;
            InitializeComponent();
        }

        private void ApplicationEvents_WaitingRoom_OnGameStopped(object? sender, EventArgs e)
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

                lobby = Lobby.Fetch(lobby.Id, ApplicationState.Instance.Connection);
                var resultOk = lobby.Start(player.Nickname);

                if (resultOk)
                {
                    ApplicationState.Instance.CurrentGameState = GameState.Fetch(lobby.Id, ApplicationState.Instance.Connection);
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
            _joinedGame = true;
            Close();
        }

        private void QuitLobbyBtn_Click(object sender, EventArgs e)
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

                if (lobby.Players.Count <= 1)
                {
                    lobby.Close();
                }
                else
                {
                    lobby.Leave(player.Nickname);
                }

                if (ApplicationState.Instance.ApplicationEvents is not null)
                {
                    _joinedGame = false;
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

                lobby = Lobby.Fetch(lobby.Id, ApplicationState.Instance.Connection);

                if (lobby.Players.Count == Constraints.MAX_PLAYER_NUMBER)
                {
                    PollingTimer.Enabled = false;
                    return;
                }
                else if (WaitingRoomListBox.Items.Count != lobby.Players.Count)
                {
                    WaitingRoomListBox.Items.Clear();
                    lobby.Players.ForEach(p => WaitingRoomListBox.Items.Add(p));
                    WaitingRoomListBox.Refresh();
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
            if (!_joinedGame)
            {
                ApplicationState.Instance.CurrentLobby?.Leave(ApplicationState.Instance.CurrentPlayer?.Nickname ?? string.Empty);
            }
            
            ApplicationState.Instance.ApplicationEvents.OnGameStopped -= ApplicationEvents_WaitingRoom_OnGameStopped;
            PollingTimer.Stop();
        }

        private void GameStartTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                var lobby = ApplicationState.Instance.CurrentLobby;

                if (lobby is null)
                {
                    throw new NullReferenceException(nameof(lobby));
                }

                GameStartTimer.Stop();

                ApplicationState.Instance.CurrentGameState = GameState.Fetch(lobby.Id, ApplicationState.Instance.Connection);

                GameStartTimer.Enabled = false;
                JoinGame();
            }
            catch (NotExistingEntityException)
            {
                GameStartTimer.Start();
                return;
            }
            catch (Exception ex)
            {
                this.ThrowError(ex);
            }
        }
    }
}
