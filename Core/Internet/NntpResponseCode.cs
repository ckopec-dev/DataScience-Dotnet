
namespace Core.Internet
{
    public enum NntpResponseCode
    {
        ServerReadyPostingAllowed = 200,
        ServerReadyNoPostingAllowed = 201,
        ClosingConnection = 205,
        ListFollows = 215
    }
}
