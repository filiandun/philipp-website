using Microsoft.EntityFrameworkCore;

using PhilippWebsite.Server.Models;


namespace PhilippWebsite.Server.Infrastructure
{
    public class RepositoryDbContext : DbContext
    {
        public DbSet<Repository> Repositories { get; set; }
        public DbSet<FileNode> FileNodes { get; set; }
        public DbSet<FileContent> FileContents { get; set; }


        public RepositoryDbContext(DbContextOptions<RepositoryDbContext> options) : base(options) { }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FileNode>().HasOne(n => n.Parent).WithMany(n => n.Children).HasForeignKey(n => n.ParentId).OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<FileContent>().HasKey(c => c.NodeId);

            modelBuilder.Entity<FileContent>().HasOne(c => c.Node).WithOne(n => n.Content).HasForeignKey<FileContent>(c => c.NodeId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
