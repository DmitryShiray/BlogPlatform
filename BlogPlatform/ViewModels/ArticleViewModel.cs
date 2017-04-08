using System;

namespace BlogPlatform.ViewModels
{
    public class ArticleViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime DateCreated { get; set; }
        public int TotalComments { get; set; }
        public double Rating { get; set; }
        public AccountViewModel Account { get; set; }
    }
}