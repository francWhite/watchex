using System.ComponentModel;
using Spectre.Console.Cli;

namespace Watchex;

internal class WatchCommandSettings : CommandSettings
{
  [Description("Prints verbose log output")]
  [CommandOption("-v|--verbose")]
  public bool Verbose { get; init; }

  [Description("Prints version information")]
  [CommandOption("-V|--version")]
  public bool PrintVersion { get; init; }

  [Description("Path to the project file. Defaults to the first *.csproj file found in the current directory.")]
  [CommandOption("-p|--project <PROJECT>")]
  public string? ProjectFile { get; init; }
}