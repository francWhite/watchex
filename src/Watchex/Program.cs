using Microsoft.Build.Locator;
using Spectre.Console.Cli;
using Watchex;

MSBuildLocator.RegisterDefaults();

var app = new CommandApp<WatchCommand>();
app.Run(args);