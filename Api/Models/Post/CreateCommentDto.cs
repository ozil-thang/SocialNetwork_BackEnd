using System;
using FluentValidation;

namespace Api.Models.Post
{
    public class CreateCommentDto
    {
        public string Text { get; set; }
    }

    public class CreateCommentDtoValidator : AbstractValidator<CreateCommentDto>
    {
        public CreateCommentDtoValidator()
        {
            RuleFor(x => x.Text).Must(x => !String.IsNullOrEmpty(x));
        }
    }
}
