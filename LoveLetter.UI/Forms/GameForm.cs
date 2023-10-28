using LoveLetter.Core.Entities;
using LoveLetter.Core.Utils;
using LoveLetter.UI.Infrastructure;

namespace LoveLetter.UI.Forms
{
    public partial class GameForm : Form
    {
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

                if (cardEvents is null)
                {
                    throw new NullReferenceException(nameof(cardEvents));
                }

                if (gameState is null)
                {
                    throw new NullReferenceException(nameof(gameState));
                }

                var args = new CardEventArgs((short)PlayerNumberValue.Value, (short)PlayerValueValue.Value);
                SelectedCard.Effect(cardEvents, args);
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

                ApplicationState.Instance.CurrentGameState = GameState.Fetch(lobby.Id);
                var gameState = ApplicationState.Instance.CurrentGameState;

                if (gameState.WinnerPlayerNumber is not null)
                {
                    this.SendResult(gameState.WinnerPlayerNumber.Value);

                    if (ApplicationState.Instance.ApplicationEvents is not null)
                    {
                        ApplicationState.Instance.ApplicationEvents.OnGameStoppedHandler(e);
                    }
                }

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
                SelectedCard = new Card(InitialCard);
                InitialCard = new Card(AdditionalCard);
            }
        }

        private void AdditionalCardPicture_Click(object sender, EventArgs e)
        {
            // Hover picture
            if (AdditionalCard is not null)
            {
                SelectedCard = new Card(AdditionalCard);
            }
        }

        private void CardEvents_OnPrincess(object sender, CardEventArgs e)
        {
            try
            {

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

                if (gameState is null)
                {
                    throw new NullReferenceException(nameof(gameState));
                }

                var opponent = gameState.Players.FirstOrDefault(p => p.PlayerNumber == e.PlayerNumber);

                if (opponent is not null)
                {
                    // Auditing shown card(local audit)
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

                var playerToGuess = gameState.Players.FirstOrDefault(p => p.PlayerNumber == e.PlayerNumber);
                if (playerToGuess is not null && e.CardType == (short)playerToGuess.CurrentCard.CardType)
                {
                    gameState.SetWinner(player.PlayerNumber);
                    this.Congratulate();

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
            catch (Exception ex)
            {
                this.ThrowError(ex);
            }
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
        }
    }
}
