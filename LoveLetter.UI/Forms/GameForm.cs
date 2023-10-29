using LoveLetter.Core.Constants;
using LoveLetter.Core.Entities;
using LoveLetter.Core.Exceptions;
using LoveLetter.Core.Utils;
using LoveLetter.UI.Infrastructure;
using Microsoft.VisualBasic;
using Newtonsoft.Json.Linq;
using System.ComponentModel;
using System.Data;
using System.Numerics;
using System.Text.Json;
using System.Threading;

namespace LoveLetter.UI.Forms
{
    public partial class GameForm : Form
    {
        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private Thread? _updateGridViewThread = null;
        public Card InitialCard { get; set; } = new Card();

        public Card? AdditionalCard { get; private set; }

        public Card SelectedCard { get; private set; }

        public Deck CurrentDeck { get; private set; } = new Deck();

        public GameForm()
        {
            InitializeComponent();
            ApplicationState.Instance.CardEvents = new CardEvents();
            ApplicationState.Instance.CardEvents.OnGuard += CardEvents_OnGuard;
            ApplicationState.Instance.CardEvents.OnPriest += CardEvents_OnPriest;
            ApplicationState.Instance.CardEvents.OnBaron += CardEvents_OnBaron;
            ApplicationState.Instance.CardEvents.OnHandmaid += CardEvents_OnHandmaid;
            ApplicationState.Instance.CardEvents.OnPrince += CardEvents_OnPrince;
            ApplicationState.Instance.CardEvents.OnKing += CardEvents_OnKing;
            ApplicationState.Instance.CardEvents.OnCountess += CardEvents_OnCountess;
            ApplicationState.Instance.CardEvents.OnPrincess += CardEvents_OnPrincess;
            ApplicationState.Instance.ApplicationEvents.OnGameStopped += ApplicationEvents_OnGameStopped;
            SelectedCard = InitialCard;
        }

        private void ApplicationEvents_OnGameStopped(object? sender, EventArgs e)
        {
            Close();
        }

        private void EndTurnBtn_Click(object sender, EventArgs e)
        {
            try
            {
                var cardEvents = ApplicationState.Instance.CardEvents;
                var gameState = ApplicationState.Instance.CurrentGameState;
                var player = ApplicationState.Instance.CurrentPlayer;

                if (cardEvents is null)
                {
                    throw new NullReferenceException(nameof(cardEvents));
                }

                if (gameState is null)
                {
                    throw new NullReferenceException(nameof(gameState));
                }

                if (player is null)
                {
                    throw new NullReferenceException(nameof(player));
                }

                var args = new CardEventArgs((short)PlayerNumberValue.Value, (short)PlayerValueValue.Value);
                SelectedCard.Effect(cardEvents, args);
                AuditItem.Append(
                    gameState.Id, player, 
                    string.Join(" ", SelectedCard.CardType.ToString(), nameof(Card.Effect)),
                    ApplicationState.Instance.Connection);
            }
            catch (Exception ex)
            {
                this.ThrowError(ex);
            }
        }

        private void GameForm_Load(object sender, EventArgs e)
        {
            try
            {
                var gameState = ApplicationState.Instance.CurrentGameState;
                var player = ApplicationState.Instance.CurrentPlayer;

                if (gameState is null)
                {
                    throw new NullReferenceException(nameof(gameState));
                }

                if (player is null)
                {
                    throw new NullReferenceException(nameof(player));
                }

                _updateGridViewThread = new Thread(new ParameterizedThreadStart(TrackAudit));
                _updateGridViewThread.Start(new { _cancellationTokenSource.Token, GameStateId = gameState.Id });

                PlayerNumberValue.Maximum = Constraints.MAX_PLAYER_NUMBER;
                PlayerValueValue.Maximum = Enum.GetValues(typeof(CardType)).Length;

                YourNicknameValue.Text = player.NickName;
                YourPlayerNumberValue.Text = player.PlayerNumber.ToString();
                TurnPlayerNumberValue.Text = gameState.TurnPlayerNumber.ToString();

                InitialCard = gameState.TakeCard();

                if (gameState.InTurn(player.PlayerNumber))
                {
                   AdditionalCard = gameState.TakeCard();
                }
            }
            catch (Exception ex)
            {
                this.ThrowError(ex);
            }
        }

        private void TrackAudit(object? args = default)
        {
            if (args is not null)
            {
                if ((JObject.FromObject(args)["Token"]?.ToObject<CancellationToken>() ?? null) is CancellationToken token 
                    && token.IsCancellationRequested)
                {
                    return;
                }

                if (Guid.TryParse(JObject.FromObject(args)["GameStateId"]?.ToString() ?? null, out var gameStateId))
                {
                    SetDataSourceInGridView(DataGridUtils.LoadAudit(gameStateId, ApplicationState.Instance.Connection));
                }
            }
        }

