
namespace Core.Internet
{
    public class NntpUnknownResponseCodeException : Exception
    {
        public int ResponseCode { get; }
        
        public NntpUnknownResponseCodeException()
        {
        }
        public NntpUnknownResponseCodeException(int code) 
        {
            ResponseCode = code;
        }
    }
}
