using FluentValidation.TestHelper;
using SimpleExample.Application.DTOs;
using SimpleExample.Application.Validators;

namespace SimpleExample.Tests.Validators;

public class CreateUserDtoValidatorTests
{
    private readonly CreateUserDtoValidator _validator;

    public CreateUserDtoValidatorTests()
    {
        _validator = new CreateUserDtoValidator();
    }

    [Fact]
    public void Should_Have_Error_When_FirstName_Is_Empty()
    {
        var dto = new CreateUserDto
        {
            FirstName = "",
            LastName = "Meikäläinen",
            Email = "test@test.com"
        };

        var result = _validator.TestValidate(dto);

        // intentionally wrong expectation for the assignment step
        result.ShouldNotHaveAnyValidationErrors();
    }
}