        private void SetDataSourceInGridView(DataTable table)
        {
            if (AuditGrid.InvokeRequired)
            {
                Invoke(SetDataSourceInGridView, new object[] { table });
            }
            else
            {
                AuditGrid.DataSource = table;
                AuditGrid.Update();
                AuditGrid.Refresh();
            }
        }

        private void PollingTimer_Tick(object sender, EventArgs e)
        {
            PollingTimer.Stop();

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

                ApplicationState.Instance.CurrentGameState = GameState.Fetch(lobby.Id, ApplicationState.Instance.Connection);
                var gameState = ApplicationState.Instance.CurrentGameState;
                player = gameState.Players.FirstOrDefault(p => p.NickName == player.NickName);

                if (player is null)
                {
                    throw new NullReferenceException(nameof(player));
                }

                if (gameState.WinnerPlayerNumber is not null)
                {
                    lobby.Close();
                    this.SendResultMessage(gameState.WinnerPlayerNumber.Value);

                    if (ApplicationState.Instance.ApplicationEvents is not null)
                    {
                        ApplicationState.Instance.ApplicationEvents.OnGameStoppedHandler(e);
                    }
                }

                if (!gameState.Players.Any(p => p.PlayerNumber == player.PlayerNumber))
                {
                    lobby.Leave(player.NickName);
                    this.LoseMessage();

                    if (ApplicationState.Instance.ApplicationEvents is not null)
                    {
                        ApplicationState.Instance.ApplicationEvents.OnGameStoppedHandler(e);
                    }
                }

                TurnPlayerNumberValue.Text = gameState.TurnPlayerNumber.ToString();

                if (gameState.InTurn(player.PlayerNumber))
                {
                    AdditionalCard = gameState.TakeCard();
                }
                else
                {
                    PollingTimer.Start();
                }
            }
            catch (Exception ex)
            {
                this.ThrowError(ex);
            }
        }

        private void InitialCardPicture_Click(object sender, EventArgs e)
        {
            // Hover picture

            if (AdditionalCard is not null)
            {
                if (IsCountessWithKingOrPrince(AdditionalCard, InitialCard))
                {
                    SelectedCard = new Card(AdditionalCard);
                    this.AlertMessage("Countess is your selected card because of Prince or King in your hand");
                    return;
                }

                SelectedCard = new Card(InitialCard);
                InitialCard = new Card(AdditionalCard);
            }
        }

        private void AdditionalCardPicture_Click(object sender, EventArgs e)
        {
            // Hover picture
            if (AdditionalCard is not null)
            {
                if (IsCountessWithKingOrPrince(InitialCard, AdditionalCard))
                {
                    SelectedCard = new Card(InitialCard);
                    InitialCard = new Card(AdditionalCard);
                    this.AlertMessage("Countess is your selected card because of Prince or King in your hand");
                    return;
                }

                SelectedCard = new Card(AdditionalCard);
            }
        }

        private bool IsCountessWithKingOrPrince(Card firstCard, Card secondCard) =>
            firstCard.CardType == CardType.Countess && 
            (secondCard.CardType == CardType.Prince || secondCard.CardType == CardType.King);

        private void CardEvents_OnPrincess(object sender, CardEventArgs e)
        {
            try
            {
                var gameState = ApplicationState.Instance.CurrentGameState;
                var player = ApplicationState.Instance.CurrentPlayer;

                if (gameState is null)
                {
                    throw new NullReferenceException(nameof(gameState));
                }

                if (player is null)
                {
                    throw new NullReferenceException(nameof(player));
                }

                gameState.Lose(player);
                gameState.EndTurn(InitialCard);
            }
            catch (Exception ex)
            {
                this.ThrowError(ex);
            }
        }

        private void CardEvents_OnCountess(object sender, CardEventArgs e)
        {
            try
            {
                var gameState = ApplicationState.Instance.CurrentGameState;
                var player = ApplicationState.Instance.CurrentPlayer;

                if (gameState is null)
                {
                    throw new NullReferenceException(nameof(gameState));
                }

                if (player is null)
                {
                    throw new NullReferenceException(nameof(player));
                }

                gameState.EndTurn(InitialCard);
            }
            catch (Exception ex)
            {
                this.ThrowError(ex);
            }
        }

