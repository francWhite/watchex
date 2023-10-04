using Spectre.Console;

namespace Watchex.Logging;

internal class ConsoleLogger : IConsoleLogger
{
  private readonly bool _verbose;

  public ConsoleLogger(bool verbose)
  {
    _verbose = verbose;
  }

  public void LogInfo(string markup)
  {
    AnsiConsole.MarkupLine(markup);
  }

  public void LogDebug(string markup)
  {
    if (!_verbose) return;
    AnsiConsole.MarkupLine(markup);
  }
}