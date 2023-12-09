using EmitKnowledgeApp.Models;

namespace EmitKnowledgeApp.Services
{
    public interface IHackerNewsService
    {
        Task<List<News>> GetTopNews();
        Task<List<News>> GetNewsSortedByNewestToOldest();
    }
}
