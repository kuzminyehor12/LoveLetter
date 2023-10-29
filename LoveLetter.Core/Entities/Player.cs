using LoveLetter.Core.Utils;

namespace LoveLetter.Core.Entities
{
    public class Player : DomainEntity
    {
        private string? _nickname;

        public short PlayerNumber { get; set; }

        public string NickName
        {
            get
            {
                if (_nickname is null)
                {
                    _nickname = "Player " + PlayerNumber;
                }

                return _nickname;
            }
            set { _nickname = value; }
        }

        public CardType CurrentCard { get; set; }

        public bool Available { get; set; } = true;

        public Player(short playerNumber, string nickName)
        {
            PlayerNumber = playerNumber;
            NickName = nickName;
        }
    }
}
