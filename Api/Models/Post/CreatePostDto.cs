using System;
using Microsoft.AspNetCore.Http;

namespace Api.Models.Post
{
    public class CreatePostDto
    {
        public string Text { get; set; }

        public IFormFile Photo { get; set; }

        public IFormFile Video { get; set; }
    }
}
