// using Microsoft.EntityFrameworkCore;
// using project.Models;

// namespace project.Data
// {
//     public class AppDbContext : DbContext
//     {
//         public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

//         public AppDbContext()
//         {
//         }

//         protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//         {
//             if (!optionsBuilder.IsConfigured)
//             {
//                 optionsBuilder.UseSqlServer("Server=DESKTOP-SSNMLFD;Database=UsersAndItemsDB;Trusted_Connection=True;MultipleActiveResultSets=true");
//             }
//         }
//         public DbSet<AuthorDb> Authors { get; set; }
//         public DbSet<BookDb> Books { get; set; }

//         protected override void OnModelCreating(ModelBuilder modelBuilder)
//         {
//             modelBuilder.Entity<AuthorDb>()
//                 .ToTable("Authors") // טבלה למחברים
//                 .HasKey(a => a.Id);

//             modelBuilder.Entity<BookDb>()
//                 .ToTable("Books") // טבלה לספרים
//                 .HasKey(b => b.Id);

//             modelBuilder.Entity<BookDb>()
//                 .HasOne(b => b.Author) // קשר בין ספר למחבר
//                 .WithMany(a => a.Books) // מחבר יכול לכתוב כמה ספרים
//                 .HasForeignKey(b => b.AuthorId) // המפתח הזר לספרים
//                 .OnDelete(DeleteBehavior.Cascade); // אם מחבר נמחק - מחק גם את הספרים שלו
//         }
//     }
// }
using Microsoft.EntityFrameworkCore;
using project.Models;

namespace project.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<AuthorDb> Authors { get; set; }
        public DbSet<BookDb> Books { get; set; }
  /// protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//         {
//             if (!optionsBuilder.IsConfigured)
//             {
//                 optionsBuilder.UseSqlServer("Server=DESKTOP-SSNMLFD;Database=UsersAndItemsDB;Trusted_Connection=True;MultipleActiveResultSets=true");
//             }
//         }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AuthorDb>()
                .ToTable("Authors")
                .HasKey(a => a.Id);

            modelBuilder.Entity<BookDb>()
                .ToTable("Books")
                .HasKey(b => b.Id);

            modelBuilder.Entity<BookDb>()
                .HasOne(b => b.Author)
                .WithMany(a => a.Books)
                .HasForeignKey(b => b.AuthorId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
