using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

using MudBlazor.Services;

using PhilippWebsite.Client.Services.CodeHighlighter;
using PhilippWebsite.Client.Services.FileContent;
using PhilippWebsite.Client.Services.SolutionExplorer;
using PhilippWebsite.Client.Services.TabsBarState;


namespace PhilippWebsite.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);

            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");

            builder.Services.AddMudServices();


            // api clients

            string apiBaseUrl = builder.Configuration["ApiBaseUrl"] ?? throw new InvalidOperationException("Missing configuration: 'ApiBaseUrl' is not set in appsettings.json.");

            builder.Services.AddHttpClient<IProjectTreeClient, ProjectTreeClient>(client => client.BaseAddress = new Uri(apiBaseUrl));

            builder.Services.AddScoped<IFileContentClient, FileContentClient>();


            // services

            builder.Services.AddScoped<ISolutionExplorerService, SolutionExplorerService>();

            builder.Services.AddScoped<TabsBarStateService>();

            builder.Services.AddSingleton<ICodeHighlighterService, ShikiCodeHighlighterService>();


            // preloads

            WebAssemblyHost host = builder.Build();

            ICodeHighlighterService codeHighlighter = host.Services.GetRequiredService<ICodeHighlighterService>();
            _ = codeHighlighter.PreloadAsync();


            await host.RunAsync();
        }
    }
}
