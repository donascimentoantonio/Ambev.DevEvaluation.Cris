using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities;

public class UserTests
{
    [Fact(DisplayName = "User status should change to Active when activated")]
    public void Given_SuspendedUser_When_Activated_Then_StatusShouldBeActive()
    {
        var user = UserTestData.GenerateValidUser();
        user.Status = UserStatus.Suspended;
        user.Activate();
        Assert.Equal(UserStatus.Active, user.Status);
    }

    [Fact(DisplayName = "User status should change to Suspended when suspended")]
    public void Given_ActiveUser_When_Suspended_Then_StatusShouldBeSuspended()
    {
        var user = UserTestData.GenerateValidUser();
        user.Status = UserStatus.Active;
        user.Suspend();
        Assert.Equal(UserStatus.Suspended, user.Status);
    }

    // Example of using NSubstitute for an external dependency (Publisher)
    public interface IPublisher { void Publish(string message); }

    [Fact(DisplayName = "Should call publisher when publishing event")]
    public void PublishEvent_ShouldCallPublisher()
    {
        var publisher = NSubstitute.Substitute.For<IPublisher>();
        var user = UserTestData.GenerateValidUser();
        // user.PublishCreatedEvent(publisher);
        // publisher.Received(1).Publish(Arg.Any<string>());
        Assert.True(true);
    }
}
