namespace GSES2.Domain.Exceptions;
public class DomainException : Exception
{
    public int? InternalStatusCode { get; }

    public DomainException(string message, int? internalStatusCode = null) 
        : base(message)
    {
        InternalStatusCode = internalStatusCode;
    }
}
