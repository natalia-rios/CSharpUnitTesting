using NUnit.Framework;
using TestNinja.Fundamentals;
using Assert = NUnit.Framework.Assert;

namespace TestNInja.UnitTests;

public class DemeritPointsCalculatorTests
{
    private DemeritPointsCalculator _demeritPointsCalculator;
    
    [SetUp]
    public void SetUp()
    {
        _demeritPointsCalculator = new DemeritPointsCalculator();
    }

    [Test]
    [TestCase(0, 0)]
    [TestCase(64, 0)]
    [TestCase(65, 0)]
    [TestCase(66, 0)]
    [TestCase(70, 1)]
    [TestCase(75, 2)]

    public void CalculateDemeritPoints_whenCalled_ReturnDemeritPoints(int speed, int expectedResult)
    {
        var result = _demeritPointsCalculator.CalculateDemeritPoints(speed);
        Assert.That(result, Is.EqualTo(expectedResult));
    }
    
    [Test]
    [TestCase(-2)]
    [TestCase(500)]
    public void CalculateDemeritPoints_speedIsOutOfRange_ThrowOutOfRangeException(int speed)
    {
        Assert.That(() => _demeritPointsCalculator.CalculateDemeritPoints(speed), 
            Throws.Exception.TypeOf<ArgumentOutOfRangeException>());
    }
}