        private void CardEvents_OnKing(object sender, CardEventArgs e)
        {
            try
            {
                var gameState = ApplicationState.Instance.CurrentGameState;
                var player = ApplicationState.Instance.CurrentPlayer;

                if (gameState is null)
                {
                    throw new NullReferenceException(nameof(gameState));
                }

                if (player is null)
                {
                    throw new NullReferenceException(nameof(player));
                }

                var opponent = gameState.Players.FirstOrDefault(p => p.PlayerNumber == e.PlayerNumber);

                if (opponent is not null)
                {
                    opponent = FindOpponent(gameState, player, opponent);

                    if (opponent is null)
                    {
                        gameState.EndTurn(InitialCard);
                        return;
                    }

                    AuditItem.Append(gameState.Id, player, 
                        string.Join(" ", nameof(FindOpponent), opponent.PlayerNumber),
                        ApplicationState.Instance.Connection);

                    gameState.SwapCards(player.PlayerNumber, opponent);
                }

                gameState.EndTurn(InitialCard);
            }
            catch (Exception ex)
            {
                this.ThrowError(ex);
            }
        }

        private void CardEvents_OnPrince(object sender, CardEventArgs e)
        {
            try
            {
                var gameState = ApplicationState.Instance.CurrentGameState;
                var player = ApplicationState.Instance.CurrentPlayer;

                if (gameState is null)
                {
                    throw new NullReferenceException(nameof(gameState));
                }

                if (player is null)
                {
                    throw new NullReferenceException(nameof(player));
                }

                var opponent = gameState.Players.FirstOrDefault(p => p.PlayerNumber == e.PlayerNumber);

                if (opponent is not null)
                {
                    opponent = FindOpponent(gameState, player, opponent);

                    if (opponent is null)
                    {
                        gameState.EndTurn(InitialCard);
                        return;
                    }

                    AuditItem.Append(
                        gameState.Id, player, 
                        string.Join(" ", nameof(FindOpponent), opponent.PlayerNumber),
                        ApplicationState.Instance.Connection);

                    if (opponent.CurrentCard == CardType.Princess)
                    {
                        gameState.Lose(opponent);
                    }
                }

                gameState.EndTurn(InitialCard);
            }
            catch (Exception ex)
            {
                this.ThrowError(ex);
            }
        }

        private void CardEvents_OnHandmaid(object sender, CardEventArgs e)
        {
            try
            {
                var gameState = ApplicationState.Instance.CurrentGameState;
                var player = ApplicationState.Instance.CurrentPlayer;

                if (gameState is null)
                {
                    throw new NullReferenceException(nameof(gameState));
                }

                if (player is null)
                {
                    throw new NullReferenceException(nameof(player));
                }

                var playerToUpdate = gameState.Players.FirstOrDefault(p => p.PlayerNumber == player.PlayerNumber);

                if (playerToUpdate is not null)
                {
                    playerToUpdate.Available = false;
                    gameState.Save((nameof(GameState.Players), gameState.Players));
                }

                gameState.EndTurn(InitialCard);
            }
            catch (Exception ex)
            {
                this.ThrowError(ex);
            }
        }

        private void CardEvents_OnBaron(object sender, CardEventArgs e)
        {
            try
            {
                var gameState = ApplicationState.Instance.CurrentGameState;
                var player = ApplicationState.Instance.CurrentPlayer;
                var lobby = ApplicationState.Instance.CurrentLobby;

                if (lobby is null)
                {
                    throw new NullReferenceException(nameof(lobby));
                }

                if (gameState is null)
                {
                    throw new NullReferenceException(nameof(gameState));
                }

                if (player is null)
                {
                    throw new NullReferenceException(nameof(player));
                }

                var opponent = gameState.Players.FirstOrDefault(p => p.PlayerNumber == e.PlayerNumber);

                if (opponent is not null)
                {
                    opponent = FindOpponent(gameState, player, opponent);

                    if (opponent is null)
                    {
                        gameState.EndTurn(InitialCard);
                        return;
                    }

                    AuditItem.Append(
                        gameState.Id, player, 
                        string.Join(" ", nameof(FindOpponent), opponent.PlayerNumber), 
                        ApplicationState.Instance.Connection);

                    if (InitialCard > opponent.CurrentCard)
                    {
                        gameState.Lose(opponent);
                    }
                    else if (InitialCard < opponent.CurrentCard)
                    {
                        gameState.Lose(player);
                        lobby.Leave(player.NickName);
                    }
                }

                gameState.EndTurn(InitialCard);
            }
            catch (Exception ex)
            {
                this.ThrowError(ex);
            }
        }

        private void CardEvents_OnPriest(object sender, CardEventArgs e)
        {
            try
            {
                var gameState = ApplicationState.Instance.CurrentGameState;
                var player = ApplicationState.Instance.CurrentPlayer;

                if (gameState is null)
                {
                    throw new NullReferenceException(nameof(gameState));
                }

                if (player is null)
                {
                    throw new NullReferenceException(nameof(player));
                }

                var opponent = gameState.Players.FirstOrDefault(p => p.PlayerNumber == e.PlayerNumber);

                if (opponent is not null)
                {
                    opponent = FindOpponent(gameState, player, opponent);

                    if (opponent is null)
                    {
                        gameState.EndTurn(InitialCard);
                        return;
                    }

                    AuditItem.Append(
                        gameState.Id, player, 
                        string.Join(" ", nameof(FindOpponent), opponent.PlayerNumber),
                        ApplicationState.Instance.Connection);

                    this.AlertMessage($"You revealed opponents`s card. His card type is {opponent.CurrentCard}");
                }

                gameState.EndTurn(InitialCard);

            }
            catch (Exception ex)
            {
                this.ThrowError(ex);
            }
        }

