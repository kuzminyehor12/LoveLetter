using LoveLetter.Core.Constants;
using LoveLetter.Core.Entities;
using LoveLetter.Core.Exceptions;
using LoveLetter.Core.Utils;
using LoveLetter.UI.Infrastructure;
using System.ComponentModel;
using System.Data;
using System.Numerics;

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

                gameState = GameState.Fetch(gameState.Id, ApplicationState.Instance.Connection);
                var args = new CardEventArgs((short)PlayerNumberValue.Value, (short)PlayerValueValue.Value);

                if (AdditionalCard is not null)
                {
                    if (InitialCard != SelectedCard && IsCountessWithKingOrPrince(InitialCard, SelectedCard))
                    {
                        SelectedCard = InitialCard;
                    }

                    if (AdditionalCard != SelectedCard && IsCountessWithKingOrPrince(AdditionalCard, SelectedCard))
                    {
                        SelectedCard = AdditionalCard;
                    }
                }

                SelectedCard.Effect(cardEvents, args);

                if (AdditionalCard is not null)
                {
                    if (SelectedCard == InitialCard)
                    {
                        InitialCard = new Card(AdditionalCard);
                        InitialCardPicture.Image = AdditionalCardPicture.Image;
                    }

                    AdditionalCard = null;

                    gameState = GameState.Fetch(gameState.Id, ApplicationState.Instance.Connection);
                    var playerToUpdate = gameState.Players.FirstOrDefault(p => p.PlayerNumber == player.PlayerNumber);

                    if (playerToUpdate is not null)
                    {
                        playerToUpdate.CurrentCard = InitialCard;
                        gameState.Save((nameof(GameState.Players), gameState.Players));
                    }

                    AdditionalCardPicture.Image = null;
                    AdditionalCardPicture.Invalidate();
                    InitialCardPicture.Invalidate();
                }

                gameState.PopulateCardHistory(SelectedCard);

                gameState = GameState.Fetch(gameState.Id, ApplicationState.Instance.Connection);

                PollingTimer.Enabled = true;
                EndTurnBtn.Enabled = false;

                AuditItem.Append(
                    gameState.Id, player,
                    string.Join(" ", SelectedCard.CardType.ToString(), nameof(Card.Effect)),
                    ApplicationState.Instance.Connection);
            }
            catch (NotExistingEntityException)
            {
                return;
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

                CardsCount.Text = GetCardsCount(gameState).ToString();
                RefreshCardHistory(gameState);
                AuditGrid.DataSource = DataGridUtils.LoadAudit(gameState.Id, ApplicationState.Instance.Connection);
            }
            catch (Exception ex)
            {
                this.ThrowError(ex);
            }
        }

        private int GetCardsCount(GameState gameState)
        {
            var princesCount = gameState.CardHistory.Cards.Count(c => c == CardType.Prince);
            return Constraints.INITIAL_CARDS_COUNT - gameState.CardHistory.Cards.Count() - princesCount - gameState.Players.Count - 1;
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
                    Close();
                    return;
                }

                if (player is null)
                {
                    throw new NullReferenceException(nameof(player));
                }

                ApplicationState.Instance.CurrentGameState = GameState.Fetch(lobby.Id, ApplicationState.Instance.Connection);
                var gameState = ApplicationState.Instance.CurrentGameState;

                if (gameState is null)
                {
                    PollingTimer.Start();
                    return;
                }

                if (gameState.Players.Count == 1)
                {
                    gameState.Win(gameState.Players.First().PlayerNumber);
                    lobby.Close();
                    this.SendResultMessage(gameState.Players.First().Nickname);
                    Close();
                    return;
                }

                if (gameState.WinnerPlayerNumber is not null)
                {
                    var winner = gameState.Players.First(p => p.PlayerNumber == gameState.WinnerPlayerNumber.Value);
                    lobby.Close();
                    this.SendResultMessage(winner.Nickname);
                    Close();
                    return;
                }

                if (!gameState.Players.Any(p => p.Nickname == player.Nickname))
                {
                    lobby.Leave(player.Nickname);
                    this.LoseMessage();
                    Close();
                    return;
                }

                player = gameState.Players.FirstOrDefault(p => p.Nickname == player.Nickname);

                if (player is null)
                {
                    throw new NullReferenceException(nameof(player));
                }

                if (IsGameFinished(gameState, player))
                {
                    var winner = gameState.Players.First(p => p.CurrentCard == gameState.Players.Max(pl => pl.CurrentCard));
                    lobby.Close();
                    gameState.Win(winner.PlayerNumber);
                    this.SendResultMessage(winner.Nickname);
                    Close();
                }

                AuditGrid.DataSource = DataGridUtils.LoadAudit(gameState.Id, ApplicationState.Instance.Connection);
                AuditGrid.Update();
                AuditGrid.Refresh();

                PlayerNumberValue.Maximum = gameState.Players.Count;
                TurnPlayerNumberValue.Text = gameState.TurnPlayerNumber.ToString();
                CardsCount.Text = GetCardsCount(gameState).ToString();

                InitialCard = new Card(player.CurrentCard);
                InitialCardPicture.Image = ImageUtils.GetImage(InitialCard);

                if (gameState.InTurn(player.PlayerNumber))
                {
                    AdditionalCard = gameState.TakeCard(player);
                    AdditionalCardPicture.Image = ImageUtils.GetImage(AdditionalCard);
                    SelectedCard = _toggleBorders ? new Card(AdditionalCard) : new Card(InitialCard);
                    PollingTimer.Enabled = false;
                    EndTurnBtn.Enabled = true;
                }
                else
                {
                    PollingTimer.Start();
                }

                gameState = GameState.Fetch(gameState.Id, ApplicationState.Instance.Connection);
                RefreshCardHistory(gameState);
            }
            catch (NotExistingEntityException)
            {
                PollingTimer.Start();
                return;
            }
            catch (Exception ex)
            {
                this.ThrowError(ex);
            }

            bool IsGameFinished(GameState gameState, Player player) =>
                gameState.Deck.IsEmpty && AdditionalCard is null && gameState.InTurn(player.PlayerNumber);
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

                gameState = GameState.Fetch(gameState.Id, ApplicationState.Instance.Connection);
                var opponent = gameState.Players.FirstOrDefault(p => p.PlayerNumber == e.PlayerNumber);

                Card swappedCard = new Card();
                if (opponent is not null)
                {
                    opponent = FindOpponent(gameState, player, opponent);

                    if (opponent is null)
                    {
                        if (AdditionalCard is not null && SelectedCard == InitialCard)
                        {
                            gameState.EndTurn(AdditionalCard);
                        }
                        else
                        {
                            gameState.EndTurn(InitialCard);
                        }

                        return;
                    }

                    AuditItem.Append(gameState.Id, player, 
                        string.Join(" ", nameof(FindOpponent), "Picked player: " + opponent.Nickname),
                        ApplicationState.Instance.Connection);

                    swappedCard = gameState.SwapCards(player.PlayerNumber, opponent);
                }

                gameState = GameState.Fetch(gameState.Id, ApplicationState.Instance.Connection);
                if (AdditionalCard is not null && SelectedCard == InitialCard)
                {
                    AdditionalCard = new Card(swappedCard);
                    gameState.EndTurn(AdditionalCard);
                }
                else
                {
                    InitialCard = new Card(swappedCard);
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
                var lobby = ApplicationState.Instance.CurrentLobby;

                if (lobby is null)
                {
                    Close();
                    return;
                }

                if (gameState is null)
                {
                    throw new NullReferenceException(nameof(gameState));
                }

                if (player is null)
                {
                    throw new NullReferenceException(nameof(player));
                }

                gameState = GameState.Fetch(gameState.Id, ApplicationState.Instance.Connection);
                var opponent = gameState.Players.FirstOrDefault(p => p.PlayerNumber == e.PlayerNumber);

                if (opponent is not null)
                {
                    opponent = FindOpponent(gameState, player, opponent);

                    if (opponent is null)
                    {
                        if (AdditionalCard is not null && SelectedCard == InitialCard)
                        {
                            gameState.EndTurn(AdditionalCard);
                        }
                        else
                        {
                            gameState.EndTurn(InitialCard);
                        }

                        return;
                    }

                    AuditItem.Append(
                     gameState.Id, player,
                     string.Join(" ", nameof(FindOpponent), "Picked player: " + opponent.Nickname),
                     ApplicationState.Instance.Connection);

                    if (opponent.CurrentCard == CardType.Princess)
                    {
                        gameState.Lose(opponent);
                        lobby.Leave(opponent.Nickname);
                    }
                    else
                    {
                        opponent.CurrentCard = gameState.TakeCard(opponent);
                    }
                }

                gameState = GameState.Fetch(gameState.Id, ApplicationState.Instance.Connection);
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

                gameState = GameState.Fetch(gameState.Id, ApplicationState.Instance.Connection);
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
                    Close();
                    return;
                }

                if (gameState is null)
                {
                    throw new NullReferenceException(nameof(gameState));
                }

                if (player is null)
                {
                    throw new NullReferenceException(nameof(player));
                }

                gameState = GameState.Fetch(gameState.Id, ApplicationState.Instance.Connection);
                var opponent = gameState.Players.FirstOrDefault(p => p.PlayerNumber == e.PlayerNumber);

                if (opponent is not null)
                {
                    opponent = FindOpponent(gameState, player, opponent);

                    if (opponent is null)
                    {
                        if (AdditionalCard is not null && SelectedCard == InitialCard)
                        {
                            gameState.EndTurn(AdditionalCard);
                        }
                        else
                        {
                            gameState.EndTurn(InitialCard);
                        }

                        return;
                    }

                    AuditItem.Append(
                      gameState.Id, player,
                      string.Join(" ", nameof(FindOpponent), "Picked player: " + opponent.Nickname),
                      ApplicationState.Instance.Connection);

                    if (AdditionalCard is not null)
                    {
                        if (SelectedCard == InitialCard)
                        {
                            if (AdditionalCard > opponent.CurrentCard)
                            {
                                gameState.Lose(opponent);
                            }
                            else if (AdditionalCard < opponent.CurrentCard)
                            {
                                gameState.Lose(player);
                                lobby.Leave(player.Nickname);
                            }
                        }

                        if (SelectedCard == AdditionalCard)
                        {
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
                    }
                }

                gameState = GameState.Fetch(gameState.Id, ApplicationState.Instance.Connection);
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

                gameState = GameState.Fetch(gameState.Id, ApplicationState.Instance.Connection);
                var opponent = gameState.Players.FirstOrDefault(p => p.PlayerNumber == e.PlayerNumber);

                if (opponent is not null)
                {
                    opponent = FindOpponent(gameState, player, opponent);

                    if (opponent is null)
                    {
                        if (AdditionalCard is not null && SelectedCard == InitialCard)
                        {
                            gameState.EndTurn(AdditionalCard);
                        }
                        else
                        {
                            gameState.EndTurn(InitialCard);
                        }

                        return;
                    }

                    AuditItem.Append(
                      gameState.Id, player,
                      string.Join(" ", nameof(FindOpponent), "Picked player: " + opponent.Nickname),
                      ApplicationState.Instance.Connection);

                    this.AlertMessage($"You revealed opponents`s card. His card type is {opponent.CurrentCard}");
                }

                gameState = GameState.Fetch(gameState.Id, ApplicationState.Instance.Connection);
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

                gameState = GameState.Fetch(gameState.Id, ApplicationState.Instance.Connection);
                var opponent = gameState.Players.FirstOrDefault(p => p.PlayerNumber == e.PlayerNumber);
                
                if (opponent is not null)
                {
                    opponent = FindOpponent(gameState, player, opponent);

                    if (opponent is null)
                    {
                        if (AdditionalCard is not null && SelectedCard == InitialCard)
                        {
                            gameState.EndTurn(AdditionalCard);
                        }
                        else
                        {
                            gameState.EndTurn(InitialCard);
                        }

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
                        Close();
                    }
                    else
                    {
                        gameState = GameState.Fetch(gameState.Id, ApplicationState.Instance.Connection);
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
            if (player.PlayerNumber == opponent.PlayerNumber)
            {
                return null;
            }

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
            var player = ApplicationState.Instance.CurrentPlayer;

            if (player is not null)
            {
                ApplicationState.Instance.CurrentGameState?.Lose(player);
                ApplicationState.Instance.CurrentLobby?.Leave(player.Nickname);
            }
            
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
                    Close();
                    return;
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
                Close();
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
                var player = ApplicationState.Instance.CurrentPlayer;

                if (gameState is null)
                {
                    throw new NullReferenceException(nameof(gameState));
                }

                if (player is null)
                {
                    throw new NullReferenceException(nameof(player));
                }

                if ((short)PlayerNumberValue.Value == player.PlayerNumber)
                {
                    this.AlertMessage("If you choose yourself there wouldn`t be any effects!");
                    return;
                }

                var opponents = gameState.Players.Where(p => p.Available);

                if (!opponents.Any(o => o.PlayerNumber == (short)PlayerNumberValue.Value))
                {
                    this.AlertMessage("If you choose this player, he/she wouldn`t be affected somehow becaue of " + CardType.Handmaid.ToString());
                    return;
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

                gameState = GameState.Fetch(gameState.Id, ApplicationState.Instance.Connection);

                AuditGrid.DataSource = DataGridUtils.LoadAudit(gameState.Id, ApplicationState.Instance.Connection);
                AuditGrid.Update();
                AuditGrid.Refresh();

                RefreshCardHistory(gameState);

                CardsCount.Text = GetCardsCount(gameState).ToString();
            }
            catch (NotExistingEntityException)
            {
                return;
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
                CardHistoryListBox.Items.Add($"{card.CardType} was played");
            }
        }

        private void PlayerValueValue_ValueChanged(object sender, EventArgs e)
        {
            if ((short)PlayerValueValue.Value <= 1)
            {
                this.AlertMessage("Player value couldn`t be less or equal to 1");
                PlayerValueValue.Value = PlayerValueValue.Minimum;
                return;
            }
        }
    }
}
