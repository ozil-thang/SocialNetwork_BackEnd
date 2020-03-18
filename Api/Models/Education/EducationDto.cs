using System.Security.Cryptography.X509Certificates;
using System;
using FluentValidation;

namespace Api.Models.Education
{
    public class EducationDto
    {
        public string Id { get; set; }
        public string School { get; set; }
        public string Degree { get; set; }
        public string FieldOfStudy { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public Boolean Current { get; set; }
        public string Description { get; set; }
    }

    public class EducationDtoValidator : AbstractValidator<EducationDto>
    {
        public EducationDtoValidator()
        {
            RuleFor(x => x.School).Must(x => !String.IsNullOrEmpty(x));
            RuleFor(x => x.From).NotNull();
            RuleFor(x => x.To).NotNull();
        }
    }
}
