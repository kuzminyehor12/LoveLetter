namespace LoveLetter.Core.Utils
{
    public class ApplicationEvents
    {
        public event EventHandler? OnGameStopped;

        public void OnGameStoppedHandler(EventArgs args)
        {
            OnGameStopped?.Invoke(this, args);
        }
    }
}
