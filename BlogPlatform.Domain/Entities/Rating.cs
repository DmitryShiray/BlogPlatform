using System;

namespace BlogPlatform.Domain.Entities
{
    public class Rating : IEntityBase
    {
        public int Id { get; set; }
        public byte Value { get; set; }
        public DateTime DateAdded { get; set; }
        public int ArticleId { get; set; }
        public int AccountId { get; set; }
        public virtual Article Article { get; set; }
        public virtual Account Account { get; set; }
    }
}
