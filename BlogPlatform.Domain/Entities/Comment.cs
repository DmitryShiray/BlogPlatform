using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogPlatform.Domain.Entities
{
    public class Comment : IEntityBase
    {
        public int Id { get; set; }
        public string Value { get; set; }
        public DateTime DateAdded { get; set; }
        public int ArticleId { get; set; }
        public int AccountId { get; set; }
        public virtual Article Article { get; set; }
        public virtual Account Account { get; set; }
    }
}
