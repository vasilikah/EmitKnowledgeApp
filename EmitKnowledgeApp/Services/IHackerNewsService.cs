using EmitKnowledgeApp.Models;

namespace EmitKnowledgeApp.Services
{
    public interface IHackerNewsService
    {
        Task<List<News>> GetTopNews();
        Task<List<News>> GetNewsSortedByNewestToOldest();
        Task<List<News>> GetHotNews();
        Task<List<News>> GetAllAskHNNews();
        Task<List<News>> GetAllShowHNNews();
        Task<List<News>> SearchNewsByKeyword(string keyword);
        Task<List<Comment>> GetCommentsForNewsItem(int newsItemId);
    }
}
