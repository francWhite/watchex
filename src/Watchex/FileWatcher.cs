namespace Watchex;

internal static class FileWatcher
{
  public static void Watch(IEnumerable<CopyFileInfo> filesToWatch)
  {
    var watchers = filesToWatch.Select(CreateFileSystemWatcher).ToList();
    watchers.ForEach(w => w.EnableRaisingEvents = true);
  }

  private static FileSystemWatcher CreateFileSystemWatcher(CopyFileInfo fileInfo)
  {
    Console.WriteLine($"Start watching file: {fileInfo.Source}");

    if (fileInfo.Source.DirectoryName == null)
      throw new InvalidOperationException($"Could not get directory name for {fileInfo.Source}");

    var watcher = new FileSystemWatcher(fileInfo.Source.DirectoryName, fileInfo.Destination.Name);
    watcher.NotifyFilter = NotifyFilters.LastWrite;
    watcher.Changed += (_, args) => HandleChangedEvent(args, fileInfo);
    return watcher;
  }

  private static void HandleChangedEvent(FileSystemEventArgs args, CopyFileInfo copyFileInfo)
  {
    Console.WriteLine($"File {args.FullPath} changed");
    File.Copy(copyFileInfo.Source.FullName, copyFileInfo.Destination.FullName, true);
  }
}