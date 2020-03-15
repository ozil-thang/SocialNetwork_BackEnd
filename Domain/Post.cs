using System;
using System.Collections.Generic;

namespace Domain
{
    public class Post
    {
        public Post()
        {
            Likes = new List<Like>();
            Comments = new List<Comment>();
            Date = DateTime.Now;
        }
        public string Id { get; set; }

        public string UserId { get; set; }
        public Profile UserProfile { get; set; }

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
