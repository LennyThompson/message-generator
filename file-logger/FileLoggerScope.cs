namespace Gaming.Cougar.FileLogger;

public class FileLoggerScope<TState>
    : System.IDisposable
{     
    protected FileLogger _logger;
    protected TState _scopeName;


    public TState ScopeName => _scopeName;

    public FileLoggerScope(FileLogger logger, TState scopeName)
    {
        _logger = logger;
        _scopeName = scopeName;
    }  


    void System.IDisposable.Dispose()
    {
        _logger.EndScope(_scopeName);
    } 

}