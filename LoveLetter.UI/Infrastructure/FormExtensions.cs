using LoveLetter.Core.Constants;
using LoveLetter.Core.Utils;

namespace LoveLetter.UI.Infrastructure
{
    public static class FormExtensions
    {
        public static void ThrowError(this Form form, Exception ex) => 
             MessageBox.Show(ex.StackTrace, ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error);

        public static void ThrowIssue(this Form form, string message) =>
             MessageBox.Show(
                 message, 
                 GameMessageType.GAME_ISSUE, 
                 MessageBoxButtons.OK, 
                 MessageBoxIcon.Information);

        public static void CongratulationMessage(this Form form) =>
            MessageBox.Show(
                "Congrats! You won the game", 
                GameMessageType.GAME_RESULT, 
                MessageBoxButtons.OK, 
                MessageBoxIcon.Information);

        public static void SendResultMessage(this Form form, short winnerPlayerNumber) =>
           MessageBox.Show(
               $"Player {winnerPlayerNumber} won the game", 
               GameMessageType.GAME_RESULT, 
               MessageBoxButtons.OK, 
               MessageBoxIcon.Information);

        public static void LoseMessage(this Form form) =>
           MessageBox.Show(
               $"You lost the game!", 
               GameMessageType.GAME_RESULT, 
               MessageBoxButtons.OK, 
               MessageBoxIcon.Information);

        public static void AlertMessage(this Form form, string message) =>
          MessageBox.Show(
              message, 
              GameMessageType.GAME_ALERT, 
              MessageBoxButtons.OK, 
              MessageBoxIcon.Information);
    }
}
