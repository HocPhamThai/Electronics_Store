namespace Electronics_Store.Server.DatabaseContext;

public class ElectronicsStoreDbContext:DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Picture> Pictures { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Address> Addresses { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<CartProduct> CartProducts { get; set; }
    public DbSet<OrderProduct> OrderProducts { get; set; }
    public DbSet<ProductVariant> ProductVariants { get; set; }
    public DbSet<ProductVariety> ProductVarieties { get; set; }
    
    public ElectronicsStoreDbContext(DbContextOptions<ElectronicsStoreDbContext> options): base(options) {}
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CartProduct>().HasKey(
            cartProduct => new {cartProduct.userId, cartProduct.productId, cartProduct.productVarietyId}
        );
        
        modelBuilder.Entity<ProductVariant>().HasKey (
            productVariant => new {productVariant.productId, productVariant.productVarietyId}
        );
        
        modelBuilder.Entity<OrderProduct>()
            .HasKey(orderProduct => new {orderProduct.orderId, orderProduct.productId, orderProduct.productVarietyId});




        modelBuilder.Entity<ProductVariety>().HasData(
              // Laptops
              new ProductVariety { Id = 1, Name = "Default Laptop" },
              new ProductVariety { Id = 2, Name = "8GB RAM Laptop" },
              new ProductVariety { Id = 3, Name = "16GB RAM Laptop" },
              new ProductVariety { Id = 4, Name = "256GB SSD Laptop" },
              new ProductVariety { Id = 5, Name = "512GB SSD Laptop" },
              new ProductVariety { Id = 6, Name = "Gaming Laptop" },
              new ProductVariety { Id = 7, Name = "Ultra Thin Laptop" },

              // Smartphones
              new ProductVariety { Id = 8, Name = "Default Smartphone" },
              new ProductVariety { Id = 9, Name = "64GB Storage Smartphone" },
              new ProductVariety { Id = 10, Name = "128GB Storage Smartphone" },
              new ProductVariety { Id = 11, Name = "5G Smartphone" },
              new ProductVariety { Id = 12, Name = "Flagship Smartphone" },

              // Headphones
              new ProductVariety { Id = 13, Name = "Default Headphones" },
              new ProductVariety { Id = 14, Name = "Wireless Headphones" },
              new ProductVariety { Id = 15, Name = "Noise-Canceling Headphones" },
              new ProductVariety { Id = 16, Name = "Over-Ear Headphones" },
              new ProductVariety { Id = 17, Name = "In-Ear Headphones" }
          );
        modelBuilder.Entity<Category>().HasData(
            new Category
            {
                CategoryId = 1,
                CategoryName = "Laptops",
                CategoryAccessUrl = "laptops"
            },
            new Category
            {
                CategoryId = 2,
                CategoryName = "SmartPhones",
                CategoryAccessUrl = "smartphones"
            },
            new Category
            {
                CategoryId = 3,
                CategoryName = "HeadPhones",
                CategoryAccessUrl = "headphones"
            }
        );
        modelBuilder.Entity<Product>().HasData(
            new Product
            {
                ProductId = 1,
                ProductName = "High-Performance Laptop",
                ProductDetail = "Experience unparalleled computing power with our High-Performance Gaming Laptop. This powerful machine is crafted for all your computing needs, especially gaming. Immerse yourself in cutting-edge technology and elevate your gaming experience to new heights.",
                ProductImageUrl = "https://images.unsplash.com/photo-1525547719571-a2d4ac8945e2?w=500&auto=format&fit=crop&q=60&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxzZWFyY2h8NHx8bGFwdG9wfGVufDB8fDB8fHww",
                ProductCategoryId = 1,
                IsTopProduct = true
            },
            new Product
            {
                ProductId = 2,
                ProductName = "Medium-Performance Laptop for Everyday Use",
                ProductDetail = "Introducing our Medium-Performance Laptop, a versatile device perfect for everyday use. Whether you're working, studying, or enjoying entertainment, this laptop offers a reliable performance. Stay connected and productive with this sleek and efficient companion",
                ProductImageUrl = "https://images.unsplash.com/photo-1525547719571-a2d4ac8945e2?w=500&auto=format&fit=crop&q=60&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxzZWFyY2h8NHx8bGFwdG9wfGVufDB8fDB8fHww",
                ProductCategoryId = 1
            },
            new Product
            {
                ProductId = 3,
                ProductName = "Energy-Efficient Laptop for Extended Battery Life",
                ProductDetail = "Meet our Save-Battery Laptop, designed to optimize energy efficiency and provide an extended battery life. Perfect for users on the go, this laptop ensures you stay connected without worrying about running out of power. Experience reliability and sustainability in one compact device.",
                ProductImageUrl = "https://images.unsplash.com/photo-1525547719571-a2d4ac8945e2?w=500&auto=format&fit=crop&q=60&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxzZWFyY2h8NHx8bGFwdG9wfGVufDB8fDB8fHww",
                ProductCategoryId = 1
            },
            new Product
            {
                ProductId = 4,
                ProductCategoryId = 2,
                ProductName = "Flagship Smartphone - TechMaster X1",
                ProductDetail = "Experience the pinnacle of mobile technology with the TechMaster X1, our flagship smartphone. Packed with cutting-edge features, this device is designed for power users who demand the best. From its stunning display to its advanced camera capabilities, the TechMaster X1 redefines what a smartphone can be.",
                ProductImageUrl = "https://images.unsplash.com/photo-1511707171634-5f897ff02aa9?w=500&auto=format&fit=crop&q=60&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxzZWFyY2h8M3x8cGhvbmV8ZW58MHx8MHx8fDA%3D",
            },
            new Product
            {
                ProductId = 5,
                ProductCategoryId = 2,
                ProductName = "Slim and Stylish - EchoSlim S22",
                ProductDetail = "Introducing the EchoSlim S22, a sleek and stylish smartphone that doesn't compromise on performance. With its slim design and powerful features, the EchoSlim S22 is perfect for users who prioritize both aesthetics and functionality. Stay connected in style.",
                ProductImageUrl = "https://images.unsplash.com/photo-1511707171634-5f897ff02aa9?w=500&auto=format&fit=crop&q=60&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxzZWFyY2h8M3x8cGhvbmV8ZW58MHx8MHx8fDA%3D",
                IsTopProduct = true
            },
            new Product
            {
                ProductId = 6,
                ProductCategoryId = 2,
                ProductName = "Budget-Friendly Smart Choice - SmartPlus Z3",
                ProductDetail = "Get the most value for your money with the SmartPlus Z3, our budget-friendly smart choice. This smartphone offers a balance of performance and affordability, making it ideal for users looking for a reliable device without breaking the bank. Embrace smart living with the SmartPlus Z3.",
                ProductImageUrl = "https://images.unsplash.com/photo-1511707171634-5f897ff02aa9?w=500&auto=format&fit=crop&q=60&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxzZWFyY2h8M3x8cGhvbmV8ZW58MHx8MHx8fDA%3D",
            },
            new Product
            {
                ProductId = 7,
                ProductCategoryId = 3,
                ProductName = "Premium Over-Ear Headphones - AcousticX Pro",
                ProductDetail = "Immerse yourself in exceptional sound quality with the AcousticX Pro over-ear headphones. Designed for audiophiles, these premium headphones deliver crystal-clear audio and a comfortable listening experience. Elevate your music and entertainment with AcousticX Pro.",
                ProductImageUrl = "https://images.unsplash.com/photo-1618366712010-f4ae9c647dcb?w=500&auto=format&fit=crop&q=60&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxzZWFyY2h8NHx8aGVhZHBob25lfGVufDB8fDB8fHww",

            },
            new Product
            {
                ProductId = 8,
                ProductCategoryId = 3,
                ProductName = "Wireless Noise-Canceling Earbuds - SonicZen X1",
                ProductDetail = "Experience the freedom of wireless audio with the SonicZen X1 earbuds. These sleek and compact earbuds feature advanced noise-canceling technology, providing a distraction-free listening experience. Enjoy your favorite music on the go with SonicZen X1.",
                ProductImageUrl = "https://images.unsplash.com/photo-1618366712010-f4ae9c647dcb?w=500&auto=format&fit=crop&q=60&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxzZWFyY2h8NHx8aGVhZHBob25lfGVufDB8fDB8fHww",
            },
            new Product
            {
                ProductId = 9,
                ProductCategoryId = 3,
                ProductName = "Foldable On-Ear Headphones - UrbanFlex FoldX",
                ProductDetail = "Experience flexibility and style with the UrbanFlex FoldX on-ear headphones. These foldable headphones are perfect for on-the-go lifestyles, offering a blend of comfort and portability. Enjoy your favorite tunes with UrbanFlex FoldX",
                ProductImageUrl = "https://images.unsplash.com/photo-1618366712010-f4ae9c647dcb?w=500&auto=format&fit=crop&q=60&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxzZWFyY2h8NHx8aGVhZHBob25lfGVufDB8fDB8fHww"
            },
            new Product
            {
                ProductId = 10,
                ProductCategoryId = 3,
                ProductName = "Sporty In-Ear Headphones - FitBeat Sports Edition",
                ProductDetail = "Stay motivated during workouts with the FitBeat Sports Edition in-ear headphones. Designed for active lifestyles, these sweat-resistant headphones offer a secure fit and powerful sound. Elevate your fitness journey with FitBeat.",
                ProductImageUrl = "https://images.unsplash.com/photo-1618366712010-f4ae9c647dcb?w=500&auto=format&fit=crop&q=60&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxzZWFyY2h8NHx8aGVhZHBob25lfGVufDB8fDB8fHww",
                IsTopProduct = true
            },
            new Product
            {
                ProductId = 11,
                ProductCategoryId = 3,
                ProductName = "Studio-Quality Professional Headset - StudioSound Pro",
                ProductDetail = "Unleash your creativity with the StudioSound Pro professional headset. Ideal for content creators and professionals, these studio-quality headphones deliver exceptional audio accuracy. Elevate your work with the precision of StudioSound Pro.",
                ProductImageUrl = "https://images.unsplash.com/photo-1618366712010-f4ae9c647dcb?w=500&auto=format&fit=crop&q=60&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxzZWFyY2h8NHx8aGVhZHBob25lfGVufDB8fDB8fHww",
            }
        );
        modelBuilder.Entity<ProductVariant>().HasData (
    // Variants for High-Performance Laptop (ProductId = 1)
            new ProductVariant
            {
                productId = 1,
                productVarietyId = 1,
                price = 79.99m,
                initialPrice = 399m
            },
            new ProductVariant
            {
                productId = 1,
                productVarietyId = 2,
                price = 89.99m,
                initialPrice = 449m
            },
            new ProductVariant
            {
                productId = 1,
                productVarietyId = 3,
                price = 99.99m,
                initialPrice = 499m
            },
            new ProductVariant
            {
                productId = 1,
                productVarietyId = 4,
                price = 109.99m,
                initialPrice = 549m
            },
            new ProductVariant
            {
                productId = 1,
                productVarietyId = 5,
                price = 119.99m,
                initialPrice = 599m
            },
            new ProductVariant
            {
                productId = 1,
                productVarietyId = 6,
                price = 129.99m,
                initialPrice = 649m
            },
            new ProductVariant
            {
                productId = 1,
                productVarietyId = 7,
                price = 139.99m,
                initialPrice = 699m
            },
            new ProductVariant
            {
                productId = 1,
                productVarietyId = 8,
                price = 149.99m,
                initialPrice = 749m
            },

            // Variants for Medium-Performance Laptop for Everyday Use (ProductId = 2)
            new ProductVariant
            {
                productId = 2,
                productVarietyId = 1,
                price = 59.99m,
                initialPrice = 299m
            },
            new ProductVariant
            {
                productId = 2,
                productVarietyId = 2,
                price = 69.99m,
                initialPrice = 349m
            },
            new ProductVariant
            {
                productId = 2,
                productVarietyId = 3,
                price = 79.99m,
                initialPrice = 399m
            },
            new ProductVariant
            {
                productId = 2,
                productVarietyId = 4,
                price = 89.99m,
                initialPrice = 449m
            },
            new ProductVariant
            {
                productId = 2,
                productVarietyId = 5,
                price = 99.99m,
                initialPrice = 499m
            },
            new ProductVariant
            {
                productId = 2,
                productVarietyId = 6,
                price = 109.99m,
                initialPrice = 549m
            },
            new ProductVariant
            {
                productId = 2,
                productVarietyId = 7,
                price = 119.99m,
                initialPrice = 599m
            },
            new ProductVariant
            {
                productId = 2,
                productVarietyId = 8,
                price = 129.99m,
                initialPrice = 649m
            },

            // Variants for Energy-Efficient Laptop for Extended Battery Life (ProductId = 3)
            new ProductVariant
            {
                productId = 3,
                productVarietyId = 1,
                price = 69.99m,
                initialPrice = 349m
            },
            new ProductVariant
            {
                productId = 3,
                productVarietyId = 2,
                price = 79.99m,
                initialPrice = 399m
            },
            new ProductVariant
            {
                productId = 3,
                productVarietyId = 3,
                price = 89.99m,
                initialPrice = 449m
            },
            new ProductVariant
            {
                productId = 3,
                productVarietyId = 4,
                price = 99.99m,
                initialPrice = 499m
            },
            new ProductVariant
            {
                productId = 3,
                productVarietyId = 5,
                price = 109.99m,
                initialPrice = 549m
            },
            new ProductVariant
            {
                productId = 3,
                productVarietyId = 6,
                price = 119.99m,
                initialPrice = 599m
            },
            new ProductVariant
            {
                productId = 3,
                productVarietyId = 7,
                price = 129.99m,
                initialPrice = 649m
            },
            new ProductVariant
            {
                productId = 3,
                productVarietyId = 8,
                price = 139.99m,
                initialPrice = 699m
            },
            // Variants for Flagship Smartphone - TechMaster X1 (ProductId = 4)
            new ProductVariant
            {
                productId = 4,
                productVarietyId = 8,
                price = 599.99m,
                initialPrice = 799.99m
            },
            new ProductVariant
            {
                productId = 4,
                productVarietyId = 9,
                price = 649.99m,
                initialPrice = 849.99m
            },
            new ProductVariant
            {
                productId = 4,
                productVarietyId = 10,
                price = 699.99m,
                initialPrice = 899.99m
            },
            new ProductVariant
            {
                productId = 4,
                productVarietyId = 11,
                price = 749.99m,
                initialPrice = 999.99m
            },
            new ProductVariant
            {
                productId = 4,
                productVarietyId = 12,
                price = 799.99m,
                initialPrice = 1099.99m
            },

            // Variants for Slim and Stylish - EchoSlim S22 (ProductId = 5)
            new ProductVariant
            {
                productId = 5,
                productVarietyId = 8,
                price = 349.99m,
                initialPrice = 499.99m
            },
            new ProductVariant
            {
                productId = 5,
                productVarietyId = 9,
                price = 399.99m,
                initialPrice = 549.99m
            },
            new ProductVariant
            {
                productId = 5,
                productVarietyId = 10,
                price = 449.99m,
                initialPrice = 599.99m
            },
            new ProductVariant
            {
                productId = 5,
                productVarietyId = 11,
                price = 499.99m,
                initialPrice = 649.99m
            },
            new ProductVariant
            {
                productId = 5,
                productVarietyId = 12,
                price = 549.99m,
                initialPrice = 699.99m
            },

            // Variants for Budget-Friendly Smart Choice - SmartPlus Z3 (ProductId = 6)
            new ProductVariant
            {
                productId = 6,
                productVarietyId = 8,
                price = 149.99m,
                initialPrice = 199.99m
            },
            new ProductVariant
            {
                productId = 6,
                productVarietyId = 9,
                price = 169.99m,
                initialPrice = 219.99m
            },
            new ProductVariant
            {
                productId = 6,
                productVarietyId = 10,
                price = 189.99m,
                initialPrice = 239.99m
            },
            // Variants for Premium Over-Ear Headphones - AcousticX Pro (ProductId = 7)
            new ProductVariant
            {
                productId = 7,
                productVarietyId = 13,
                price = 149.99m,
                initialPrice = 199.99m
            },
            new ProductVariant
            {
                productId = 7,
                productVarietyId = 14,
                price = 179.99m,
                initialPrice = 229.99m
            },
            // Variants for Wireless Noise-Canceling Earbuds - SonicZen X1 (ProductId = 8)
            new ProductVariant
            {
                productId = 8,
                productVarietyId = 16,
                price = 109.99m,
                initialPrice = 149.99m
            },
            new ProductVariant
            {
                productId = 8,
                productVarietyId = 17,
                price = 119.99m,
                initialPrice = 159.99m
            },

            // Variants for Foldable On-Ear Headphones - UrbanFlex FoldX (ProductId = 9)
            new ProductVariant
            {
                productId = 9,
                productVarietyId = 14,
                price = 59.99m,
                initialPrice = 79.99m
            },

            // Variants for Sporty In-Ear Headphones - FitBeat Sports Edition (ProductId = 10)
            new ProductVariant
            {
                productId = 10,
                productVarietyId = 14,
                price = 49.99m,
                initialPrice = 59.99m
            },

            // Variants for Studio-Quality Professional Headset - StudioSound Pro (ProductId = 11)
            new ProductVariant
            {
                productId = 11,
                productVarietyId = 17,
                price = 149.99m,
                initialPrice = 199.99m
            },
            new ProductVariant
            {
                productId = 11,
                productVarietyId = 16,
                price = 169.99m,
                initialPrice = 219.99m
            }
        );
    }
}