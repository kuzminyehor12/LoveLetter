namespace LoveLetter.Core.Exceptions
{
    public class WrongPlayerNumberException : Exception
    {
        public WrongPlayerNumberException() : base()
        {

        }

        public WrongPlayerNumberException(string msg) : base(msg)
        {

        }
    }
}
