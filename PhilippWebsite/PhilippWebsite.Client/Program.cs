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

            // services

            string? apiBaseUrl = builder.Configuration.GetValue<string>("ApiBaseUrl") ?? string.Empty;

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(apiBaseUrl) });

            builder.Services.AddScoped<ISolutionExplorerService, SolutionExplorerService>();

            builder.Services.AddScoped<TabsBarStateService>();

            builder.Services.AddScoped<IFileContentService, FileContentService>();
            builder.Services.AddScoped<IFileContentService, FileContentService>();

            builder.Services.AddSingleton<ICodeHighlighterService, ShikiCodeHighlighterService>();


            // preloads

            WebAssemblyHost host = builder.Build();

            ICodeHighlighterService codeHighlighter = host.Services.GetRequiredService<ICodeHighlighterService>();
            _ = codeHighlighter.PreloadAsync();


            await host.RunAsync();
        }
    }
}
