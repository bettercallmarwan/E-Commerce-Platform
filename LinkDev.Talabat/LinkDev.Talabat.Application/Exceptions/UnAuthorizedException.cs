namespace LinkDev.Talabat.Core.Application.Exceptions
{
    public class UnAuthorizedException : ApplicationException
    {
        public UnAuthorizedException(string message)
            : base(message)
        {
            
        }
    }
}
