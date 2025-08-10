namespace Core.Internet
{
    /// <summary>
    /// Format:
    /// {ResponseCode} {Number of articles} {First article number} {Last article number} {Group name}
    /// Format example:
    /// 211 6 5376 5381 hr.org.carnet
    /// </summary>
    public class NntpGroupResponse : NntpResponse
    {
        public int? ArticleCount { get; set; }
        public int? FirstArticle { get; set; }
        public int? LastArticle { get; set; }

        public NntpGroupResponse()
        {
        }

        public NntpGroupResponse(int articleCount, int firstArticle, int lastArticle)
        {
            ArticleCount = articleCount;
            FirstArticle = firstArticle;
            LastArticle = lastArticle;
        }

        public override string ToString()
        {
            return String.Format($"{ArticleCount} {FirstArticle} {LastArticle}");
        }
    }
}
