namespace EmitKnowledgeApp.Models
{
    public class Comment
    {
        public int ParentId { get; set; } // Field to hold parent ID (ID of the news story)
        public string Text { get; set; }
        public string By { get; set; }
        public long Time { get; set; }
    }
}
