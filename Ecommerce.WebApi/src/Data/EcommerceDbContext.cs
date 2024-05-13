using Ecommerce.Core.src.Entity;
using Ecommerce.Core.src.ValueObject;
using Ecommerce.WebApi.src.Data.Interceptors;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.WebApi.src.Data
{
    public class EcommerceDbContext : DbContext
    {
        //this is just a inject configuration so we can get connection string in appasettings.json
        protected readonly IConfiguration configuration;
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Review> Reviews { get; set; }

        public DbSet<User>? Users { get; set; }

        private readonly ILoggerFactory _loggerFactory;

        public EcommerceDbContext(IConfiguration configuration, ILoggerFactory loggerFactory)
        {
            this.configuration = configuration;
            _loggerFactory = loggerFactory;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(configuration.GetConnectionString("PgDbConnection")).UseSnakeCaseNamingConvention().AddInterceptors(new SqlLoggingInterceptor(_loggerFactory.CreateLogger<SqlLoggingInterceptor>()));
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
                entity.Property(e => e.Role)
                    .HasConversion(
                        v => v.ToString().ToLower(),
                        v => (UserRole)Enum.Parse(typeof(UserRole), char.ToUpper(v[0]) + v.Substring(1))
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
                entity.HasOne(e => e.Category)
                    .WithMany()
                    .HasForeignKey(e => e.CategoryId);
            });

            modelBuilder.Entity<Address>(entity =>
            {
                entity.ToTable("addresses");
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.User)
                    .WithMany()
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("orders");
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.User)
                    .WithMany()
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(e => e.Address)
                    .WithMany()
                    .HasForeignKey(e => e.AddressId)
                    .OnDelete(DeleteBehavior.SetNull);
                entity.Property(e => e.Status)
                .HasConversion(
                    v => v.ToString(),
                    v => (OrderStatus)Enum.Parse(typeof(OrderStatus), v) // String to enum
                )
                .IsRequired();
            });

            modelBuilder.Entity<OrderItem>(entity =>
            {
                entity.ToTable("order_items");
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.Product)
                    .WithMany()
                    .HasForeignKey(e => e.ProductId)
                    .OnDelete(DeleteBehavior.SetNull);
                entity.HasOne(e => e.Order)
                    .WithMany()
                    .HasForeignKey(e => e.OrderId)
                    .OnDelete(DeleteBehavior.Cascade);

                // Add unique constraint for order_id and product_id combination
                entity.HasIndex(e => new { e.OrderId, e.ProductId }).IsUnique();
            });

            modelBuilder.Entity<Review>(entity =>
            {
                entity.ToTable("reviews");
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.User)
                    .WithMany()
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(e => e.product)
                .WithMany()
                .HasForeignKey(e => e.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Image>(entity =>
            {
                entity.ToTable("images");
                entity.HasKey(e => e.Id);

                // Define the foreign key relationship with the products table
                entity.Property(e => e.EntityId)
                    .IsRequired();

                entity.Property(e => e.Url)
                    .IsRequired();

                entity.Property(e => e.CreatedAt)
                    .HasDefaultValueSql("NOW()");

                entity.Property(e => e.UpdatedAt)
                    .HasDefaultValueSql("NOW()");
            });

            // Add dummy data

            var categorySeed = new CategorySeed(this);
            modelBuilder.Entity<Category>().HasData(categorySeed.SeedCategories("../data/category.csv"));

            var userSeed = new UserSeed(this);
            modelBuilder.Entity<User>().HasData(userSeed.SeedUsers("../data/users.csv"));

            var productSeed = new ProductSeed(this);
            modelBuilder.Entity<Product>().HasData(productSeed.SeedProducts("../data/products.csv"));

            var adddressSeed = new AddressSeed(this);
            modelBuilder.Entity<Address>().HasData(adddressSeed.SeedAddresses("../data/addresses.csv"));

            var orderSeed = new OrderSeed(this);
            modelBuilder.Entity<Order>().HasData(orderSeed.SeedOrders("../data/orders.csv"));

            var orderItemSeed = new OrderItemSeed(this);
            modelBuilder.Entity<OrderItem>().HasData(orderItemSeed.SeedOrderItem("../data/order_items.csv"));

            var reviewSeed = new ReviewSeed(this);
            modelBuilder.Entity<Review>().HasData(reviewSeed.SeedReviews("../data/review.csv"));

            var imageSeed = new ImageSeed(this);
            modelBuilder.Entity<Image>().HasData(imageSeed.SeedImage("../data/images.csv"));


        }

        public List<T> ReadFileSeedData<T>(string path, Func<string[], T> createObject)
        {
            var dataList = new List<T>();

            using (var reader = new StreamReader(path))
            {
                // Read the first line (header) and discard it
                reader.ReadLine();
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    var fields = line.Split(';');
                    var dataObject = createObject(fields);
                    dataList.Add(dataObject);
                }
            }

            return dataList;
        }
    }
}