using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

using MudBlazor.Services;

using PhilippWebsite.Services.FileContent;
using PhilippWebsite.Services.ProjectTree;
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

            builder.Services.Configure<GitHubApiConfig>(builder.Configuration.GetSection("GitHubApi"));
            builder.Services.Configure<GitHubRawConfig>(builder.Configuration.GetSection("GitHubRaw"));

            GitHubApiConfig? gitHubApiConfig = builder.Configuration.GetSection("GitHubApi").Get<GitHubApiConfig>();

            if (gitHubApiConfig is null || string.IsNullOrEmpty(gitHubApiConfig.SolutionName) || string.IsNullOrEmpty(gitHubApiConfig.BaseUrl) || gitHubApiConfig.RepoList.Count == 0)
            {
                throw new InvalidOperationException("GitHub API config not found or section is empty");
            }

            GitHubRawConfig? gitHubRawConfig = builder.Configuration.GetSection("GitHubRaw").Get<GitHubRawConfig>();

            if (gitHubRawConfig is null || string.IsNullOrEmpty(gitHubRawConfig.BaseUrl))
            {
                throw new InvalidOperationException("GitHub Raw config not found or section is empty");
            }


            // services

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            builder.Services.AddScoped<IProjectTreeService, GitHubProjectTreeService>();
            builder.Services.AddScoped<IFileContentService, GitHubFileContentService>();

            builder.Services.AddScoped<TabsBarStateService>();



            await builder.Build().RunAsync();
        }
    }
}
