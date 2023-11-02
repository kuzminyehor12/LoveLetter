using LoveLetter.Core.Constants;
using LoveLetter.Core.Entities;
using LoveLetter.Core.Utils;
using LoveLetter.UI.Infrastructure;
using System.Data;

namespace LoveLetter.UI.Forms
{
    public partial class GameForm : Form
    {
        private const int BORDER_WIDTH = 5;
        private bool _toggleBorders = false;

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
            ApplicationState.Instance.ApplicationEvents.OnGameStopped += ApplicationEvents_GameForm_OnGameStopped;

            InitialCardPicture.Paint += InitialCardPicture_Paint;
            AdditionalCardPicture.Paint += AdditionalCardPicture_Paint;
            SelectedCard = InitialCard;
        }

        private void AdditionalCardPicture_Paint(object? sender, PaintEventArgs e)
        {
            if (sender is PictureBox pictureBox)
            {
                var borderColor = _toggleBorders ? Color.Green : Color.Transparent;

                if (PollingTimer.Enabled)
                {
                    borderColor = Color.Transparent;
                }

                Pen borderPen = new Pen(borderColor, BORDER_WIDTH);
                Rectangle borderRect = pictureBox.ClientRectangle;
                borderRect.Inflate(-1, -1);
                e.Graphics.DrawRectangle(borderPen, borderRect);
                borderPen.Dispose();
            }
        }

        private void InitialCardPicture_Paint(object? sender, PaintEventArgs e)
        {
            if (sender is PictureBox pictureBox)
            {
                var borderColor = !_toggleBorders ? Color.Green : Color.Transparent;

                if (PollingTimer.Enabled)
                {
                    borderColor = Color.Transparent;
                }
               
                Pen borderPen = new Pen(borderColor, BORDER_WIDTH);
                Rectangle borderRect = pictureBox.ClientRectangle;
                borderRect.Inflate(-1, -1);
                e.Graphics.DrawRectangle(borderPen, borderRect);
                borderPen.Dispose();
            }
        }

        private void ApplicationEvents_GameForm_OnGameStopped(object? sender, EventArgs e)
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

                if (AdditionalCard is not null)
                {
                    if (SelectedCard == AdditionalCard)
                    {
                        AdditionalCard = null;
                    }
                    else
                    {
                        InitialCard = new Card(AdditionalCard);
                        InitialCardPicture.Image = AdditionalCardPicture.Image;
                    }

                    AdditionalCardPicture.Image = null;
                    AdditionalCardPicture.Invalidate();
                    InitialCardPicture.Invalidate();
                }

                gameState.PopulateCardHistory(SelectedCard);
                PollingTimer.Enabled = true;
                EndTurnBtn.Enabled = false;

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

                Image img = DeckPicture.Image;
                img.RotateFlip(RotateFlipType.Rotate90FlipNone);
                DeckPicture.Image = img;
                DeckPicture.SizeMode = PictureBoxSizeMode.Zoom;

                PlayerNumberValue.Maximum = gameState.Players.Count;
                PlayerValueValue.Maximum = Enum.GetValues(typeof(CardType)).Length;

                YourNicknameValue.Text = player.Nickname;
                YourPlayerNumberValue.Text = player.PlayerNumber.ToString();
                TurnPlayerNumberValue.Text = gameState.TurnPlayerNumber.ToString();

                CardsCount.Text = Constraints.INITIAL_CARDS_COUNT.ToString();

                InitialCard = gameState.TakeCard(player);
                InitialCardPicture.Image = ImageUtils.GetImage(InitialCard);

                SelectedCard = new Card(InitialCard);

                if (gameState.InTurn(player.PlayerNumber))
                {
                   AdditionalCard = gameState.TakeCard(player);
                   AdditionalCardPicture.Image = ImageUtils.GetImage(AdditionalCard);
                }
                else
                {
                    EndTurnBtn.Enabled = false;
                    PollingTimer.Enabled = true;
                }

