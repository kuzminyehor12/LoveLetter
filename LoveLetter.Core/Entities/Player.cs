using LoveLetter.Core.Utils;
using System.Xml.Serialization;

namespace LoveLetter.Core.Entities
{
    [XmlRoot("Players")]
    public class PlayersList : List<Player>
    {
        public PlayersList()
        {

        }

        public static List<Player> Empty() =>
            new List<Player>();
    }

    public class Player : DomainEntity
    {
        private string? _nickname;

        [XmlElement("PlayerNumber")]
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

        [XmlElement("PlayerCard")]
        public CardType CurrentCard { get; set; }

        [XmlElement("PlayerAvailable")]
        public bool Available { get; set; } = true;

        [XmlIgnore]
        public bool IsHost { get; set; } = false;

        public Player()
        {
            Available = true;
            CurrentCard = CardType.Unknown;
        }

        public Player(short playerNumber, string nickName) : this()
        {
            PlayerNumber = playerNumber;
            Nickname = nickName;
        }

        public Player(short playerNumber, bool isHost, string nickName) : this(playerNumber, nickName)
        {
            IsHost = isHost;
        }
    }
}
