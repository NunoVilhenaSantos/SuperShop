#nullable disable

using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

namespace SuperShop.Web.Migrations.DataContextMySqlMigrations;

public partial class AddCountriesAndCitiesWithUsersUpdate : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterDatabase()
            .Annotation("MySQL:Charset", "utf8mb4");

        migrationBuilder.CreateTable(
                "AspNetRoles",
                table => new
                {
                    Id = table.Column<string>("varchar(255)", nullable: false),
                    Name = table.Column<string>("varchar(256)", maxLength: 256,
                        nullable: true),
                    NormalizedName = table.Column<string>("varchar(256)",
                        maxLength: 256, nullable: true),
                    ConcurrencyStamp =
                        table.Column<string>("longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                })
            .Annotation("MySQL:Charset", "utf8mb4");

        migrationBuilder.CreateTable(
                "Countries",
                table => new
                {
                    Id = table.Column<int>("int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy",
                            MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>("varchar(50)", maxLength: 50,
                        nullable: false),
                    WasDeleted =
                        table.Column<bool>("tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries", x => x.Id);
                })
            .Annotation("MySQL:Charset", "utf8mb4");

        migrationBuilder.CreateTable(
                "AspNetRoleClaims",
                table => new
                {
                    Id = table.Column<int>("int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy",
                            MySQLValueGenerationStrategy.IdentityColumn),
                    RoleId =
                        table.Column<string>("varchar(255)", nullable: false),
                    ClaimType =
                        table.Column<string>("longtext", nullable: true),
                    ClaimValue =
                        table.Column<string>("longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        x => x.RoleId,
                        "AspNetRoles",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                })
            .Annotation("MySQL:Charset", "utf8mb4");

        migrationBuilder.CreateTable(
                "Cities",
                table => new
                {
                    Id = table.Column<int>("int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy",
                            MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>("varchar(50)", maxLength: 50,
                        nullable: false),
                    WasDeleted =
                        table.Column<bool>("tinyint(1)", nullable: false),
                    CountryId = table.Column<int>("int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cities", x => x.Id);
                    table.ForeignKey(
                        "FK_Cities_Countries_CountryId",
                        x => x.CountryId,
                        "Countries",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                })
            .Annotation("MySQL:Charset", "utf8mb4");

        migrationBuilder.CreateTable(
                "AspNetUsers",
                table => new
                {
                    Id = table.Column<string>("varchar(255)", nullable: false),
                    FirstName = table.Column<string>("varchar(50)",
                        maxLength: 50, nullable: true),
                    LastName = table.Column<string>("varchar(50)",
                        maxLength: 50, nullable: true),
                    Address = table.Column<string>("varchar(100)",
                        maxLength: 100, nullable: true),
                    CountryId = table.Column<int>("int", nullable: false),
                    CityId = table.Column<int>("int", nullable: true),
                    UserName = table.Column<string>("varchar(256)",
                        maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>("varchar(256)",
                        maxLength: 256, nullable: true),
                    Email = table.Column<string>("varchar(256)", maxLength: 256,
                        nullable: true),
                    NormalizedEmail = table.Column<string>("varchar(256)",
                        maxLength: 256, nullable: true),
                    EmailConfirmed =
                        table.Column<bool>("tinyint(1)", nullable: false),
                    PasswordHash =
                        table.Column<string>("longtext", nullable: true),
                    SecurityStamp =
                        table.Column<string>("longtext", nullable: true),
                    ConcurrencyStamp =
                        table.Column<string>("longtext", nullable: true),
                    PhoneNumber =
                        table.Column<string>("longtext", nullable: true),
                    PhoneNumberConfirmed =
                        table.Column<bool>("tinyint(1)", nullable: false),
                    TwoFactorEnabled =
                        table.Column<bool>("tinyint(1)", nullable: false),
                    LockoutEnd =
                        table.Column<DateTimeOffset>("datetime(6)",
                            nullable: true),
                    LockoutEnabled =
                        table.Column<bool>("tinyint(1)", nullable: false),
                    AccessFailedCount =
                        table.Column<int>("int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                    table.ForeignKey(
                        "FK_AspNetUsers_Cities_CityId",
                        x => x.CityId,
                        "Cities",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                })
            .Annotation("MySQL:Charset", "utf8mb4");

        migrationBuilder.CreateTable(
                "AspNetUserClaims",
                table => new
                {
                    Id = table.Column<int>("int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy",
                            MySQLValueGenerationStrategy.IdentityColumn),
                    UserId =
                        table.Column<string>("varchar(255)", nullable: false),
                    ClaimType =
                        table.Column<string>("longtext", nullable: true),
                    ClaimValue =
                        table.Column<string>("longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        "FK_AspNetUserClaims_AspNetUsers_UserId",
                        x => x.UserId,
                        "AspNetUsers",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                })
            .Annotation("MySQL:Charset", "utf8mb4");

        migrationBuilder.CreateTable(
                "AspNetUserLogins",
                table => new
                {
                    LoginProvider =
                        table.Column<string>("varchar(255)", nullable: false),
                    ProviderKey =
                        table.Column<string>("varchar(255)", nullable: false),
                    ProviderDisplayName =
                        table.Column<string>("longtext", nullable: true),
                    UserId =
                        table.Column<string>("varchar(255)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins",
                        x => new {x.LoginProvider, x.ProviderKey});
                    table.ForeignKey(
                        "FK_AspNetUserLogins_AspNetUsers_UserId",
                        x => x.UserId,
                        "AspNetUsers",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                })
            .Annotation("MySQL:Charset", "utf8mb4");

        migrationBuilder.CreateTable(
                "AspNetUserRoles",
                table => new
                {
                    UserId =
                        table.Column<string>("varchar(255)", nullable: false),
                    RoleId =
                        table.Column<string>("varchar(255)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles",
                        x => new {x.UserId, x.RoleId});
                    table.ForeignKey(
                        "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        x => x.RoleId,
                        "AspNetRoles",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        "FK_AspNetUserRoles_AspNetUsers_UserId",
                        x => x.UserId,
                        "AspNetUsers",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                })
            .Annotation("MySQL:Charset", "utf8mb4");

        migrationBuilder.CreateTable(
                "AspNetUserTokens",
                table => new
                {
                    UserId =
                        table.Column<string>("varchar(255)", nullable: false),
                    LoginProvider =
                        table.Column<string>("varchar(255)", nullable: false),
                    Name =
                        table.Column<string>("varchar(255)", nullable: false),
                    Value = table.Column<string>("longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens",
                        x => new {x.UserId, x.LoginProvider, x.Name});
                    table.ForeignKey(
                        "FK_AspNetUserTokens_AspNetUsers_UserId",
                        x => x.UserId,
                        "AspNetUsers",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                })
            .Annotation("MySQL:Charset", "utf8mb4");

        migrationBuilder.CreateTable(
                "Orders",
                table => new
                {
                    Id = table.Column<int>("int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy",
                            MySQLValueGenerationStrategy.IdentityColumn),
                    OrderDate =
                        table.Column<DateTime>("datetime(6)", nullable: false),
                    DeliveryDate =
                        table.Column<DateTime>("datetime(6)", nullable: true),
                    UserId =
                        table.Column<string>("varchar(255)", nullable: false),
                    WasDeleted =
                        table.Column<bool>("tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        "FK_Orders_AspNetUsers_UserId",
                        x => x.UserId,
                        "AspNetUsers",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                })
            .Annotation("MySQL:Charset", "utf8mb4");

        migrationBuilder.CreateTable(
                "Products",
                table => new
                {
                    Id = table.Column<int>("int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy",
                            MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>("varchar(50)", maxLength: 50,
                        nullable: false),
                    Price = table.Column<decimal>("decimal(18,2)",
                        nullable: false),
                    ImageUrl = table.Column<string>("longtext", nullable: true),
                    ImageId = table.Column<Guid>("char(36)", nullable: false),
                    ImageIdGcp =
                        table.Column<Guid>("char(36)", nullable: false),
                    ImageIdAws =
                        table.Column<Guid>("char(36)", nullable: false),
                    LastPurchase =
                        table.Column<DateTime>("datetime(6)", nullable: true),
                    LastSale =
                        table.Column<DateTime>("datetime(6)", nullable: true),
                    IsAvailable =
                        table.Column<bool>("tinyint(1)", nullable: false),
                    Stock = table.Column<double>("double", nullable: false),
                    UserId =
                        table.Column<string>("varchar(255)", nullable: false),
                    WasDeleted =
                        table.Column<bool>("tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        "FK_Products_AspNetUsers_UserId",
                        x => x.UserId,
                        "AspNetUsers",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                })
            .Annotation("MySQL:Charset", "utf8mb4");

        migrationBuilder.CreateTable(
                "OrderDetails",
                table => new
                {
                    Id = table.Column<int>("int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy",
                            MySQLValueGenerationStrategy.IdentityColumn),
                    OrderId = table.Column<int>("int", nullable: false),
                    ProductId = table.Column<int>("int", nullable: false),
                    Price = table.Column<decimal>("decimal(18,2)",
                        precision: 18, scale: 2, nullable: false),
                    Quantity = table.Column<double>("double", nullable: false),
                    WasDeleted =
                        table.Column<bool>("tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderDetails", x => x.Id);
                    table.ForeignKey(
                        "FK_OrderDetails_Orders_OrderId",
                        x => x.OrderId,
                        "Orders",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        "FK_OrderDetails_Products_ProductId",
                        x => x.ProductId,
                        "Products",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                })
            .Annotation("MySQL:Charset", "utf8mb4");

        migrationBuilder.CreateTable(
                "OrderDetailTemps",
                table => new
                {
                    Id = table.Column<int>("int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy",
                            MySQLValueGenerationStrategy.IdentityColumn),
                    UserId =
                        table.Column<string>("varchar(255)", nullable: false),
                    ProductId = table.Column<int>("int", nullable: false),
                    Price = table.Column<decimal>("decimal(18,2)",
                        precision: 18, scale: 2, nullable: false),
                    Quantity = table.Column<double>("double", nullable: false),
                    WasDeleted =
                        table.Column<bool>("tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderDetailTemps", x => x.Id);
                    table.ForeignKey(
                        "FK_OrderDetailTemps_AspNetUsers_UserId",
                        x => x.UserId,
                        "AspNetUsers",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        "FK_OrderDetailTemps_Products_ProductId",
                        x => x.ProductId,
                        "Products",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                })
            .Annotation("MySQL:Charset", "utf8mb4");

        migrationBuilder.CreateIndex(
            "IX_AspNetRoleClaims_RoleId",
            "AspNetRoleClaims",
            "RoleId");

        migrationBuilder.CreateIndex(
            "RoleNameIndex",
            "AspNetRoles",
            "NormalizedName",
            unique: true);

        migrationBuilder.CreateIndex(
            "IX_AspNetUserClaims_UserId",
            "AspNetUserClaims",
            "UserId");

        migrationBuilder.CreateIndex(
            "IX_AspNetUserLogins_UserId",
            "AspNetUserLogins",
            "UserId");

        migrationBuilder.CreateIndex(
            "IX_AspNetUserRoles_RoleId",
            "AspNetUserRoles",
            "RoleId");

        migrationBuilder.CreateIndex(
            "EmailIndex",
            "AspNetUsers",
            "NormalizedEmail");

        migrationBuilder.CreateIndex(
            "IX_AspNetUsers_CityId",
            "AspNetUsers",
            "CityId");

        migrationBuilder.CreateIndex(
            "UserNameIndex",
            "AspNetUsers",
            "NormalizedUserName",
            unique: true);

        migrationBuilder.CreateIndex(
            "IX_Cities_CountryId",
            "Cities",
            "CountryId");

        migrationBuilder.CreateIndex(
            "IX_OrderDetails_OrderId",
            "OrderDetails",
            "OrderId");

        migrationBuilder.CreateIndex(
            "IX_OrderDetails_ProductId",
            "OrderDetails",
            "ProductId");

        migrationBuilder.CreateIndex(
            "IX_OrderDetailTemps_ProductId",
            "OrderDetailTemps",
            "ProductId");

        migrationBuilder.CreateIndex(
            "IX_OrderDetailTemps_UserId",
            "OrderDetailTemps",
            "UserId");

        migrationBuilder.CreateIndex(
            "IX_Orders_UserId",
            "Orders",
            "UserId");

        migrationBuilder.CreateIndex(
            "IX_Products_UserId",
            "Products",
            "UserId");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            "AspNetRoleClaims");

        migrationBuilder.DropTable(
            "AspNetUserClaims");

        migrationBuilder.DropTable(
            "AspNetUserLogins");

        migrationBuilder.DropTable(
            "AspNetUserRoles");

        migrationBuilder.DropTable(
            "AspNetUserTokens");

        migrationBuilder.DropTable(
            "OrderDetails");

        migrationBuilder.DropTable(
            "OrderDetailTemps");

        migrationBuilder.DropTable(
            "AspNetRoles");

        migrationBuilder.DropTable(
            "Orders");

        migrationBuilder.DropTable(
            "Products");

        migrationBuilder.DropTable(
            "AspNetUsers");

        migrationBuilder.DropTable(
            "Cities");

        migrationBuilder.DropTable(
            "Countries");
    }
}