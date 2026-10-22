using PhilippWebsite.Client.Resources;

using PhilippWebsite.Shared.SolutionState;


namespace PhilippWebsite.Client.Services
{
    public static class IconResolver
    {
        public static string GetIcon(SolutionTreeItem item)
        {
            return item.Type switch
            {
                SolutionTreeItemType.Solution => VsIcons.SolutionExplorer.Solutions.Solution,

                SolutionTreeItemType.PortfolioSolutionFolder => VsIcons.SolutionExplorer.Solutions.Portfolio,
                SolutionTreeItemType.PetProjectsSolutionFolder => VsIcons.SolutionExplorer.Solutions.PetProjectV2,

                SolutionTreeItemType.Project => VsIcons.SolutionExplorer.Projects.Project,
                SolutionTreeItemType.AspProject => VsIcons.SolutionExplorer.Projects.Asp,
                SolutionTreeItemType.WpfProject => VsIcons.SolutionExplorer.Projects.Wpf,
                SolutionTreeItemType.ExtensionProject => VsIcons.SolutionExplorer.Projects.Extension,

                SolutionTreeItemType.Folder => GetFolderIcon(item.Name),
                SolutionTreeItemType.File => GetFileIcon(item.Name),

                _ => VsIcons.SolutionExplorer.Files.File
            };
        }


        private static string GetFolderIcon(string folderName)
        {
            return folderName switch
            {
                "wwwroot" => VsIcons.SolutionExplorer.Folders.Wwwroot,
                "Properties" => VsIcons.SolutionExplorer.Folders.Properties,

                _ => VsIcons.SolutionExplorer.Folders.Folder
            };
        }

        private static string GetFileIcon(string fileName)
        {
            string fileExtension = Path.GetExtension(fileName).ToLowerInvariant();

            return fileExtension switch
            {
                ".cs" => VsIcons.SolutionExplorer.Files.CSharp,
                ".razor" => VsIcons.SolutionExplorer.Files.Razor,

                ".json" => VsIcons.SolutionExplorer.Files.Json,
                ".md" or ".markdown" => VsIcons.SolutionExplorer.Files.Markdown,

                ".html" => VsIcons.SolutionExplorer.Files.Html,
                ".css" => VsIcons.SolutionExplorer.Files.Css,

                ".js" => VsIcons.SolutionExplorer.Files.JavaScript,
                ".ts" => VsIcons.SolutionExplorer.Files.TypeScript,

                ".ico" => VsIcons.SolutionExplorer.Files.Icon,

                ".png" or ".jpg" or ".jpeg" or ".svg" => VsIcons.SolutionExplorer.Files.Image,

                _ => VsIcons.SolutionExplorer.Files.File
            };
        }
    }
}
