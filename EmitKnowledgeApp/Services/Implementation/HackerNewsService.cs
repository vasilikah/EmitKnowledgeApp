using EmitKnowledgeApp.Models;
using Newtonsoft.Json;
using System.Net.Http;
using static System.Net.WebRequestMethods;

namespace EmitKnowledgeApp.Services.Implementation
{
    public class HackerNewsService : IHackerNewsService
    {
        private readonly HttpClient _httpClient;
        private const string AskHNPrefix = "Ask HN:";
        private const string ShowHNPrefix = "Show HN:";
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

        public async Task<List<News>> GetHotNews()
        {
            try
            {
                List<News> topNewsItems = await GetTopNews();

                var sortedNews = topNewsItems.OrderByDescending(item => item.Score).ToList();

                return sortedNews;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to fetch and sort hot news items: {ex.Message}");
            }
        }

        public async Task<List<News>> GetAllAskHNNews()
        {
            try
            {
                string askHNJson = await _httpClient.GetStringAsync("askstories.json");
                List<int> askHNStoryIds = JsonConvert.DeserializeObject<List<int>>(askHNJson);

                List<News> askHNStories = new List<News>();

                foreach (int storyId in askHNStoryIds)
                {
                    News newsItem = await GetNewsItemDetails(storyId);
                    askHNStories.Add(newsItem);
                }
                askHNStories = askHNStories.Where(item => item.Title.StartsWith(AskHNPrefix, StringComparison.OrdinalIgnoreCase)).ToList();
                return askHNStories;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to fetch Ask HN stories: {ex.Message}");
            }

        }

        public async Task<List<News>> GetAllShowHNNews()
        {
            try
            {
                string showHNJson = await _httpClient.GetStringAsync("showstories.json");
                List<int> showHNStoryIds = JsonConvert.DeserializeObject<List<int>>(showHNJson);

                List<News> showHNStories = new List<News>();

                foreach (int storyId in showHNStoryIds)
                {
                    News newsItem = await GetNewsItemDetails(storyId);
                    showHNStories.Add(newsItem);
                }

                showHNStories = showHNStories.Where(item => item.Title.StartsWith(ShowHNPrefix, StringComparison.OrdinalIgnoreCase)).ToList();

                return showHNStories;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to fetch Show HN stories: {ex.Message}");
            }
        }

        public async Task<List<News>> SearchNewsByKeyword(string keyword)
        {
            try
            {
                List<News> allNewsItems = await GetTopNews(); // Fetch all news items

                
                var searchResults = allNewsItems.Where(item =>
                    item.Title.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                    //item.Content != null && item.Content.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                    item.By.Contains(keyword, StringComparison.OrdinalIgnoreCase)
                ).ToList();

                return searchResults;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to search news by keyword: {ex.Message}");
            }
        }
        public async Task<List<Comment>> GetCommentsForNewsItem(int newsItemId)
        {
            try
            {
                string json = await _httpClient.GetStringAsync($"item/{newsItemId}.json");
                dynamic newsItemJson = JsonConvert.DeserializeObject(json);

                List<int> commentIds = newsItemJson.kids != null ?
                    JsonConvert.DeserializeObject<List<int>>(newsItemJson.kids.ToString()) :
                    new List<int>();

                List<Comment> comments = new List<Comment>();

                foreach (int commentId in commentIds)
                {
                    string commentJson = await _httpClient.GetStringAsync($"item/{commentId}.json");
                    dynamic commentItemJson = JsonConvert.DeserializeObject(commentJson);

                    Comment comment = new Comment
                    {
                        Text = commentItemJson.text,
                        By = commentItemJson.by,
                        Time = commentItemJson.time,
                        Type = commentItemJson.type,
                    };

                    comments.Add(comment);
                }

                return comments;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to fetch comments for the news item: {ex.Message}");
            }
        }


    }
}


