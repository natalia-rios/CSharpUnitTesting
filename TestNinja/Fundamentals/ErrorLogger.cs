namespace TestNinja.Fundamentals;

using System;
    
public class ErrorLogger
{
    public string LastError { get; set; }
    
    public event EventHandler<Guid> ErrorLogged;

    public Guid _errorId;
    
    public void Log(string error)
    {
        if (String.IsNullOrWhiteSpace(error))
            throw new ArgumentNullException();
            
        LastError = error; 
        
        // Write the log to a storage
        // ...

        _errorId = Guid.NewGuid();
        
        ErrorLogged?.Invoke(this, _errorId);
    }
}
