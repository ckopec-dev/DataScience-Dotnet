
namespace Core.Internet
{
    public enum NntpResponseCode
    {
        ServerReadyPostingAllowed = 200,
        ServerReadyNoPostingAllowed = 201,
        ClosingConnection = 205,
        GroupSelected = 211,
        ListFollows = 215
    }
}
