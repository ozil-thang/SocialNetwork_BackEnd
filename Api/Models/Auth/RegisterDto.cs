using System;
using FluentValidation;

namespace Api.Models.Auth
{
    public class RegisterDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class RegisterDtoValidator : AbstractValidator<RegisterDto>
    {
        public RegisterDtoValidator()
        {
            RuleFor(x => x.Email).EmailAddress();
            RuleFor(x => x.Password).NotEmpty();
        }
    }
}
