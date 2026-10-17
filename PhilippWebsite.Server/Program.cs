using PhilippWebsite.Server.Services.EditorContent;

using PhilippWebsite.Server.Services.FileContent;
using PhilippWebsite.Server.Services.FileContent.FileSource;

using PhilippWebsite.Server.Services.ProjectTree;
using PhilippWebsite.Server.Services.ProjectTree.GitHub;
using PhilippWebsite.Server.Services.ProjectTree.Local;
using PhilippWebsite.Server.Services.SolutionState;
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


            // Services

            builder.Services.AddScoped<ITreeProvider, LocalTreeProvider>();

            builder.Services.AddHttpClient<ITreeProvider, GitHubTreeProvider>(client =>
            {
                client.BaseAddress = new Uri("https://api.github.com/");
                client.DefaultRequestHeaders.Add("User-Agent", "PhilippWebsite");
            });

            builder.Services.AddScoped<IProjectTreeService, ProjectTreeService>();

            builder.Services.AddScoped<ISolutionStateService, SolutionStateService>();


            builder.Services.AddScoped<IFileSource, LocalFileSource>();
            builder.Services.AddHttpClient<IFileSource, GitHubFileSource>();
            builder.Services.AddScoped<IFileContentService, FileContentService>();

            builder.Services.AddScoped<ISyntaxHighlighterService, SyntaxHighlighterService>();

            builder.Services.AddScoped<IEditorContentService, EditorContentService>();


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
