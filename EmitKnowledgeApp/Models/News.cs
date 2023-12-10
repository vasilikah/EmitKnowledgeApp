using System.Xml.Linq;

namespace EmitKnowledgeApp.Models
{
    public class News
    {
        public string Title { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public int Score { get; set; }
        public string By { get; set; } = string.Empty;
        public DateTime Time { get; set; }
        public int Descendants { get; set; }
        public string Type {  get; set; } = string.Empty;

    }
}
