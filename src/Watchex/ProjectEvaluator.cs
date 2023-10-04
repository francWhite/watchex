using Microsoft.Build.Evaluation;

namespace Watchex;

internal static class ProjectEvaluator
{
  public static IEnumerable<CopyFileInfo> EvaluateFilesCopiedToOutput(
    string rootProjectPath)
  {
    var (projectCollection, rootProject) = LoadProjectCollection(rootProjectPath);

    var copyToOutputItems = projectCollection.LoadedProjects
      .SelectMany(p => p.Items.Where(CopyItemToOutputDirectory));

    var outputDirectory = Path.Combine(rootProject.DirectoryPath, rootProject.GetPropertyValue("OutputPath"));

    return copyToOutputItems
      .Select(item => CreateOutputFileInfo(item, outputDirectory));
  }

  private static CopyFileInfo CreateOutputFileInfo(ProjectItem projectItem, string outputDirectory)
  {
    var sourcePath = Path.Combine(projectItem.Project.DirectoryPath, projectItem.EvaluatedInclude);
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
}