using FluentValidation;
using SimpleExample.Application.DTOs;

namespace SimpleExample.Application.Validators
{
    public class CreateUserDtoValidator : AbstractValidator<CreateUserDto>
    {
        public CreateUserDtoValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty()
                .WithMessage("Etunimi on pakollinen");

            RuleFor(x => x.LastName)
                .NotEmpty()
                .WithMessage("Sukunimi on pakollinen");

            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("Sähköposti on pakollinen")
                .EmailAddress()
                .WithMessage("Sähköpostin tulee olla kelvollinen");
        }
    }
}
