using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities;

public class UserValidationTests
{
    [Fact(DisplayName = "Validation should pass for valid user data")]
    public void Given_ValidUserData_When_Validated_Then_ShouldReturnValid()
    {
        var user = UserTestData.GenerateValidUser();
        var result = user.Validate();
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    [Fact(DisplayName = "Validation should fail for invalid user data")]
    public void Given_InvalidUserData_When_Validated_Then_ShouldReturnInvalid()
    {
        var user = new User
        {
            Username = "",
            Password = UserTestData.GenerateInvalidPassword(),
            Email = UserTestData.GenerateInvalidEmail(),
            Phone = UserTestData.GenerateInvalidPhone(),
            Status = UserStatus.Unknown,
            Role = UserRole.None
        };
        var result = user.Validate();
        Assert.False(result.IsValid);
        Assert.NotEmpty(result.Errors);
    }
}
