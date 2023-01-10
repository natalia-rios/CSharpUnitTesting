using Moq;
using NUnit.Framework;
using TestNinja.Mocking;
using Assert = NUnit.Framework.Assert;

namespace TestNInja.UnitTests.Mocking;

[TestFixture]
public class EmployeeControllerTests
{
    [SetUp]
    public void SetUp()
    {
        _storage = new Mock<IEmployeeStorage>();
        _controller = new EmployeeController(_storage.Object);
    }

    private EmployeeController _controller;
    private Mock<IEmployeeStorage> _storage;


    [Test]
    public void DeleteEmployee_WhenCalled_DeleteTheEmployeeFromDb()
    {
        _controller.DeleteEmployee(1);

        _storage.Verify(s => s.DeleteEmployee(1));
    }

    [Test]
    public void DeleteEmployee_WhenCalled_ReturnRedirectResultObject()
    {
        var result = _controller.DeleteEmployee(1);

        Assert.IsInstanceOf<ActionResult>(result);
    }
}