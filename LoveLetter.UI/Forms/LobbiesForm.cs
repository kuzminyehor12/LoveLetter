using LoveLetter.Core.Entities;
using LoveLetter.Core.Exceptions;
using LoveLetter.Core.Utils;
using LoveLetter.UI.Infrastructure;

namespace LoveLetter.UI.Forms
{
    public partial class LobbiesForm : Form
    {
        public LobbiesForm()
        {
            InitializeComponent();
            ApplicationState.Instance.ApplicationEvents.OnGameStopped += GameForm_OnClosed;
        }

        private void LobbiesGrid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(NicknameValue.Text))
                {
                    this.ThrowIssue("You have to enter nickname before starting to play game!");
                    return;
                }

                if (Guid.TryParse(LobbiesGrid.Rows[e.RowIndex].Cells[0].Value.ToString(), out var lobbyId) 
                    && Enum.TryParse(typeof(LobbyStatus), LobbiesGrid.Rows[e.RowIndex].Cells[1].Value.ToString(), out var lobbyStatus)
                    && (LobbyStatus?)lobbyStatus == LobbyStatus.Open)
                {
                    ApplicationState.Instance.CurrentLobby = Lobby.Join(lobbyId, NicknameValue.Text.Trim(), ApplicationState.Instance.Connection);
                    short yourPlayerNumber = (short)ApplicationState.Instance.CurrentLobby.Players.Count;
                    JoinWaitingRoom(yourPlayerNumber, false);
                }
                else
                {
                    this.ThrowIssue("Lobby is in progress! Try to find another one.");
                }
            }
            catch (FullLobbyException)
            {
                this.ThrowIssue("Lobby is full! Try to find another one.");
            }
            catch (Exception ex)
            {
                this.ThrowError(ex);
            }
        }

        private void CreateLobbyBtn_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(NicknameValue.Text))
                {
                    this.ThrowIssue("You have to enter nickname before starting to play game!");
                    return;
                }

                ApplicationState.Instance.CurrentLobby = Lobby.CreateNew(NicknameValue.Text.Trim(), ApplicationState.Instance.Connection);
                JoinWaitingRoom(isHost: true);
            }
            catch (Exception ex)
            {
                this.ThrowError(ex);
            }
        }

        private void JoinWaitingRoom(short yourNumber = 1, bool isHost = false)
        {
            new WaitingRoomForm(NicknameValue.Text.Trim(), isHost, yourNumber).Show();
            Hide();
        }

        private void LobbiesForm_Load(object sender, EventArgs e)
        {
            ApplicationState.Instance.Connection.Open();
            LobbiesGrid.DataSource = DataGridUtils.GetLobbyDataTable(ApplicationState.Instance.Connection);
        }

        private void GameForm_OnClosed(object? sender, EventArgs e)
        {
            Show();
        }

        private void LobbiesForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            ApplicationState.Instance.Connection.Close();
            ApplicationState.Instance.ApplicationEvents.OnGameStopped -= GameForm_OnClosed;
        }

        private void RefreshIcon_Click(object sender, EventArgs e)
        {
            LobbiesGrid.DataSource = DataGridUtils.GetLobbyDataTable(ApplicationState.Instance.Connection);
            LobbiesGrid.Update();
            LobbiesGrid.Refresh();
        }

        private void LobbiesTimer_Tick(object sender, EventArgs e)
        {
            LobbiesTimer.Stop();
            LobbiesGrid.DataSource = DataGridUtils.GetLobbyDataTable(ApplicationState.Instance.Connection);
            LobbiesGrid.Update();
            LobbiesGrid.Refresh();
            LobbiesTimer.Start();
        }
    }
}
