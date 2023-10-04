using System.Reactive.Linq;
using Watchex.Logging;

namespace Watchex;

internal class FileWatcher
{
  private readonly IConsoleLogger _logger;

  public FileWatcher(IConsoleLogger logger)
  {
    _logger = logger;
  }

  public void Watch(IEnumerable<CopyFileInfo> filesToWatch)
  {
    _logger.LogDebug("creating file system watchers...");
    var watchers = filesToWatch.Select(CreateFileSystemWatcher).ToList();
    watchers.ForEach(w => w.EnableRaisingEvents = true);

    _logger.LogInfo($"watching [bold]{watchers.Count}[/] files for changes...");
  }

  private FileSystemWatcher CreateFileSystemWatcher(CopyFileInfo fileInfo)
  {
    if (fileInfo.Source.DirectoryName == null)
      throw new InvalidOperationException($"Could not get directory name for {fileInfo.Source}");

    _logger.LogDebug($"  [grey]{fileInfo.Source.FullName}[/] -> [grey]{fileInfo.Destination.FullName}[/]");

    var watcher = new FileSystemWatcher(fileInfo.Source.DirectoryName, fileInfo.Destination.Name);
    watcher.NotifyFilter = NotifyFilters.LastWrite;

    Observable
      .FromEventPattern<FileSystemEventArgs>(watcher, nameof(watcher.Changed))
      .Select(e => e.EventArgs)
      .Throttle(TimeSpan.FromMilliseconds(10))
      .Subscribe(e => HandleChangedEvent(e, fileInfo));

    return watcher;
  }

  private void HandleChangedEvent(FileSystemEventArgs args, CopyFileInfo copyFileInfo)
  {
    _logger.LogDebug($"file [grey]{args.FullPath}[/] changed");

    if (!string.IsNullOrWhiteSpace(copyFileInfo.Destination.DirectoryName))
      Directory.CreateDirectory(copyFileInfo.Destination.DirectoryName);

    using var inStream = new FileStream(copyFileInfo.Source.FullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
    using var outStream = new FileStream(copyFileInfo.Destination.FullName, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
    inStream.CopyTo(outStream);
  }
}