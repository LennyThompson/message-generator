# FileLogger

Since the Microsoft logger (Microsoft.Extension.Logging namespace) doesnt include a file logger (WTAF??) this is someone elses opinion of the compatible file logger.

See [stackoverflow - file logger](https://stackoverflow.com/questions/40073743/how-to-log-to-a-file-without-using-third-party-logger-in-net-core)

This is a quick a dirty implementation that really only provides a framework to be improved upon.

TODO:

1. Configure using *appsettings.json*
2. Add custom formatter, or reuse the default
3. Find the name of the source and add the logger
4. Decide when to start a new log file, and how


## FileLoggerExtensions

Added *FileLoggerExtensions* to extends the *ILoggingBuilder* to add a file logger provider to the host builder.

### Usage

Use the *HostApplicationBuilder*,  *LoggingBuilder* to add the *FileLoggerProvider*

For instance

``` charp
        HostApplicationBuilder builder = Host.CreateApplicationBuilder();
        builder.Logging.AddFileLogger(config => config.LogFilePath = "./code-generate.log");
```