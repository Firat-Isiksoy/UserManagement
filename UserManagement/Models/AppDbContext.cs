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
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserModel>()
                .HasIndex(u => u.Email)
                .IsUnique();
            modelBuilder.Entity<MovieModel>()
                 .HasOne(m => m.Category)     
                 .WithMany(c => c.Movies)     
                 .HasForeignKey(m => m.CategoryId); 
        }
    }
}