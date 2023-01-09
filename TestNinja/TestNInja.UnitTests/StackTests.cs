using NUnit.Framework;
using Assert = NUnit.Framework.Assert;

namespace TestNInja.UnitTests;

public class StackTests
{ 
    private TestNinja.Fundamentals.Stack<string> _stack;
    
    [SetUp]
    public void SetUp()
    {
        _stack = new TestNinja.Fundamentals.Stack<string>();
    }

    [Test]
    public void Push_ArgIsNull_ThrowArgNullException()
    {
        Assert.That(() => _stack.Push(null), Throws.ArgumentNullException);
    }

    [Test]
    public void Push_ValidArg_AddTheObjectToTheStack()
    {
        _stack.Push("a");
        
        Assert.That(_stack.Count, Is.EqualTo(1));
    }

    [Test]
    public void Count_EmptyStack_ReturnZero()
    {
        Assert.That(_stack.Count, Is.EqualTo(0));
    }

    [Test]
    public void Push_ValidArg_AddObjectToTheStack()
    {
        _stack.Push("a");
        
        Assert.That(_stack.Count, Is.EqualTo(1));
    }

    [Test]
    public void Pop_EmptyStack_ThrowInvalidOperationException()
    {
        var _stack = new TestNinja.Fundamentals.Stack<string>();
        
        Assert.That(() => _stack.Pop(), Throws.InvalidOperationException);
    }

    [Test]
    public void Pop_StackWithAFewObjects_ReturnObjectOnTheTop()
    {
        _stack.Push("a");
        _stack.Push("b");
        _stack.Push("c");

        var result = _stack.Pop();
        
        Assert.That(result, Is.EqualTo("c"));
    }
    
    [Test]
    public void Pop_StackWithAFewObjects_RemoveObjectOnTheTop()
    {
        _stack.Push("a");
        _stack.Push("b");
        _stack.Push("c");

        var result = _stack.Pop();
        
        Assert.That(_stack.Count, Is.EqualTo(2));
    }

    [Test]
    public void Peek_EmptyStack_ThrowInvalidOperationException()
    {
        Assert.That(() => _stack.Peek(), Throws.InvalidOperationException);
    }

    [Test]
    public void Peek_StackWithObjects_ReturnObjectOnTopOfTheStack()
    {
        _stack.Push("a");
        _stack.Push("b");
        _stack.Push("c");

        var result = _stack.Peek();
        
        Assert.That(result, Is.EqualTo("c"));
    }

    [Test]
    public void Peek_StackWithObjects_DoesNotRemoveObjectOnTopOfTheStack()
    {
        _stack.Push("a");
        _stack.Push("b");
        _stack.Push("c");

        _stack.Peek();
        
        Assert.That(_stack.Count, Is.EqualTo(3));
    }
    
}