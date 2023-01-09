using NUnit.Framework;
using TestNinja.Fundamentals;
using Assert = NUnit.Framework.Assert;

namespace TestNInja.UnitTests;

[TestFixture]
public class CustomerControllerTests
{
    private CustomerController _customerController;
    
    [SetUp]
    public void SetUp()
    {
        _customerController = new CustomerController();
    }
    
    [Test]
    public void GetCustomer_IdIsZero_ReturnNotFound()
    {

        var result = _customerController.GetCustomer(0);

        // NotFound
        Assert.That(result, Is.TypeOf<NotFound>());
        
        // NotFound or one of its derivatives
        // Assert.That(result, Is.InstanceOf<NotFound>());
    }

    [Test]
    public void GetCustomer_IdIsNotZero_ReturnOk()
    {
        var result = _customerController.GetCustomer(2);
        
        Assert.That(result, Is.TypeOf<Ok>());
    }
}