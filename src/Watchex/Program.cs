using Microsoft.Build.Locator;
using Watchex;

MSBuildLocator.RegisterDefaults();

var currentDirectoryProject = Directory
  .GetFiles(Directory.GetCurrentDirectory(), "*.csproj")
  .SingleOrDefault();

var rootProjectPath = args.Any() && File.Exists(args[0])
  ? args[0]
  : currentDirectoryProject
    ?? throw new InvalidOperationException("Could not find project file");

Console.WriteLine($"Evaluating project tree for {rootProjectPath}...");

var filesToWatch = ProjectEvaluator.EvaluateFilesCopiedToOutput(rootProjectPath);
FileWatcher.Watch(filesToWatch);

Console.WriteLine("Press any key to exit");
Console.ReadLine();