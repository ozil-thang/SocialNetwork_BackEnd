using System;
namespace Api.Models.Post
{
    public class CommentDto
    {
        public string Id { get; set; }

        public string DisplayName { get; set; }
        public string Avatar { get; set; }

        public string Text { get; set; }
        public DateTime Date { get; set; }
    }
}
