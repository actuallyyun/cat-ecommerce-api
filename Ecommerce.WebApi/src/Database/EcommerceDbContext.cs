using Ecommerce.Core.src.Entity;
using Ecommerce.Core.src.ValueObject;
using Ecommerce.WebApi.src.Data.Interceptors;
using Ecommerce.WebApi.src.Database;

//using Ecommerce.WebApi.src.Database.SeedingData;
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
        public DbSet<OrderItem>? OrderItems { get; set; }
        public DbSet<User>? Users { get; set; }

        public DbSet<Address>? Addresses { get; set; }

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

            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("products");
                entity.HasKey(e => e.Id);
                entity
                    .HasOne(e => e.Category)
                    .WithMany()
                    .HasForeignKey(e => e.CategoryId)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            //modelBuilder.Entity<Address>(entity =>
            //{
            //    entity.ToTable("addresses");
            //    entity.HasKey(e => e.Id);
            //    entity
            //        .HasOne(e => e.User)
            //        .WithMany()
            //        .HasForeignKey(e => e.UserId)
            //        .OnDelete(DeleteBehavior.Cascade);
            //});

            //modelBuilder.Entity<Order>(entity =>
            //{
            //    entity.ToTable("orders");
            //    entity.HasKey(e => e.Id);
            //    entity
            //        .HasOne(e => e.User)
            //        .WithMany()
            //        .HasForeignKey(e => e.UserId)
            //        .OnDelete(DeleteBehavior.Cascade);
            //    entity
            //        .HasOne(e => e.Address)
            //        .WithMany()
            //        .HasForeignKey(e => e.AddressId)
            //        .OnDelete(DeleteBehavior.SetNull);
            //    entity
            //        .Property(e => e.Status)
            //        .HasConversion(
            //            v => v.ToString(),
            //            v => (OrderStatus)Enum.Parse(typeof(OrderStatus), v) // String to enum
            //        )
            //        .IsRequired();
            //});

            //modelBuilder.Entity<OrderItem>(entity =>
            //{
            //    entity.ToTable("order_items");
            //    entity.HasKey(e => e.Id);
            //    entity
            //        .HasOne(e => e.Product)
            //        .WithMany()
            //        .HasForeignKey(e => e.ProductId)
            //        .OnDelete(DeleteBehavior.SetNull);
            //    entity
            //        .HasOne(e => e.Order)
            //        .WithMany(e=>e.OrderItems)
            //        .HasForeignKey(e => e.OrderId)
            //        .OnDelete(DeleteBehavior.Cascade);

            //    // Add unique constraint for order_id and product_id combination
            //    entity.HasIndex(e => new { e.OrderId, e.ProductId }).IsUnique();
            //});

            modelBuilder.Entity<Review>(entity =>
            {
                entity.ToTable("reviews");
                entity.HasKey(e => e.Id);
                entity.HasMany(e => e.Images).WithOne().HasForeignKey(e => e.ReviewId);
                entity
                    .HasOne(e => e.User)
                    .WithMany()
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
                entity
                    .HasOne(e => e.product)
                    .WithMany()
                    .HasForeignKey(e => e.ProductId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            //modelBuilder.Entity<ProductImage>(entity =>
            //{
            //    entity.ToTable("product_images");
            //    entity.HasKey(e => e.Id);
            //    entity
            //        .HasOne(e => e.Product)
            //        .WithMany()
            //        .HasForeignKey(e => e.ProductId)
            //        .OnDelete(DeleteBehavior.Cascade);
            //    // Define the foreign key relationship with the products table
            //    entity.Property(e => e.Data).IsRequired();
            //});

            //modelBuilder.Entity<ReviewImage>(entity =>
            //{
            //    entity.ToTable("review_images");
            //    entity.HasKey(e => e.Id);
            //    entity
            //        .HasOne(e => e.Review)
            //        .WithMany()
            //        .HasForeignKey(e => e.ReviewId)
            //        .OnDelete(DeleteBehavior.Cascade);
            //    // Define the foreign key relationship with the products table
            //    entity.Property(e => e.Data).IsRequired();
            //});

            // Seed database

            //var categories = SeedingData.GetCategories();
            //modelBuilder.Entity<Category>().HasData(categories);

            //var users = SeedingData.GetUsers();
            //modelBuilder.Entity<User>().HasData(users);

            //var products = SeedingData.GetProducts();
            //modelBuilder.Entity<Product>().HasData(products);

            //var adddresses = SeedingData.GetAddresses();
            //modelBuilder.Entity<Address>().HasData(adddresses);

            //var orders = SeedingData.GetOrders();
            //modelBuilder.Entity<Order>().HasData(orders);

            //var orderItems = SeedingData.GetOrderItems();
            //modelBuilder.Entity<OrderItem>().HasData(orderItems);

            //var reviews = SeedingData.GetReviews();
            //modelBuilder.Entity<Review>().HasData(reviews);
        }
    }
}
