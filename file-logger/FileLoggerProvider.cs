namespace Gaming.Cougar.FileLogger;


public class IgnoreLogger
  : Microsoft.Extensions.Logging.ILogger
{

    public class IgnoreScope
        : System.IDisposable
    {
        void System.IDisposable.Dispose()
        {
        }
    }

    System.IDisposable Microsoft.Extensions.Logging.ILogger.BeginScope<TState>(TState state)
    {
        return new IgnoreScope();
    }

    bool Microsoft.Extensions.Logging.ILogger.IsEnabled(
        Microsoft.Extensions.Logging.LogLevel logLevel)
    {
        return false;
    }

    void Microsoft.Extensions.Logging.ILogger.Log<TState>(
          Microsoft.Extensions.Logging.LogLevel logLevel
        , Microsoft.Extensions.Logging.EventId eventId
        , TState state
        , System.Exception exception
        , System.Func<TState, System.Exception, string> formatter)
    { }

}


public class FileLoggerProvider
    : Microsoft.Extensions.Logging.ILoggerProvider
{

    protected FileLoggerOptions _options;
    protected IgnoreLogger _nullLogger;
    protected FileLogger _cachedLogger;


    public FileLoggerProvider(Microsoft.Extensions.Options.IOptions<FileLoggerOptions> fso)
    {
        _options = fso.Value;
        _nullLogger = new IgnoreLogger();
        _cachedLogger = new FileLogger(this, _options, "OneInstanceFitsAll");
    }


    Microsoft.Extensions.Logging.ILogger Microsoft.Extensions.Logging.ILoggerProvider
        .CreateLogger(string categoryName)
    {
        // Microsoft.Extensions.Hosting.Internal.ApplicationLifetime
        // Microsoft.Extensions.Hosting.Internal.Host
        // Microsoft.Hosting.Lifetime
        if (categoryName.StartsWith("Microsoft", System.StringComparison.Ordinal))
            return _nullLogger; // NULL is not a valid value... 

        return _cachedLogger;
    } 



    private bool disposedValue = false;

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
            }

            disposedValue = true;
        }
    }


    void System.IDisposable.Dispose()
    {
        Dispose(true);
    }


}  


