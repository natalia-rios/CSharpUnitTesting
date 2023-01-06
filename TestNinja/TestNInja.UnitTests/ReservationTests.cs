using NUnit.Framework;
using TestNinja.Fundamentals;
using Assert = NUnit.Framework.Assert;

namespace TestNInja.UnitTests;

[TestFixture]
public class ReservationTests
{
    [Test]
    public void CanBeCancelledBy_AdminCancelling_ReturnsTrue()
    {
        // Arrange
        var reservation = new Reservation();
        // Act
        var result = reservation.CanBeCancelledBy(new User { IsAdmin = true });

        // Assert
        Assert.IsTrue(result);
        // NUnit
        Assert.That(result, Is.True);
        Assert.That(result == true);
    }
    
    [TestMethod]
    public void CanBeCancelledBy_SameUserCancelling_ReturnsTrue()
    {
        var user = new User {IsAdmin = false};
        var reservation = new Reservation {MadeBy = user};

        var result = reservation.CanBeCancelledBy(user);
        
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void CanBeCancelledBy_AnotherUserCancelling_ReturnsFalse()
    {
        var reservation = new Reservation {MadeBy = new User()};
        var result = reservation.CanBeCancelledBy(new User());
        
        Assert.IsFalse(result);
    }
}