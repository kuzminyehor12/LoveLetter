using LoveLetter.Core.Entities;
using LoveLetter.Core.Exceptions;
using LoveLetter.UI.Infrastructure;
using System.Data;
using System.Threading;

namespace LoveLetter.UI.Forms
{
    public partial class LobbiesForm : Form
    {
        public LobbiesForm()
        {
            InitializeComponent();
        }

        private void LobbiesGrid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (Guid.TryParse(LobbiesGrid.Rows[e.RowIndex].Cells[0].Value.ToString(), out var lobbyId))
                {
                    ApplicationState.Instance.CurrentLobby = Lobby.Join(lobbyId, NicknameValue.Text.Trim(), ApplicationState.Instance.Connection);
                    short yourPlayerNumber = (short)ApplicationState.Instance.CurrentLobby.Players.Count;
                    JoinWaitingRoom(yourPlayerNumber);
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
                ApplicationState.Instance.CurrentLobby = Lobby.CreateNew(NicknameValue.Text.Trim(), ApplicationState.Instance.Connection);
                JoinWaitingRoom();
            }
            catch (Exception ex)
            {
                this.ThrowError(ex);
            }
        }

        private void JoinWaitingRoom(short yourNumber = 1)
        {
            new WaitingRoomForm(NicknameValue.Text.Trim(), yourNumber).Show();
            Hide();
        }

        private void LobbiesForm_Load(object sender, EventArgs e)
        {
            ApplicationState.Instance.ApplicationEvents.OnGameStopped += GameForm_OnClosed;
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
    }
}
