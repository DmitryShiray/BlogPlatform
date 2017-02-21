using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogPlatform.Domain.Entities
{
    public class Article : IEntityBase
    {
        public Article()
        {
            Comments = new List<Comment>();
            Ratings = new List<Rating>();
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime LastDateModified { get; set; }
        public int AccountId { get; set; }
        public virtual Account Account { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Rating> Ratings { get; set; }
    }
}
