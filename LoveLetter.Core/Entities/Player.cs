namespace LoveLetter.Core.Entities
{
    public class Player : DomainEntity
    {
        public short PlayerNumber { get; set; }

        public string? NickName { get; set; }

        public Card CurrentCard { get; set; } = new Card();

        public Player(short playerNumber, string? nickName)
        {
            PlayerNumber = playerNumber;
            NickName = nickName;
        }
    }
}
