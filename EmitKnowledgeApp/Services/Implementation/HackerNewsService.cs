using EmitKnowledgeApp.Models;
using Newtonsoft.Json;
using System.Net.Http;

namespace EmitKnowledgeApp.Services.Implementation
{
    public class HackerNewsService : IHackerNewsService
    {
        private readonly HttpClient _httpClient;
        public HackerNewsService()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://hacker-news.firebaseio.com/v0/");
        }
        public async Task<List<News>> GetTopNews()
        {
            try
            {
                string responseContent = await _httpClient.GetStringAsync("newstories.json");
                List<int> topStoryIds = JsonConvert.DeserializeObject<List<int>>(responseContent);

                List<News> newsItems = new List<News>();
                if (topStoryIds != null && topStoryIds.Count > 0)
                {
                   foreach (int storyId in topStoryIds)
                    {
                        News newsItem = await GetNewsItemDetails(storyId);
                        if (newsItem != null)
                        {
                            newsItems.Add(newsItem);
                        }
                    }                   

                    return newsItems;
                }
                else
                {
                    throw new Exception("Failed to fetch top news items.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred: {ex.Message}");
            }
        }
        private async Task<News> GetNewsItemDetails(int newsId)
        {
            try
            {
                string json = await _httpClient.GetStringAsync($"item/{newsId}.json");
                dynamic newsItemJson = JsonConvert.DeserializeObject(json);
                News newsItem = new News
                {
                    Title = newsItemJson.title,
                    Url = newsItemJson.url,
                    Score = newsItemJson.score,
                    By = newsItemJson.by,
                    Time = newsItemJson.time,
                    Descendants = newsItemJson.descendants,
                    Type = newsItemJson.type
                };
                return newsItem;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to get news items: {ex.Message}");
            }
        }

        public async Task<List<News>> GetNewsSortedByNewestToOldest()
        {
            try
            {
                List<News> newsItems = await GetTopNews();
                return newsItems.OrderByDescending(item => item.Time).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to sort news: {ex.Message}");
            }
        }
    }
}

