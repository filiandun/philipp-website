using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

using MudBlazor.Services;

using PhilippWebsite.Services.FileContent;

using PhilippWebsite.Services.SolutionExplorer;
using PhilippWebsite.Services.SolutionExplorer.Github;
using PhilippWebsite.Services.SolutionExplorer.Local;

using PhilippWebsite.Services.TabsBarState;


namespace PhilippWebsite
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);

            builder.Logging.SetMinimumLevel(LogLevel.Debug);

            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");

            builder.Services.AddMudServices();


            // configs

            builder.Services.Configure<GitHubTreeConfig>(builder.Configuration.GetSection("GitHubTreeSource"));
            builder.Services.Configure<LocalTreeConfig>(builder.Configuration.GetSection("LocalTreeSource"));

            builder.Services.Configure<FileContentConfig>(builder.Configuration.GetSection("FileContentSource"));

            GitHubTreeConfig? gitHubTreeConfig = builder.Configuration.GetSection("GitHubTreeSource").Get<GitHubTreeConfig>();

            if (gitHubTreeConfig is null || string.IsNullOrEmpty(gitHubTreeConfig.SolutionName) || string.IsNullOrEmpty(gitHubTreeConfig.BaseUrl) || gitHubTreeConfig.RepoList.Count == 0)
            {
                throw new InvalidOperationException("GitHub Tree config not found or section is empty");
            }

            LocalTreeConfig? localTreeConfig = builder.Configuration.GetSection("LocalTreeSource").Get<LocalTreeConfig>();

            if (localTreeConfig is null || string.IsNullOrEmpty(localTreeConfig.SolutionName) || string.IsNullOrEmpty(localTreeConfig.BaseUrl) || localTreeConfig.FileList.Count == 0)
            {
                throw new InvalidOperationException("Local Tree config not found or section is empty");
            }


            FileContentConfig? fileContentConfig = builder.Configuration.GetSection("FileContentSource").Get<FileContentConfig>();

            if (fileContentConfig is null || string.IsNullOrEmpty(fileContentConfig.GitHubUrl) || string.IsNullOrEmpty(fileContentConfig.LocalUrl))
            {
                throw new InvalidOperationException("File Content config not found or section is empty");
            }


            // services

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            builder.Services.AddScoped<ITreeProvider, GitHubTreeProvider>();
            builder.Services.AddScoped<ITreeProvider, LocalTreeProvider>();
            builder.Services.AddScoped<SolutionExplorerService>();

            builder.Services.AddScoped<IFileContentService, FileContentService>();
            builder.Services.AddScoped<IFileContentService, FileContentService>();

            builder.Services.AddScoped<TabsBarStateService>();



            await builder.Build().RunAsync();
        }
    }
}
