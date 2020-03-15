using System;
namespace Api.Models.Post
{
    public class PostItemDto
    {
        public string Id { get; set; }

        public string DisplayName { get; set; }

        public string Avatar { get; set; }

        public string Text { get; set; }

        public string Photo { get; set; }

        public string Video { get; set; }

        public DateTime Date { get; set; }

        public int LikesCount { get; set; }

        public int CommentsCount { get; set; }

        public bool IsLike { get; set; }
    }
}
