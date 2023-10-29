using LoveLetter.Core.Utils;
using System.Xml.Serialization;

namespace LoveLetter.Core.Entities
{
    public class Player : DomainEntity
    {
        private string? _nickname;

        public short PlayerNumber { get; set; }

        [XmlElement("PlayerNickname")]
        public string Nickname
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
            Nickname = nickName;
        }
    }
}
