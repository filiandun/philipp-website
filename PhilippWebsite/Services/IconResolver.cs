using PhilippWebsite.Resources;


namespace PhilippWebsite.Services
{
    public static class IconResolver
    {
        public static string GetSolutionIcon(string solutionName)
        {
            return solutionName switch
            {
                "About Philipp" => VsIcons.SolutionExplorer.Solutions.Portfolio,
                "Philipp's pet projects" => VsIcons.SolutionExplorer.Solutions.PetProjectV2,

                _ => VsIcons.SolutionExplorer.Solutions.Solution
            };
        }

        public static string GetProjectIcon(string projectName)
        {
            return projectName switch
            {
                "philipp-website" => VsIcons.SolutionExplorer.Projects.Asp,
                "recbinder" => VsIcons.SolutionExplorer.Projects.Wpf,
                "docx-markdown-editor" => VsIcons.SolutionExplorer.Projects.Extension,

                _ => VsIcons.SolutionExplorer.Projects.Project
            };
        }

        public static string GetFolderIcon(string folderName)
        {
            return folderName switch
            {
                "wwwroot" => VsIcons.SolutionExplorer.Folders.Wwwroot,
                "Properties" => VsIcons.SolutionExplorer.Folders.Properties,

                _ => VsIcons.SolutionExplorer.Folders.Folder
            };
        }

        public static string GetFileIcon(string fileName)
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
