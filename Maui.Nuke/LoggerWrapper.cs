using Microsoft.Extensions.Logging;

namespace Maui.Nuke;

internal class LoggerWrapper
{
    private readonly ILogger? _logger;

    public LoggerWrapper(ILogger? logger)
    {
        _logger = logger;
    }

    public void Debug(Func<string> messageFunc)
    {
        if (_logger == null || !_logger.IsEnabled(LogLevel.Debug))
        {
            return;
        }

        _logger.LogDebug(messageFunc());
    }

    public void Warn(string message)
    {
        _logger?.LogWarning(message);
    }
}
