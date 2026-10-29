using Microsoft.EntityFrameworkCore;

using PhilippWebsite.Server.Infrastructure;
using PhilippWebsite.Server.Infrastructure.Storage;
using PhilippWebsite.Server.Options;
using PhilippWebsite.Server.Services.EditorContent;

using PhilippWebsite.Server.Services.FileContent;
using PhilippWebsite.Server.Services.FileContent.FileSource;
using PhilippWebsite.Server.Services.RepositorySync;
using PhilippWebsite.Server.Services.SolutionState;

using PhilippWebsite.Server.Services.SolutionTree;
using PhilippWebsite.Server.Services.SolutionTree.TreeProvider;
using PhilippWebsite.Server.Services.SolutionTree.TreeProvider.GitHub;
using PhilippWebsite.Server.Services.SolutionTree.TreeProvider.Local;

using PhilippWebsite.Server.Services.SyntaxHighlighter;


namespace PhilippWebsite.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            builder.Services.AddControllers();
            builder.Services.AddOpenApi();

            builder.Services.AddMemoryCache();


            // CORS

            var allowedOrigins = builder.Configuration.GetSection("AllowedOrigins").Get<string[]>() ?? Array.Empty<string>();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowLocalFront", policy =>
                {
                    policy.WithOrigins(allowedOrigins).AllowAnyHeader().AllowAnyMethod();
                });
            });


            // Database

            builder.Services.AddDbContext<RepositoryDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DatabaseConnection") ?? throw new InvalidOperationException("Connection string 'DatabaseConnection' not found")));

            builder.Services.AddScoped<RepositoryStorage>();

            builder.Services.AddScoped<IRepositoryReader>(provider => provider.GetRequiredService<RepositoryStorage>());
            builder.Services.AddScoped<IRepositoryWriter>(provider => provider.GetRequiredService<RepositoryStorage>());


            // Options

            builder.Services.AddOptions<LocalContentOptions>().Bind(builder.Configuration.GetSection("LocalContent")).ValidateDataAnnotations().ValidateOnStart();
            builder.Services.AddOptions<GitHubContentOptions>().Bind(builder.Configuration.GetSection("GitHubContent")).ValidateDataAnnotations().ValidateOnStart();

            builder.Services.AddOptions<SolutionStateOptions>().Bind(builder.Configuration.GetSection("SolutionState")).ValidateDataAnnotations().ValidateOnStart();


            // Services

            // cache

            builder.Services.AddHttpClient<IRepositorySyncService, GitHubSyncService>(httpClient =>
            {
                httpClient.BaseAddress = new Uri("https://api.github.com/");
                httpClient.DefaultRequestHeaders.Add("User-Agent", "PhilippWebsite");
            });

            builder.Services.AddHttpClient<GitHubContentService>(httpClient =>
            {
                httpClient.DefaultRequestHeaders.Add("User-Agent", "PhilippWebsite");
            });

            // tree

            builder.Services.AddScoped<ITreeProvider, LocalTreeProvider>();

            builder.Services.AddScoped<ITreeProvider, GitHubTreeProvider>();

            builder.Services.AddScoped<ISolutionTreeService, SolutionTreeService>();

            // file content

            builder.Services.AddScoped<IFileSource, LocalFileSource>();
            builder.Services.AddHttpClient<IFileSource, GitHubFileSource>();
            builder.Services.AddScoped<IFileContentService, FileContentService>();

            builder.Services.AddScoped<ISyntaxHighlighterService, SyntaxHighlighterService>();

            builder.Services.AddScoped<IEditorContentService, EditorContentService>();

            // state

            builder.Services.AddScoped<ISolutionStateService, SolutionStateService>();


            var app = builder.Build();


            // CORS

            app.UseCors("AllowLocalFront");

            // Configure the HTTP request pipeline.

            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.UseHttpsRedirection();
            }


            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
