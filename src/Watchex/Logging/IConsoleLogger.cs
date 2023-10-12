namespace Watchex.Logging;

internal interface IConsoleLogger
{
  void LogInfo(string markup);
  void LogDebug(string markup);
  void LogError(Exception exception, string message);
}