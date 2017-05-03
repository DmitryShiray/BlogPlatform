using System;

namespace BlogPlatform.ViewModels
{
    public class CommentViewModel
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public int ArticleId { get; set; }
        public DateTime DateAdded { get; set; }
        public AccountViewModel Author { get; set; }
    }
}
