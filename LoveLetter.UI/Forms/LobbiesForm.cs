using LoveLetter.Core.Entities;
using LoveLetter.Core.Exceptions;
using LoveLetter.UI.Infrastructure;
using System.Data;
using System.Threading;

namespace LoveLetter.UI.Forms
{
    public partial class LobbiesForm : Form
    {
        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private Thread? _updateDataGridViewThread = null;
        public LobbiesForm()
        {
            InitializeComponent();
        }

        private void LobbiesGrid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                var lobby = ApplicationState.Instance.CurrentLobby;

                if (lobby is null)
                {
                    throw new NullReferenceException(nameof(lobby));
                }

                if (Guid.TryParse(LobbiesGrid.Rows[e.RowIndex].Cells[0].Value.ToString(), out var lobbyId))
                {
                    lobby = Lobby.Join(lobbyId, NicknameValue.Text.Trim(), ApplicationState.Instance.Connection);
                    short yourPlayerNumber = (short)lobby.Players.Count;
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

        private void LobbiesGrid_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            LobbiesGrid_CellDoubleClick(sender, e);
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
            _updateDataGridViewThread = new Thread(new ParameterizedThreadStart(TrackLobbies));
            _updateDataGridViewThread.Start(_cancellationTokenSource.Token);
        }

        private void TrackLobbies(object? cancellationToken = default)
        {
            if (cancellationToken is CancellationToken token && token.IsCancellationRequested)
            {
                return;
            }

            SetDataSourceInGridView(DataGridUtils.GetLobbyDataTable(ApplicationState.Instance.Connection));
        }

        private void SetDataSourceInGridView(DataTable table)
        {
            if (LobbiesGrid.InvokeRequired)
            {
                Invoke(SetDataSourceInGridView, new object[] { table });
            }
            else
            {
                LobbiesGrid.DataSource = table;
                LobbiesGrid.Update();
                LobbiesGrid.Refresh();
            }
        }

        private void GameForm_OnClosed(object? sender, EventArgs e)
        {
            Show();
        }

        private void LobbiesForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            ApplicationState.Instance.Connection.Close();
            ApplicationState.Instance.ApplicationEvents.OnGameStopped -= GameForm_OnClosed;
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
        }
    }
}
