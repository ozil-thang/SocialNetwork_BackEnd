using System;
namespace Domain
{
    public class Like
    {
        public Like()
        {
        }
        public string UserId { get; set; }
        public Profile UserProfile { get; set; }

        public string PostId { get; set; }
        public Post Post { get; set; }

        public DateTime Date { get; set; }
    }
}
