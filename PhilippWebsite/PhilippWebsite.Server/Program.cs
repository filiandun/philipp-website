using PhilippWebsite.Server.Services.ProjectTree;
using PhilippWebsite.Server.Services.ProjectTree.GitHub;
using PhilippWebsite.Server.Services.ProjectTree.Local;


namespace PhilippWebsite.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            // 
            builder.Services.AddHttpClient<GitHubTreeProvider>(client =>
            {
                client.BaseAddress = new Uri("https://api.github.com/");
                client.DefaultRequestHeaders.Add("User-Agent", "PhilippWebsite");
            });

            builder.Services.AddHttpClient<LocalTreeProvider>(client =>
            {
                client.BaseAddress = new Uri("https://api.github.com/");
                client.DefaultRequestHeaders.Add("User-Agent", "PhilippWebsite");
            });

            builder.Services.AddSingleton<ITreeProvider, LocalTreeProvider>();
            builder.Services.AddSingleton<ITreeProvider, GitHubTreeProvider>();

            builder.Services.AddSingleton<IProjectTreeService, ProjectTreeService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
