using System;

namespace BlogPlatform.ViewModels
{
    public class RatingViewModel
    {
        public int Id { get; set; }
        public double Value { get; set; }
        public DateTime DateAdded { get; set; }
        public int ArticleId { get; set; }
        public AccountViewModel Author { get; set; }
    }
}
