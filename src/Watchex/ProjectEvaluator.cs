using Microsoft.Build.Evaluation;
using Watchex.Logging;

namespace Watchex;

internal class ProjectEvaluator
{
  private readonly IConsoleLogger _logger;

  public ProjectEvaluator(IConsoleLogger logger)
  {
    _logger = logger;
  }

  public IEnumerable<CopyFileInfo> EvaluateFilesCopiedToOutput(
    string rootProjectPath)
  {
    _logger.LogInfo($"evaluating project [bold]{rootProjectPath}[/]");

    var (projectCollection, rootProject) = LoadProjectCollection(rootProjectPath);
    _logger.LogDebug($"loaded [bold]{projectCollection.LoadedProjects.Count}[/] projects");

    var outputDirectory = Path.Combine(rootProject.DirectoryPath, rootProject.GetPropertyValue("OutputPath"));

    var copyToOutputItems = projectCollection.LoadedProjects
      .SelectMany(p => p.Items.Where(CopyItemToOutputDirectory))
      .ToList();

    _logger.LogDebug(
      $"found [bold]{copyToOutputItems.Count}[/] files marked for copy to output directory ([grey]{outputDirectory}[/]): ");
    _logger.LogDebug(
      string.Join(
        Environment.NewLine,
        copyToOutputItems.Select(
          i => $"  [grey]{i.Project.GetPropertyValue("ProjectName")} <{i.ItemType}> {i.EvaluatedInclude}[/]")));

    return copyToOutputItems
      .Select(item => CreateOutputFileInfo(item, outputDirectory));
  }

  private static CopyFileInfo CreateOutputFileInfo(ProjectItem projectItem, string outputDirectory)
  {
    var sourcePath = EvaluatedPath(projectItem);
    var outputPath = Path.Combine(outputDirectory, projectItem.EvaluatedInclude);
    return new CopyFileInfo(new FileInfo(sourcePath), new FileInfo(outputPath));
  }

  private static (ProjectCollection, Project) LoadProjectCollection(string rootProjectPath)
  {
    var projectCollection = new ProjectCollection();
    var rootProject = projectCollection.LoadProject(rootProjectPath);
    LoadProjectsRecursive(projectCollection, rootProject);

    return (projectCollection, rootProject);
  }

  private static void LoadProjectsRecursive(ProjectCollection projectCollection, Project project)
  {
    var directProjectReferences = project.GetItems("ProjectReference")
      .Select(x => x.EvaluatedInclude)
      .Select(p => Path.GetFullPath(p, project.DirectoryPath))
      .Where(path => projectCollection.LoadedProjects.Any(p => p.FullPath == path) == false)
      .Select(projectCollection.LoadProject)
      .ToList();

    directProjectReferences.ForEach(p => LoadProjectsRecursive(projectCollection, p));
  }

  private static bool CopyItemToOutputDirectory(ProjectItem item)
  {
    var metaData = item.Metadata.SingleOrDefault(m => m.Name == "CopyToOutputDirectory");
    return metaData != null && metaData.EvaluatedValue != "Never";
  }

  private static string EvaluatedPath(ProjectItem item) =>
    Path.Combine(item.Project.DirectoryPath, item.EvaluatedInclude);
}