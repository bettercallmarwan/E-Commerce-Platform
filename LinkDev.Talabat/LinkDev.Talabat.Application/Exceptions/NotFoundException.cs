namespace LinkDev.Talabat.Core.Application.Exceptions
{
    public class NotFoundException : ApplicationException
    {
        public NotFoundException(string name, object key) // property name and pk
            : base($"{name} with : ({key}) is not found")
        {
            
        }
    }
}
