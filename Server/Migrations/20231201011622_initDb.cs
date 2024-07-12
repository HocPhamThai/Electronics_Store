using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Electronics_Store.Server.Migrations
{
    /// <inheritdoc />
    public partial class initDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CartProducts",
                columns: table => new
                {
                    productId = table.Column<int>(type: "int", nullable: false),
                    productVarietyId = table.Column<int>(type: "int", nullable: false),
                    userId = table.Column<int>(type: "int", nullable: false),
                    quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartProducts", x => new { x.userId, x.productId, x.productVarietyId });
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    isViewable = table.Column<bool>(type: "bit", nullable: false),
                    isDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CategoryName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CategoryAccessUrl = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.CategoryId);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    orderId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    orderPrice = table.Column<decimal>(type: "decimal(20,2)", nullable: false),
                    orderDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    userId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.orderId);
                });

            migrationBuilder.CreateTable(
                name: "ProductVarieties",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductVarieties", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    userId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    userEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    userPasswordHashed = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    userPasswordSalt = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    accountCreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.userId);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    ProductId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductCategoryId = table.Column<int>(type: "int", nullable: false),
                    IsTopProduct = table.Column<bool>(type: "bit", nullable: false),
                    isDeleted = table.Column<bool>(type: "bit", nullable: false),
                    isViewable = table.Column<bool>(type: "bit", nullable: false),
                    ProductName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProductDetail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProductImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.ProductId);
                    table.ForeignKey(
                        name: "FK_Products_Categories_ProductCategoryId",
                        column: x => x.ProductCategoryId,
                        principalTable: "Categories",
                        principalColumn: "CategoryId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Addresses",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    userId = table.Column<int>(type: "int", nullable: false),
                    firstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    lastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    country = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    city = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    district = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    street = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    zip = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addresses", x => x.id);
                    table.ForeignKey(
                        name: "FK_Addresses_Users_userId",
                        column: x => x.userId,
                        principalTable: "Users",
                        principalColumn: "userId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderProducts",
                columns: table => new
                {
                    productId = table.Column<int>(type: "int", nullable: false),
                    productVarietyId = table.Column<int>(type: "int", nullable: false),
                    orderId = table.Column<int>(type: "int", nullable: false),
                    quantity = table.Column<int>(type: "int", nullable: false),
                    orderProductPrice = table.Column<decimal>(type: "decimal(20,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderProducts", x => new { x.orderId, x.productId, x.productVarietyId });
                    table.ForeignKey(
                        name: "FK_OrderProducts_Orders_orderId",
                        column: x => x.orderId,
                        principalTable: "Orders",
                        principalColumn: "orderId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderProducts_ProductVarieties_productVarietyId",
                        column: x => x.productVarietyId,
                        principalTable: "ProductVarieties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderProducts_Products_productId",
                        column: x => x.productId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Pictures",
                columns: table => new
                {
                    pictureId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    pictureInfo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pictures", x => x.pictureId);
                    table.ForeignKey(
                        name: "FK_Pictures_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId");
                });

            migrationBuilder.CreateTable(
                name: "ProductVariants",
                columns: table => new
                {
                    productId = table.Column<int>(type: "int", nullable: false),
                    productVarietyId = table.Column<int>(type: "int", nullable: false),
                    isDeleted = table.Column<bool>(type: "bit", nullable: false),
                    isViewable = table.Column<bool>(type: "bit", nullable: false),
                    price = table.Column<decimal>(type: "Decimal(20,2)", nullable: false),
                    initialPrice = table.Column<decimal>(type: "Decimal(20,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductVariants", x => new { x.productId, x.productVarietyId });
                    table.ForeignKey(
                        name: "FK_ProductVariants_ProductVarieties_productVarietyId",
                        column: x => x.productVarietyId,
                        principalTable: "ProductVarieties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductVariants_Products_productId",
                        column: x => x.productId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "CategoryId", "CategoryAccessUrl", "CategoryName", "isDeleted", "isViewable" },
                values: new object[,]
                {
                    { 1, "laptops", "Laptops", false, true },
                    { 2, "smartphones", "SmartPhones", false, true },
                    { 3, "headphones", "HeadPhones", false, true }
                });

            migrationBuilder.InsertData(
                table: "ProductVarieties",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Default Laptop" },
                    { 2, "8GB RAM Laptop" },
                    { 3, "16GB RAM Laptop" },
                    { 4, "256GB SSD Laptop" },
                    { 5, "512GB SSD Laptop" },
                    { 6, "Gaming Laptop" },
                    { 7, "Ultra Thin Laptop" },
                    { 8, "Default Smartphone" },
                    { 9, "64GB Storage Smartphone" },
                    { 10, "128GB Storage Smartphone" },
                    { 11, "5G Smartphone" },
                    { 12, "Flagship Smartphone" },
                    { 13, "Default Headphones" },
                    { 14, "Wireless Headphones" },
                    { 15, "Noise-Canceling Headphones" },
                    { 16, "Over-Ear Headphones" },
                    { 17, "In-Ear Headphones" }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "ProductId", "IsTopProduct", "ProductCategoryId", "ProductDetail", "ProductImageUrl", "ProductName", "isDeleted", "isViewable" },
                values: new object[,]
                {
                    { 1, true, 1, "Experience unparalleled computing power with our High-Performance Gaming Laptop. This powerful machine is crafted for all your computing needs, especially gaming. Immerse yourself in cutting-edge technology and elevate your gaming experience to new heights.", "https://images.unsplash.com/photo-1525547719571-a2d4ac8945e2?w=500&auto=format&fit=crop&q=60&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxzZWFyY2h8NHx8bGFwdG9wfGVufDB8fDB8fHww", "High-Performance Laptop", false, true },
                    { 2, false, 1, "Introducing our Medium-Performance Laptop, a versatile device perfect for everyday use. Whether you're working, studying, or enjoying entertainment, this laptop offers a reliable performance. Stay connected and productive with this sleek and efficient companion", "https://images.unsplash.com/photo-1525547719571-a2d4ac8945e2?w=500&auto=format&fit=crop&q=60&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxzZWFyY2h8NHx8bGFwdG9wfGVufDB8fDB8fHww", "Medium-Performance Laptop for Everyday Use", false, true },
                    { 3, false, 1, "Meet our Save-Battery Laptop, designed to optimize energy efficiency and provide an extended battery life. Perfect for users on the go, this laptop ensures you stay connected without worrying about running out of power. Experience reliability and sustainability in one compact device.", "https://images.unsplash.com/photo-1525547719571-a2d4ac8945e2?w=500&auto=format&fit=crop&q=60&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxzZWFyY2h8NHx8bGFwdG9wfGVufDB8fDB8fHww", "Energy-Efficient Laptop for Extended Battery Life", false, true },
                    { 4, false, 2, "Experience the pinnacle of mobile technology with the TechMaster X1, our flagship smartphone. Packed with cutting-edge features, this device is designed for power users who demand the best. From its stunning display to its advanced camera capabilities, the TechMaster X1 redefines what a smartphone can be.", "https://images.unsplash.com/photo-1511707171634-5f897ff02aa9?w=500&auto=format&fit=crop&q=60&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxzZWFyY2h8M3x8cGhvbmV8ZW58MHx8MHx8fDA%3D", "Flagship Smartphone - TechMaster X1", false, true },
                    { 5, true, 2, "Introducing the EchoSlim S22, a sleek and stylish smartphone that doesn't compromise on performance. With its slim design and powerful features, the EchoSlim S22 is perfect for users who prioritize both aesthetics and functionality. Stay connected in style.", "https://images.unsplash.com/photo-1511707171634-5f897ff02aa9?w=500&auto=format&fit=crop&q=60&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxzZWFyY2h8M3x8cGhvbmV8ZW58MHx8MHx8fDA%3D", "Slim and Stylish - EchoSlim S22", false, true },
                    { 6, false, 2, "Get the most value for your money with the SmartPlus Z3, our budget-friendly smart choice. This smartphone offers a balance of performance and affordability, making it ideal for users looking for a reliable device without breaking the bank. Embrace smart living with the SmartPlus Z3.", "https://images.unsplash.com/photo-1511707171634-5f897ff02aa9?w=500&auto=format&fit=crop&q=60&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxzZWFyY2h8M3x8cGhvbmV8ZW58MHx8MHx8fDA%3D", "Budget-Friendly Smart Choice - SmartPlus Z3", false, true },
                    { 7, false, 3, "Immerse yourself in exceptional sound quality with the AcousticX Pro over-ear headphones. Designed for audiophiles, these premium headphones deliver crystal-clear audio and a comfortable listening experience. Elevate your music and entertainment with AcousticX Pro.", "https://images.unsplash.com/photo-1618366712010-f4ae9c647dcb?w=500&auto=format&fit=crop&q=60&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxzZWFyY2h8NHx8aGVhZHBob25lfGVufDB8fDB8fHww", "Premium Over-Ear Headphones - AcousticX Pro", false, true },
                    { 8, false, 3, "Experience the freedom of wireless audio with the SonicZen X1 earbuds. These sleek and compact earbuds feature advanced noise-canceling technology, providing a distraction-free listening experience. Enjoy your favorite music on the go with SonicZen X1.", "https://images.unsplash.com/photo-1618366712010-f4ae9c647dcb?w=500&auto=format&fit=crop&q=60&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxzZWFyY2h8NHx8aGVhZHBob25lfGVufDB8fDB8fHww", "Wireless Noise-Canceling Earbuds - SonicZen X1", false, true },
                    { 9, false, 3, "Experience flexibility and style with the UrbanFlex FoldX on-ear headphones. These foldable headphones are perfect for on-the-go lifestyles, offering a blend of comfort and portability. Enjoy your favorite tunes with UrbanFlex FoldX", "https://images.unsplash.com/photo-1618366712010-f4ae9c647dcb?w=500&auto=format&fit=crop&q=60&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxzZWFyY2h8NHx8aGVhZHBob25lfGVufDB8fDB8fHww", "Foldable On-Ear Headphones - UrbanFlex FoldX", false, true },
                    { 10, true, 3, "Stay motivated during workouts with the FitBeat Sports Edition in-ear headphones. Designed for active lifestyles, these sweat-resistant headphones offer a secure fit and powerful sound. Elevate your fitness journey with FitBeat.", "https://images.unsplash.com/photo-1618366712010-f4ae9c647dcb?w=500&auto=format&fit=crop&q=60&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxzZWFyY2h8NHx8aGVhZHBob25lfGVufDB8fDB8fHww", "Sporty In-Ear Headphones - FitBeat Sports Edition", false, true },
                    { 11, false, 3, "Unleash your creativity with the StudioSound Pro professional headset. Ideal for content creators and professionals, these studio-quality headphones deliver exceptional audio accuracy. Elevate your work with the precision of StudioSound Pro.", "https://images.unsplash.com/photo-1618366712010-f4ae9c647dcb?w=500&auto=format&fit=crop&q=60&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxzZWFyY2h8NHx8aGVhZHBob25lfGVufDB8fDB8fHww", "Studio-Quality Professional Headset - StudioSound Pro", false, true }
                });

            migrationBuilder.InsertData(
                table: "ProductVariants",
                columns: new[] { "productId", "productVarietyId", "initialPrice", "isDeleted", "isViewable", "price" },
                values: new object[,]
                {
                    { 1, 1, 399m, false, true, 79.99m },
                    { 1, 2, 449m, false, true, 89.99m },
                    { 1, 3, 499m, false, true, 99.99m },
                    { 1, 4, 549m, false, true, 109.99m },
                    { 1, 5, 599m, false, true, 119.99m },
                    { 1, 6, 649m, false, true, 129.99m },
                    { 1, 7, 699m, false, true, 139.99m },
                    { 1, 8, 749m, false, true, 149.99m },
                    { 2, 1, 299m, false, true, 59.99m },
                    { 2, 2, 349m, false, true, 69.99m },
                    { 2, 3, 399m, false, true, 79.99m },
                    { 2, 4, 449m, false, true, 89.99m },
                    { 2, 5, 499m, false, true, 99.99m },
                    { 2, 6, 549m, false, true, 109.99m },
                    { 2, 7, 599m, false, true, 119.99m },
                    { 2, 8, 649m, false, true, 129.99m },
                    { 3, 1, 349m, false, true, 69.99m },
                    { 3, 2, 399m, false, true, 79.99m },
                    { 3, 3, 449m, false, true, 89.99m },
                    { 3, 4, 499m, false, true, 99.99m },
                    { 3, 5, 549m, false, true, 109.99m },
                    { 3, 6, 599m, false, true, 119.99m },
                    { 3, 7, 649m, false, true, 129.99m },
                    { 3, 8, 699m, false, true, 139.99m },
                    { 4, 8, 799.99m, false, true, 599.99m },
                    { 4, 9, 849.99m, false, true, 649.99m },
                    { 4, 10, 899.99m, false, true, 699.99m },
                    { 4, 11, 999.99m, false, true, 749.99m },
                    { 4, 12, 1099.99m, false, true, 799.99m },
                    { 5, 8, 499.99m, false, true, 349.99m },
                    { 5, 9, 549.99m, false, true, 399.99m },
                    { 5, 10, 599.99m, false, true, 449.99m },
                    { 5, 11, 649.99m, false, true, 499.99m },
                    { 5, 12, 699.99m, false, true, 549.99m },
                    { 6, 8, 199.99m, false, true, 149.99m },
                    { 6, 9, 219.99m, false, true, 169.99m },
                    { 6, 10, 239.99m, false, true, 189.99m },
                    { 7, 13, 199.99m, false, true, 149.99m },
                    { 7, 14, 229.99m, false, true, 179.99m },
                    { 8, 16, 149.99m, false, true, 109.99m },
                    { 8, 17, 159.99m, false, true, 119.99m },
                    { 9, 14, 79.99m, false, true, 59.99m },
                    { 10, 14, 59.99m, false, true, 49.99m },
                    { 11, 16, 219.99m, false, true, 169.99m },
                    { 11, 17, 199.99m, false, true, 149.99m }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_userId",
                table: "Addresses",
                column: "userId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrderProducts_productId",
                table: "OrderProducts",
                column: "productId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderProducts_productVarietyId",
                table: "OrderProducts",
                column: "productVarietyId");

            migrationBuilder.CreateIndex(
                name: "IX_Pictures_ProductId",
                table: "Pictures",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_ProductCategoryId",
                table: "Products",
                column: "ProductCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductVariants_productVarietyId",
                table: "ProductVariants",
                column: "productVarietyId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Addresses");

            migrationBuilder.DropTable(
                name: "CartProducts");

            migrationBuilder.DropTable(
                name: "OrderProducts");

            migrationBuilder.DropTable(
                name: "Pictures");

            migrationBuilder.DropTable(
                name: "ProductVariants");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "ProductVarieties");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
