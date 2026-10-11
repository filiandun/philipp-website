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
            builder.Services.AddOpenApi();


            // CORS
            var allowedOrigins = builder.Configuration.GetSection("AllowedOrigins").Get<string[]>() ?? Array.Empty<string>();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowLocalFront", policy =>
                {
                    policy.WithOrigins(allowedOrigins).AllowAnyHeader().AllowAnyMethod();
                });
            });


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


            // CORS
            app.UseCors("AllowLocalFront");

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
