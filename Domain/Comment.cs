using System;
namespace Domain
{
    public class Comment
    {
        public Comment()
        {
        }
        public string Id { get; set; }
        public string UserId { get; set; }
        public Profile UserProfile { get; set; }

        public string PostId { get; set; }
        public Post Post { get; set; }

        public string Text { get; set; }
        public DateTime Date { get; set; }
    }
}
