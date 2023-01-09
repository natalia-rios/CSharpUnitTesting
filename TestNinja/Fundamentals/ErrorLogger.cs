namespace TestNinja.Fundamentals;

public class ErrorLogger
{
    public Guid _errorId;
    public string LastError { get; set; }

    public event EventHandler<Guid> ErrorLogged;

    public void Log(string error)
    {
        if (string.IsNullOrWhiteSpace(error))
            throw new ArgumentNullException();

        LastError = error;

        // Write the log to a storage
        // ...

        _errorId = Guid.NewGuid();

        ErrorLogged?.Invoke(this, _errorId);
    }
}