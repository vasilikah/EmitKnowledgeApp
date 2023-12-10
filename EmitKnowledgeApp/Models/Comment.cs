﻿namespace EmitKnowledgeApp.Models
{
    public class Comment
    {
        public string Text { get; set; } = String.Empty;
        public string By { get; set; } = String.Empty;
        public DateTime Time { get; set; }
        public string Type { get; set; } = String.Empty;
    }
}
