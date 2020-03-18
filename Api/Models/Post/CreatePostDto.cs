using System;
using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace Api.Models.Post
{
    public class CreatePostDto
    {
        public string Text { get; set; }

        public IFormFile Photo { get; set; }

        public IFormFile Video { get; set; }
    }

    public class CreatePostDtoValidator : AbstractValidator<CreatePostDto>
    {
        public CreatePostDtoValidator()
        {
            RuleFor(x => x).Custom((x, context) =>
            {
                if (String.IsNullOrEmpty(x.Text) &&
                    x.Photo == null &&
                    x.Video == null)
                {
                    context.AddFailure("failed");
                }
            });
        }
    }
}
