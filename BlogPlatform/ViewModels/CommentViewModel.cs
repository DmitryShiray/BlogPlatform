using System;

namespace BlogPlatform.ViewModels
{
    public class CommentViewModel
    {
        public string Text { get; set; }
        public DateTime DateAdded { get; set; }
        public AccountViewModel Account { get; set; }
    }
}