        private void CardEvents_OnGuard(object? sender, CardEventArgs e)
        {
            try
            {
                var gameState = ApplicationState.Instance.CurrentGameState;
                var player = ApplicationState.Instance.CurrentPlayer;

                if (gameState is null)
                {
                    throw new NullReferenceException(nameof(gameState));
                }

                if (player is null)
                {
                    throw new NullReferenceException(nameof(player));
                }

                var opponent = gameState.Players.FirstOrDefault(p => p.PlayerNumber == e.PlayerNumber);

                if (opponent is not null)
                {
                    opponent = FindOpponent(gameState, player, opponent);

                    if (opponent is null)
                    {
                        gameState.EndTurn(InitialCard);
                        return;
                    }

                    AuditItem.Append(
                        gameState.Id, player, 
                        string.Join(" ", nameof(FindOpponent), opponent.PlayerNumber, e.CardType), 
                        ApplicationState.Instance.Connection);

                    if (e.CardType == (short)opponent.CurrentCard)
                    {
                        gameState.Win(player.PlayerNumber);
                        this.CongratulationMessage();

                        if (ApplicationState.Instance.ApplicationEvents is not null)
                        {
                            ApplicationState.Instance.ApplicationEvents.OnGameStoppedHandler(e);
                        }
                    }
                    else
                    {
                        gameState.EndTurn(InitialCard);
                    }
                }
            }
            catch (Exception ex)
            {
                this.ThrowError(ex);
            }
        }

        private Player? FindOpponent(GameState gameState, Player player, Player opponent)
        {
            while (!opponent.Available)
            {
                if (gameState.Players.Any(p => p.Available && p.PlayerNumber != player.PlayerNumber))
                {
                    return null;
                }

                gameState.ResetTarget(player, opponent);
            }

            return opponent;
        }

        private void GameForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (ApplicationState.Instance.ApplicationEvents is not null)
            {
                ApplicationState.Instance.ApplicationEvents.OnGameStoppedHandler(e);
            }
        }

        private void GameForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (ApplicationState.Instance.CardEvents is not null)
            {
                ApplicationState.Instance.CardEvents.OnGuard -= CardEvents_OnGuard;
                ApplicationState.Instance.CardEvents.OnPriest -= CardEvents_OnPriest;
                ApplicationState.Instance.CardEvents.OnBaron -= CardEvents_OnBaron;
                ApplicationState.Instance.CardEvents.OnHandmaid -= CardEvents_OnHandmaid;
                ApplicationState.Instance.CardEvents.OnPrince -= CardEvents_OnPrince;
                ApplicationState.Instance.CardEvents.OnKing -= CardEvents_OnKing;
                ApplicationState.Instance.CardEvents.OnCountess -= CardEvents_OnCountess;
                ApplicationState.Instance.CardEvents.OnPrincess -= CardEvents_OnPrincess;
            }

            ApplicationState.Instance.CardEvents = null;

            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
        }

        private void AfkTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                var gameState = ApplicationState.Instance.CurrentGameState;
                var player = ApplicationState.Instance.CurrentPlayer;
                var lobby = ApplicationState.Instance.CurrentLobby;

                if (lobby is null)
                {
                    throw new NullReferenceException(nameof(lobby));
                }

                if (gameState is null)
                {
                    throw new NullReferenceException(nameof(gameState));
                }

                if (player is null)
                {
                    throw new NullReferenceException(nameof(player));
                }

                gameState.Lose(player);
                lobby.Leave(player.NickName);
                this.AlertMessage($"You are kicked for being afk for {TimeSpan.FromMilliseconds(AfkTimer.Interval).Minutes} minutes!");

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

        private void PlayerNumberValue_ValueChanged(object sender, EventArgs e)
        {
            var gameState = ApplicationState.Instance.CurrentGameState;

            if (gameState is null)
            {
                throw new NullReferenceException(nameof(gameState));
            }

            var opponents = gameState.Players.Where(p => p.Available);

            if (!opponents.Any(o => o.PlayerNumber == (short)PlayerNumberValue.Value))
            {
                this.AlertMessage("You cannot choose this player because he used " + CardType.Handmaid);
                PlayerNumberValue.Value = PlayerNumberValue.Minimum;
            }
        }
    }
}
