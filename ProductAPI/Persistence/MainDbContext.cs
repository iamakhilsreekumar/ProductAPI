using Microsoft.EntityFrameworkCore;
using ProductAPI.Entities;

namespace ProductAPI.Persistence
{
    public class MainDbContext : DbContext, IMainDbContext
    {

        public MainDbContext(DbContextOptions<MainDbContext> options) : base(options)
        {
            Database.EnsureCreatedAsync();
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            foreach (var item in ChangeTracker.Entries<IEntity>().AsEnumerable())
            {
                //Auto Timestamp
                item.Entity.CreatedAt = DateTime.Now;
            }
            return base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var configuration = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json")
               .Build();

            var connectionString = configuration.GetConnectionString("MainConnectionString");
            optionsBuilder.UseSqlite(connectionString);
            SQLitePCL.Batteries.Init();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
                .HasIndex(b => b.ProductId)
                .IsUnique();

            modelBuilder.Entity<Product>()
                .Property(p => p.ProductId).ValueGeneratedOnAdd();

            modelBuilder.Entity<Product>().HasData(
                new Product { Id = 1, ProductId = 6, Name = "Product 1", Description = "Description for Product 1", Price = 9.99m, StockAvailable = 100 });


        }
    }
}
