namespace LoveLetter.Core.Utils
{
    public class CardEvents
    {
        public delegate void CardEventHandler(object sender, CardEventArgs e);

        public event CardEventHandler? OnGuard;

        public void OnGuardPicked(CardEventArgs args)
        {
            OnGuard?.Invoke(this, args);
        }

        public event CardEventHandler? OnPriest;

        public void OnPriestPicked(CardEventArgs args)
        {
            OnPriest?.Invoke(this, args);
        }

        public event CardEventHandler? OnBaron;

        public void OnBaronPicked(CardEventArgs args)
        {
            OnBaron?.Invoke(this, args);
        }

        public event CardEventHandler? OnHandmaid;

        public void OnHandmaidPicked(CardEventArgs args)
        {
            OnHandmaid?.Invoke(this, args);
        }

        public event CardEventHandler? OnPrince;

        public void OnPrincePicked(CardEventArgs args)
        {
            OnPrince?.Invoke(this, args);
        }

        public event CardEventHandler? OnKing;

        public void OnKingPicked(CardEventArgs args)
        {
            OnKing?.Invoke(this, args);
        }

        public event CardEventHandler? OnCountess;

        public void OnCountessPicked(CardEventArgs args)
        {
            OnCountess?.Invoke(this, args);
        }

        public event CardEventHandler? OnPrincess;

        public void OnPrincessPicked(CardEventArgs args)
        {
            OnPrincess?.Invoke(this, args);
        }
    }
}
