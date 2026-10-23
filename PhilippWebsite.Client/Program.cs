using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

using PhilippWebsite.Client.Clients.EditorContent;
using PhilippWebsite.Client.Clients.SolutionState;

using PhilippWebsite.Client.Services.EditorContent;
using PhilippWebsite.Client.Services.SolutionExplorer;
using PhilippWebsite.Client.Services.SolutionState;
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


            // api clients

            string apiBaseUrl = builder.Configuration["ApiBaseUrl"] ?? throw new InvalidOperationException("Missing configuration: 'ApiBaseUrl' is not set in appsettings.json.");

            builder.Services.AddHttpClient<ISolutionStateClient, SolutionStateClient>(client => client.BaseAddress = new Uri(apiBaseUrl));

            builder.Services.AddHttpClient<IEditorContentClient, EditorContentClient>(client => client.BaseAddress = new Uri(apiBaseUrl));


            // services

            builder.Services.AddScoped<ISolutionStateInitializer, SolutionStateInitializer>();

            builder.Services.AddScoped<ISolutionExplorerService, SolutionExplorerService>();

            builder.Services.AddScoped<TabsBarStateService>();

            builder.Services.AddScoped<IEditorContentService, EditorContentService>();


            // init

            var host = builder.Build();

            await host.RunAsync();
        }
    }
}
