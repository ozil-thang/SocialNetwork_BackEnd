using System;
using System.Collections.Generic;

namespace Api.Models.Post
{
    public class PostDetailDto
    {
        public string Id { get; set; }

        public string DisplayName { get; set; }

        public string Avatar { get; set; }

        public string Text { get; set; }

        public string Photo { get; set; }

        public string Video { get; set; }

        public DateTime Date { get; set; }

        public int LikesCount { get; set; }

        public bool isLike { get; set; }
        public ICollection<CommentDto> Comments { get; set; }
    }
}
