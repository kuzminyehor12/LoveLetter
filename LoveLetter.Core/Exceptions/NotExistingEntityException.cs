namespace LoveLetter.Core.Exceptions
{
    public class NotExistingEntityException : Exception
    {
        public NotExistingEntityException() : base()
        {

        }

        public NotExistingEntityException(string msg) : base(msg)
        {

        }
    }
}
