using Microsoft.EntityFrameworkCore;

namespace UserManagement.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<UserModel> Users { get; set; }
        public DbSet<CategoryModel> Categories { get; set; }
        public DbSet<MovieModel> Movies { get; set; }
        public DbSet<MovieRating> MovieRatings { get; set; }
        public DbSet<WatchlistModel> Watchlists { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserModel>()
                .HasIndex(u => u.Email)
                .IsUnique();
            modelBuilder.Entity<MovieModel>()
                .HasOne(m => m.Category) 
                .WithMany()
                .HasForeignKey(m => m.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<MovieRating>()
                   .HasOne<MovieModel>()
                   .WithMany(m => m.MovieRatings) 
                   .HasForeignKey(r => r.MovieId)
                   .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<MovieRating>()
                 .HasOne(r => r.User)
                 .WithMany()
                 .HasForeignKey(r => r.UserId)
                 .OnDelete(DeleteBehavior.Restrict);
        }
    }
}