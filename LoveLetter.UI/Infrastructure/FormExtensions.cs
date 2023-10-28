using LoveLetter.Core.Entities;

namespace LoveLetter.UI.Infrastructure
{
    public static class FormExtensions
    {
        public static void ThrowError(this Form form, Exception ex) => 
            MessageBox.Show(ex.StackTrace, ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error);

        public static void ThrowIssue(this Form form, string message) =>
             MessageBox.Show(message, "Game Issue", MessageBoxButtons.OK, MessageBoxIcon.Information);

        public static void Congratulate(this Form form) =>
            MessageBox.Show("Congrats! You won the game", "Game Result", MessageBoxButtons.OK, MessageBoxIcon.Information);

        public static void SendResult(this Form form, short winnerPlayerNumber) =>
           MessageBox.Show($"Player {winnerPlayerNumber} won the game", "Game Result", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }
}
