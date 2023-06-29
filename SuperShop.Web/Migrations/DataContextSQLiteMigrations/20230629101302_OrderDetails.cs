#nullable disable

using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SuperShop.Web.Migrations.DataContextSQLiteMigrations;

public partial class OrderDetails : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            "AspNetRoles",
            table => new
            {
                Id = table.Column<string>("TEXT", nullable: false),
                Name = table.Column<string>("TEXT", maxLength: 256,
                    nullable: true),
                NormalizedName = table.Column<string>("TEXT", maxLength: 256,
                    nullable: true),
                ConcurrencyStamp = table.Column<string>("TEXT", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_AspNetRoles", x => x.Id);
            });

        migrationBuilder.CreateTable(
            "AspNetUsers",
            table => new
            {
                Id = table.Column<string>("TEXT", nullable: false),
                FirstName = table.Column<string>("TEXT", nullable: true),
                LastName = table.Column<string>("TEXT", nullable: true),
                UserName = table.Column<string>("TEXT", maxLength: 256,
                    nullable: true),
                NormalizedUserName = table.Column<string>("TEXT",
                    maxLength: 256, nullable: true),
                Email = table.Column<string>("TEXT", maxLength: 256,
                    nullable: true),
                NormalizedEmail = table.Column<string>("TEXT", maxLength: 256,
                    nullable: true),
                EmailConfirmed = table.Column<bool>("INTEGER", nullable: false),
                PasswordHash = table.Column<string>("TEXT", nullable: true),
                SecurityStamp = table.Column<string>("TEXT", nullable: true),
                ConcurrencyStamp = table.Column<string>("TEXT", nullable: true),
                PhoneNumber = table.Column<string>("TEXT", nullable: true),
                PhoneNumberConfirmed =
                    table.Column<bool>("INTEGER", nullable: false),
                TwoFactorEnabled =
                    table.Column<bool>("INTEGER", nullable: false),
                LockoutEnd =
                    table.Column<DateTimeOffset>("TEXT", nullable: true),
                LockoutEnabled = table.Column<bool>("INTEGER", nullable: false),
                AccessFailedCount =
                    table.Column<int>("INTEGER", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_AspNetUsers", x => x.Id);
            });

        migrationBuilder.CreateTable(
            "AspNetRoleClaims",
            table => new
            {
                Id = table.Column<int>("INTEGER", nullable: false)
                    .Annotation("Sqlite:Autoincrement", true),
                RoleId = table.Column<string>("TEXT", nullable: false),
                ClaimType = table.Column<string>("TEXT", nullable: true),
                ClaimValue = table.Column<string>("TEXT", nullable: true)
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
            });

        migrationBuilder.CreateTable(
            "AspNetUserClaims",
            table => new
            {
                Id = table.Column<int>("INTEGER", nullable: false)
                    .Annotation("Sqlite:Autoincrement", true),
                UserId = table.Column<string>("TEXT", nullable: false),
                ClaimType = table.Column<string>("TEXT", nullable: true),
                ClaimValue = table.Column<string>("TEXT", nullable: true)
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
            });

        migrationBuilder.CreateTable(
            "AspNetUserLogins",
            table => new
            {
                LoginProvider = table.Column<string>("TEXT", nullable: false),
                ProviderKey = table.Column<string>("TEXT", nullable: false),
                ProviderDisplayName =
                    table.Column<string>("TEXT", nullable: true),
                UserId = table.Column<string>("TEXT", nullable: false)
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
            });

        migrationBuilder.CreateTable(
            "AspNetUserRoles",
            table => new
            {
                UserId = table.Column<string>("TEXT", nullable: false),
                RoleId = table.Column<string>("TEXT", nullable: false)
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
            });

        migrationBuilder.CreateTable(
            "AspNetUserTokens",
            table => new
            {
                UserId = table.Column<string>("TEXT", nullable: false),
                LoginProvider = table.Column<string>("TEXT", nullable: false),
                Name = table.Column<string>("TEXT", nullable: false),
                Value = table.Column<string>("TEXT", nullable: true)
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
            });

        migrationBuilder.CreateTable(
            "Products",
            table => new
            {
                Id = table.Column<int>("INTEGER", nullable: false)
                    .Annotation("Sqlite:Autoincrement", true),
                Name = table.Column<string>("TEXT", maxLength: 50,
                    nullable: false),
                Price =
                    table.Column<decimal>("decimal(18, 2)", nullable: false),
                ImageUrl = table.Column<string>("TEXT", nullable: true),
                ImageId = table.Column<Guid>("TEXT", nullable: false),
                ImageIdGcp = table.Column<Guid>("TEXT", nullable: false),
                ImageIdAws = table.Column<Guid>("TEXT", nullable: false),
                LastPurchase = table.Column<DateTime>("TEXT", nullable: true),
                LastSale = table.Column<DateTime>("TEXT", nullable: true),
                IsAvailable = table.Column<bool>("INTEGER", nullable: false),
                Stock = table.Column<double>("REAL", nullable: false),
                UserId = table.Column<string>("TEXT", nullable: false),
                WasDeleted = table.Column<bool>("INTEGER", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Products", x => x.Id);
                table.ForeignKey(
                    "FK_Products_AspNetUsers_UserId",
                    x => x.UserId,
                    "AspNetUsers",
                    "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            "OrderDetailTemps",
            table => new
            {
                Id = table.Column<int>("INTEGER", nullable: false)
                    .Annotation("Sqlite:Autoincrement", true),
                UserId = table.Column<string>("TEXT", nullable: false),
                ProductId = table.Column<int>("INTEGER", nullable: false),
                Price = table.Column<decimal>("TEXT", precision: 18, scale: 2,
                    nullable: false),
                Quantity = table.Column<double>("REAL", nullable: false),
                WasDeleted = table.Column<bool>("INTEGER", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_OrderDetailTemps", x => x.Id);
                table.ForeignKey(
                    "FK_OrderDetailTemps_AspNetUsers_UserId",
                    x => x.UserId,
                    "AspNetUsers",
                    "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    "FK_OrderDetailTemps_Products_ProductId",
                    x => x.ProductId,
                    "Products",
                    "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            "Orders",
            table => new
            {
                Id = table.Column<int>("INTEGER", nullable: false)
                    .Annotation("Sqlite:Autoincrement", true),
                UserId = table.Column<string>("TEXT", nullable: false),
                OrderDateId = table.Column<int>("INTEGER", nullable: false),
                DeliveryDateId = table.Column<int>("INTEGER", nullable: false),
                WasDeleted = table.Column<bool>("INTEGER", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Orders", x => x.Id);
                table.ForeignKey(
                    "FK_Orders_AspNetUsers_UserId",
                    x => x.UserId,
                    "AspNetUsers",
                    "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    "FK_Orders_Products_DeliveryDateId",
                    x => x.DeliveryDateId,
                    "Products",
                    "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    "FK_Orders_Products_OrderDateId",
                    x => x.OrderDateId,
                    "Products",
                    "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            "OrderDetails",
            table => new
            {
                Id = table.Column<int>("INTEGER", nullable: false)
                    .Annotation("Sqlite:Autoincrement", true),
                ProductId = table.Column<int>("INTEGER", nullable: false),
                Price = table.Column<decimal>("TEXT", precision: 18, scale: 2,
                    nullable: false),
                Quantity = table.Column<double>("REAL", nullable: false),
                WasDeleted = table.Column<bool>("INTEGER", nullable: false),
                OrderId = table.Column<int>("INTEGER", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_OrderDetails", x => x.Id);
                table.ForeignKey(
                    "FK_OrderDetails_Orders_OrderId",
                    x => x.OrderId,
                    "Orders",
                    "Id");
                table.ForeignKey(
                    "FK_OrderDetails_Products_ProductId",
                    x => x.ProductId,
                    "Products",
                    "Id",
                    onDelete: ReferentialAction.Cascade);
            });

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
            "UserNameIndex",
            "AspNetUsers",
            "NormalizedUserName",
            unique: true);

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
            "IX_Orders_DeliveryDateId",
            "Orders",
            "DeliveryDateId");

        migrationBuilder.CreateIndex(
            "IX_Orders_OrderDateId",
            "Orders",
            "OrderDateId");

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
    }
}