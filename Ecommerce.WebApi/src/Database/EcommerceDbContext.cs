using Ecommerce.Core.src.Entity;
using Ecommerce.Core.src.ValueObject;
using Ecommerce.WebApi.src.Data.Interceptors;
using Ecommerce.WebApi.src.Database;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.WebApi.src.Data
{
    public class EcommerceDbContext : DbContext
    {
        //this is just a inject configuration so we can get connection string in appasettings.json
        protected readonly IConfiguration configuration;
        public DbSet<Category> Categories { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<ReviewImage> ReviewImages { get; set; }

        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<User> Users { get; set; }

        public DbSet<Address> Addresses { get; set; }

        public DbSet<RefreshToken> RefreshTokens { get; set; }
        private readonly ILoggerFactory _loggerFactory;

        public EcommerceDbContext(IConfiguration configuration, ILoggerFactory loggerFactory)
        {
            this.configuration = configuration;
            _loggerFactory = loggerFactory;
        }

        static EcommerceDbContext()
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseNpgsql(configuration.GetConnectionString("PgDbConnection"))
                .AddInterceptors(new TimestampInterceptor())
                .UseSnakeCaseNamingConvention()
                .AddInterceptors(
                    new SqlLoggingInterceptor(_loggerFactory.CreateLogger<SqlLoggingInterceptor>())
                )
                .EnableSensitiveDataLogging() // for development only, delete on prod
                .EnableDetailedErrors(); // for development only, delete on prod
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // craete enum table
            modelBuilder.HasPostgresEnum<UserRole>();
            modelBuilder.HasPostgresEnum<OrderStatus>();
            modelBuilder.HasPostgresEnum<SortBy>();
            modelBuilder.HasPostgresEnum<SortOrder>();

            // add constrain for database between tables as we cant do it using notation
            modelBuilder.HasPostgresExtension("uuid-ossp");

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("users");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Email).IsRequired();
                entity.HasIndex(p => p.Email).IsUnique();
                entity.Property(u => u.FirstName).IsRequired().HasMaxLength(255);
                entity.Property(u => u.LastName).IsRequired().HasMaxLength(255);
                entity.Property(u => u.Password).IsRequired().HasMaxLength(255);
                entity
                    .Property(e => e.Role)
                    .HasConversion(
                        v => v.ToString().ToLower(),
                        v =>
                            (UserRole)
                                Enum.Parse(typeof(UserRole), char.ToUpper(v[0]) + v.Substring(1))
                    )
                    .IsRequired();
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("categories");
                entity.HasKey(e => e.Id);
            });

            modelBuilder
                .Entity<ProductImage>()
                .HasOne(pi => pi.Product)
                .WithMany(p => p.Images)
                .HasForeignKey(pi => pi.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder
                .Entity<ReviewImage>()
                .HasOne(pi => pi.Review)
                .WithMany(p => p.Images)
                .HasForeignKey(pi => pi.ReviewId)
                .OnDelete(DeleteBehavior.Cascade);

            // Seed database

            var categories = SeedData.GenerateCategories();
            modelBuilder.Entity<Category>().HasData(categories);

            var users = SeedData.GenerateUsers();
            modelBuilder.Entity<User>().HasData(users);

            var products = SeedData.GenerateProducts(categories);
            modelBuilder.Entity<Product>().HasData(products);

            var productImages = SeedData.GenerateProductImages(products);
            modelBuilder.Entity<ProductImage>().HasData(productImages);

            var adddresses = SeedData.GenerateAddresses(users);
            modelBuilder.Entity<Address>().HasData(adddresses);

            var orders = SeedData.GenerateOrders(users, adddresses);
            modelBuilder.Entity<Order>().HasData(orders);

            var orderItems = SeedData.GenerateOrderItems(orders, products);
            modelBuilder.Entity<OrderItem>().HasData(orderItems);

            var reviews = SeedData.GenerateReviews(users, products);
            modelBuilder.Entity<Review>().HasData(reviews);

              var reviewImages = SeedData.GenerateReviewImages(reviews);
            modelBuilder.Entity<ReviewImage>().HasData(reviewImages);
        }
    }
}
