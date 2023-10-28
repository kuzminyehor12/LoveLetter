using LoveLetter.Core.Entities;
using LoveLetter.UI.Infrastructure;

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
                    ApplicationState.Instance.CurrentLobby = Lobby.Join(lobbyId, NicknameValue.Text.Trim());
                    short yourPlayerNumber = (short)ApplicationState.Instance.CurrentLobby.Players.Count;
                    JoinWaitingRoom(yourPlayerNumber);
                }
            }
            catch (Exception ex)
            {
                this.ThrowError(ex);
            }
        }

        private void LobbiesGrid_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            LobbiesGrid_CellDoubleClick(sender, e);
        }

        private void CreateLobbyBtn_Click(object sender, EventArgs e)
        {
            try
            {
                ApplicationState.Instance.CurrentLobby = Lobby.CreateNew(NicknameValue.Text.Trim());
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
        }

        private void GameForm_OnClosed(object? sender, EventArgs e)
        {
            Show();
        }
    }
}
