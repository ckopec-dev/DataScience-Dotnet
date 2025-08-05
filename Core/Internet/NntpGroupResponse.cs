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
        public int? ResponseCode { get; set; }
        public int? ArticleCount { get; set; }
        public int? FirstArticle { get; set; }
        public int? LastArticle { get; set; }
    }
}
