using System;
using System.Collections.Generic;

namespace BlogPlatform.Domain.Entities
{
    public class Account : IEntityBase
    {
        public Account()
        {
            Articles = new List<Article>();
            Comments = new List<Comment>();
            Ratings = new List<Rating>();
        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string NickName { get; set; }
        public byte[] Password { get; set; }
        public byte[] Salt { get; set; }
        public DateTime DateCreated { get; set; }
        public virtual ICollection<Article> Articles { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Rating> Ratings { get; set; }
    }
}
