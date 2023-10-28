using LoveLetter.Core.Constants;

namespace LoveLetter.Core.Utils
{
    public class CardEventArgs : EventArgs
    {
        private short _playerNumber;

        private short _cardType;

        public CardEventArgs(short playerNumber, short cardType)
        {
            PlayerNumber = playerNumber;
            CardType = cardType;
        }

        public short PlayerNumber
        {
            get { return _playerNumber; }
            set
            {
                if (value < 1 || value > Constraints.MAX_PLAYER_NUMBER)
                {
                    throw new Exception();
                }

                _playerNumber = value;
            }
        }

        public short CardType
        {
            get { return _cardType; }
            set
            {
                if (value < 1 || value > Enum.GetValues(typeof(CardType)).Length)
                {
                    throw new Exception();
                }

                _cardType = value;
            }
        }
    }
}
