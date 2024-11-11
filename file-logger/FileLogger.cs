using Microsoft.Extensions.Logging;

namespace Gaming.Cougar.FileLogger;

public class FileLogger
    : ILogger
        , System.IDisposable
{
    
        protected const int NU_INDENT_SPACES = 4;

        protected object _scopeLock;
        protected object _lock;

        protected LogLevel _logLevel;
        protected ILoggerProvider _provider;
        protected int _indentLevel;
        protected TextWriter _textWriter;

        protected LinkedList<object> _scopes;

        protected Stream _stream;


        public FileLogger(ILoggerProvider provider, FileLoggerOptions options, string categoryName)
        {
            _scopeLock = new object();
            _lock = new object();

            _logLevel = LogLevel.Trace;
            _provider = provider;
            _indentLevel = 0;
            _scopes = new LinkedList<object>();
            // _textWriter = System.Console.Out;

            string? logDir = Path.GetDirectoryName(options.LogFilePath);
            if (!string.IsNullOrEmpty(logDir))
            {
                if (!Directory.Exists(logDir))
                {
                    Directory.CreateDirectory(logDir);
                }

                _stream = File.Open(options.LogFilePath, FileMode.Append,
                    FileAccess.Write, FileShare.Read);
                _textWriter = new StreamWriter(_stream, System.Text.Encoding.UTF8);
                _textWriter.Flush();
                _stream.Flush();
            }
        }  


        protected void WriteIndent()
        {
            _textWriter.Write(new string(' ', _indentLevel * NU_INDENT_SPACES));
        } 


        System.IDisposable ILogger.BeginScope<TState>(TState state)
        {
            FileLoggerScope<TState>? scope = null;

            lock (_lock)
            {
                scope = new FileLoggerScope<TState>(this, state);
                _scopes.AddFirst(scope);

                _indentLevel++;
                WriteIndent();
                _textWriter.Write("BeginScope<TState>: ");
                _textWriter.WriteLine(state);
                _indentLevel++;

                // _provider.ScopeProvider.Push(state);
                // throw new System.NotImplementedException();

                _textWriter.Flush();
                _stream.Flush();
            }

            return scope;
        }


        public void EndScope<TState>(TState scopeName)
        {
            lock (_lock)
            {
                // FooLoggerScope<TState> scope = (FooLoggerScope<TState>)_scopes.First.Value;
                _indentLevel--;

                WriteIndent();
                _textWriter.Write("EndScope ");
                // _textWriter.WriteLine(scope.ScopeName);
                _textWriter.WriteLine(scopeName);

                _indentLevel--;
                _scopes.RemoveFirst();

                _textWriter.Flush();
                _stream.Flush();
            }
        }


        bool ILogger.IsEnabled(LogLevel logLevel)
        {
            return logLevel >= _logLevel;
        }  


        void ILogger.Log<TState>(
              LogLevel logLevel
            , EventId eventId
            , TState state
            , Exception? exception
            , Func<TState, Exception, string> formatter)
        {

            lock (_lock)
            {
                WriteIndent();
                _textWriter.Write($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} {logLevel}: {state}");
                _textWriter.WriteLine();
                _textWriter.Flush();
                _stream.Flush();

                Exception? currentException = exception;

                while (currentException != null)
                {
                    WriteIndent();
                    _textWriter.Write($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} {logLevel} Exception: {currentException.Message}");
                    WriteIndent();
                    _textWriter.Write($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} {logLevel} StackTrace: {currentException.StackTrace}");
                    _textWriter.Flush();
                    _stream.Flush();

                    currentException = currentException.InnerException;
                } 

            } 

        } 


        void System.IDisposable.Dispose()
        {
            _textWriter.Flush();
            _stream.Flush();
            _textWriter.Close();
            _stream.Close();
        }  


}