                CardsCount.Text = (Constraints.INITIAL_CARDS_COUNT - gameState.CardHistory.Cards.Count() - 3).ToString();
                RefreshCardHistory(gameState);
                AuditGrid.DataSource = DataGridUtils.LoadAudit(gameState.Id, ApplicationState.Instance.Connection);
            }
            catch (Exception ex)
            {
                this.ThrowError(ex);
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
                player = gameState.Players.FirstOrDefault(p => p.Nickname == player.Nickname);

                if (player is null)
                {
                    throw new NullReferenceException(nameof(player));
                }

                AuditGrid.DataSource = DataGridUtils.LoadAudit(gameState.Id, ApplicationState.Instance.Connection);
                AuditGrid.Update();
                AuditGrid.Refresh();

                if (gameState.WinnerPlayerNumber is not null)
                {
                    lobby.Close();
                    this.SendResultMessage(gameState.WinnerPlayerNumber.Value);

                    if (ApplicationState.Instance.ApplicationEvents is not null)
                    {
                        ApplicationState.Instance.ApplicationEvents.OnGameStoppedHandler(e);
                    }
                }

                if (gameState.Players.Count == 1)
                {
                    lobby.Close();
                    this.SendResultMessage(gameState.Players.First().PlayerNumber);

                    if (ApplicationState.Instance.ApplicationEvents is not null)
                    {
                        ApplicationState.Instance.ApplicationEvents.OnGameStoppedHandler(e);
                    }
                }

                if (!gameState.Players.Any(p => p.PlayerNumber == player.PlayerNumber))
                {
                    lobby.Leave(player.Nickname);
                    this.LoseMessage();

                    if (ApplicationState.Instance.ApplicationEvents is not null)
                    {
                        ApplicationState.Instance.ApplicationEvents.OnGameStoppedHandler(e);
                    }
                }

                PlayerNumberValue.Maximum = gameState.Players.Count;
                TurnPlayerNumberValue.Text = gameState.TurnPlayerNumber.ToString();

                if (gameState.InTurn(player.PlayerNumber))
                {
                    AdditionalCard = gameState.TakeCard(player);
                    AdditionalCardPicture.Image = ImageUtils.GetImage(AdditionalCard);
                    PollingTimer.Enabled = false;
                    EndTurnBtn.Enabled = true;
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
            if (AdditionalCard is not null)
            {
                if (IsCountessWithKingOrPrince(AdditionalCard, InitialCard))
                {
                    SelectedCard = new Card(AdditionalCard);
                    this.AlertMessage($"{CardType.Countess} is your selected card because of {CardType.Prince} or {CardType.King} in your hand");
                    return;
                }

                SelectedCard = new Card(InitialCard);

                _toggleBorders = false;
                InitialCardPicture.Invalidate();
                AdditionalCardPicture.Invalidate();
            }
        }

        private void AdditionalCardPicture_Click(object sender, EventArgs e)    
        {
            if (AdditionalCard is not null)
            {
                if (IsCountessWithKingOrPrince(InitialCard, AdditionalCard))
                {
                    SelectedCard = new Card(InitialCard);
                    this.AlertMessage($"{CardType.Countess} is your selected card because of {CardType.Prince} or {CardType.King} in your hand");
                    return;
                }

                SelectedCard = new Card(AdditionalCard);

                _toggleBorders = true;
                InitialCardPicture.Invalidate();
                AdditionalCardPicture.Invalidate();
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

                if (AdditionalCard is not null && SelectedCard == InitialCard)
                {
                    gameState.EndTurn(AdditionalCard);
                }
                else
                {
                    gameState.EndTurn(InitialCard);
                }
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

                if (AdditionalCard is not null && SelectedCard == InitialCard)
                {
                    gameState.EndTurn(AdditionalCard);
                }
                else
                {
                    gameState.EndTurn(InitialCard);
                }
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
                        string.Join(" ", nameof(FindOpponent), "Picked player: " + opponent.Nickname),
                        ApplicationState.Instance.Connection);

                    gameState.SwapCards(player.PlayerNumber, opponent);
                }

                if (AdditionalCard is not null && SelectedCard == InitialCard)
                {
                    gameState.EndTurn(AdditionalCard);
                }
                else
                {
                    gameState.EndTurn(InitialCard);
                }
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
                     string.Join(" ", nameof(FindOpponent), "Picked player: " + opponent.Nickname),
                     ApplicationState.Instance.Connection);

                    if (opponent.CurrentCard == CardType.Princess)
                    {
                        gameState.Lose(opponent);
                    }
                }

                if (AdditionalCard is not null && SelectedCard == InitialCard)
                {
                    gameState.EndTurn(AdditionalCard);
                }
                else
                {
                    gameState.EndTurn(InitialCard);
                }
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

                if (AdditionalCard is not null && SelectedCard == InitialCard)
                {
                    gameState.EndTurn(AdditionalCard);
                }
                else
                {
                    gameState.EndTurn(InitialCard);
                }
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
                      string.Join(" ", nameof(FindOpponent), "Picked player: " + opponent.Nickname),
                      ApplicationState.Instance.Connection);

                    if (InitialCard > opponent.CurrentCard)
                    {
                        gameState.Lose(opponent);
                    }
                    else if (InitialCard < opponent.CurrentCard)
                    {
                        gameState.Lose(player);
                        lobby.Leave(player.Nickname);
                    }
                }

                if (AdditionalCard is not null && SelectedCard == InitialCard)
                {
                    gameState.EndTurn(AdditionalCard);
                }
                else
                {
                    gameState.EndTurn(InitialCard);
                }
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
                      string.Join(" ", nameof(FindOpponent), "Picked player: " + opponent.Nickname),
                      ApplicationState.Instance.Connection);

                    this.AlertMessage($"You revealed opponents`s card. His card type is {opponent.CurrentCard}");
                }

                if (AdditionalCard is not null && SelectedCard == InitialCard)
                {
                    gameState.EndTurn(AdditionalCard);
                }
                else
                {
                    gameState.EndTurn(InitialCard);
                }

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
                    if (opponent.PlayerNumber == player.PlayerNumber)
                    {
                        this.ThrowIssue("You cannot choose yourself when playing " + CardType.Guard.ToString());
                        return;
                    }

                    opponent = FindOpponent(gameState, player, opponent);

                    if (opponent is null)
                    {
                        gameState.EndTurn(InitialCard);
                        return;
                    }

                    AuditItem.Append(
                        gameState.Id, player, 
                        string.Join(" ", nameof(FindOpponent), "Picked player: " + opponent.Nickname, "Picked card: " + e.CardType), 
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
                        if (AdditionalCard is not null && SelectedCard == InitialCard)
                        {
                            gameState.EndTurn(AdditionalCard);
                        }
                        else
                        {
                            gameState.EndTurn(InitialCard);
                        }
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
                if (!gameState.Players.Any(p => p.Available && p.PlayerNumber != player.PlayerNumber))
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

            ApplicationState.Instance.CurrentGameState = null;
            ApplicationState.Instance.CurrentLobby = null;
            ApplicationState.Instance.CardEvents = null;
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
                lobby.Leave(player.Nickname);
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
            try
            {
                var gameState = ApplicationState.Instance.CurrentGameState;

                if (gameState is null)
                {
                    throw new NullReferenceException(nameof(gameState));
                }

                var opponents = gameState.Players.Where(p => p.Available);

                if (!opponents.Any(o => o.PlayerNumber == (short)PlayerNumberValue.Value))
                {
                    this.AlertMessage("You cannot choose this player because he used " + CardType.Handmaid.ToString());
                    PlayerNumberValue.Value = PlayerNumberValue.Minimum;
                }
            }
            catch (Exception ex)
            {
                this.ThrowError(ex);
            }
        }

        private void RefreshIcon_Click(object sender, EventArgs e)
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

                AuditGrid.DataSource = DataGridUtils.LoadAudit(gameState.Id, ApplicationState.Instance.Connection);
                AuditGrid.Update();
                AuditGrid.Refresh();

                RefreshCardHistory(gameState);

                CardsCount.Text = (Constraints.INITIAL_CARDS_COUNT - gameState.CardHistory.Cards.Count() - 3).ToString();
            }
            catch (Exception ex)
            {
                this.ThrowError(ex);
            }
        }

        private void RefreshCardHistory(GameState gameState)
        {
            CardHistoryListBox.Items.Clear();

            foreach (var card in gameState.CardHistory.Cards)
            {
                CardHistoryListBox.Items.Add($"{gameState.CardHistory.Cards.ToList().IndexOf(card) + 1}: {card.CardType} was played");
            }
        }
    }
}
