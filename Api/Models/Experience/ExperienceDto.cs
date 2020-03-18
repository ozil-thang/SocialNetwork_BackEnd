using System;
using FluentValidation;

namespace Api.Models.Experience
{
    public class ExperienceDto
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Company { get; set; }
        public string Location { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public Boolean Current { get; set; }
        public string Description { get; set; }
    }

    public class ExperienceDtoValidator : AbstractValidator<ExperienceDto>
    {
        public ExperienceDtoValidator()
        {
            RuleFor(x => x.Title).Must(t => !String.IsNullOrEmpty(t));
            RuleFor(x => x.Company).Must(t => !String.IsNullOrEmpty(t));
            RuleFor(x => x.From).NotNull();
            RuleFor(x => x.To).NotNull();
        }
    }
}
