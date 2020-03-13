using System;
using System.Collections.Generic;

namespace Domain
{
    public class Post
    {
        public Post()
        {
        }
        public string Id { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }

        public string Text { get; set; }

        public string PhotoId { get; set; }
        public Photo Photo { get; set; }

        public string VideoId { get; set; }
        public Video Video { get; set; }

        public DateTime Date { get; set; }

        public ICollection<Like> Likes { get; set; }
        public ICollection<Comment> Comments { get; set; }
    }
}
