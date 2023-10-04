// ReSharper disable RedundantNullableFlowAttribute

using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Spectre.Console;
using Spectre.Console.Cli;
using Watchex.Logging;

namespace Watchex;

internal class WatchCommand : Command<WatchCommandSettings>
{
  public override int Execute([NotNull] CommandContext context, [NotNull] WatchCommandSettings settings)
  {
    if (settings.PrintVersion)
    {
      PrintVersion();
      return 0;
    }

    var rootProjectPath = GetProjectPath(settings);
    if (rootProjectPath == null)
    {
      AnsiConsole.MarkupLine("[red]No valid project file found, exiting...[/]");
      return 1;
    }

    var logger = new ConsoleLogger(settings.Verbose);
    var projectEvaluator = new ProjectEvaluator(logger);
    var fileWatcher = new FileWatcher(logger);

    var filesToWatch = projectEvaluator.EvaluateFilesCopiedToOutput(rootProjectPath);
    fileWatcher.Watch(filesToWatch);

    AnsiConsole.MarkupLine("Press enter to [olive]exit[/]");
    Console.ReadLine();
    return 0;
  }

  private static string? GetProjectPath(WatchCommandSettings settings)
  {
    if (File.Exists(settings.ProjectFile) && Path.GetExtension(settings.ProjectFile) == ".csproj")
      return settings.ProjectFile;

    var currentDirectoryProject = Directory
      .GetFiles(Directory.GetCurrentDirectory(), "*.csproj")
      .FirstOrDefault();

    return File.Exists(currentDirectoryProject) && Path.GetExtension(currentDirectoryProject) == ".csproj"
      ? currentDirectoryProject
      : null;
  }

  private static void PrintVersion()
  {
    var version = Assembly
      .GetExecutingAssembly()
      .GetCustomAttribute<AssemblyInformationalVersionAttribute>()?
      .InformationalVersion ?? "dev";

    AnsiConsole.MarkupLine($"[bold]watchex[/] version [cyan]{version}[/]");
  }
}