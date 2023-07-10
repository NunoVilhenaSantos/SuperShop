#nullable disable

using Microsoft.EntityFrameworkCore.Migrations;

namespace SuperShop.Web.Migrations;

public partial class AddCountriesAndCitiesWithUsersUpdate : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            "FK_City_Countries_CountryId",
            "City");

        migrationBuilder.DropPrimaryKey(
            "PK_City",
            "City");

        migrationBuilder.RenameTable(
            "City",
            newName: "Cities");

        migrationBuilder.RenameIndex(
            "IX_City_CountryId",
            table: "Cities",
            newName: "IX_Cities_CountryId");

        migrationBuilder.AlterColumn<string>(
            "LastName",
            "AspNetUsers",
            "nvarchar(50)",
            maxLength: 50,
            nullable: true,
            oldClrType: typeof(string),
            oldType: "nvarchar(max)",
            oldNullable: true);

        migrationBuilder.AlterColumn<string>(
            "FirstName",
            "AspNetUsers",
            "nvarchar(50)",
            maxLength: 50,
            nullable: true,
            oldClrType: typeof(string),
            oldType: "nvarchar(max)",
            oldNullable: true);

        migrationBuilder.AddColumn<string>(
            "Address",
            "AspNetUsers",
            "nvarchar(100)",
            maxLength: 100,
            nullable: true);

        migrationBuilder.AddColumn<int>(
            "CityId",
            "AspNetUsers",
            "int",
            nullable: true);

        migrationBuilder.AddColumn<int>(
            "CountryId",
            "AspNetUsers",
            "int",
            nullable: false,
            defaultValue: 0);

        migrationBuilder.AddPrimaryKey(
            "PK_Cities",
            "Cities",
            "Id");

        migrationBuilder.CreateIndex(
            "IX_AspNetUsers_CityId",
            "AspNetUsers",
            "CityId");

        migrationBuilder.AddForeignKey(
            "FK_AspNetUsers_Cities_CityId",
            "AspNetUsers",
            "CityId",
            "Cities",
            principalColumn: "Id",
            onDelete: ReferentialAction.Restrict);

        migrationBuilder.AddForeignKey(
            "FK_Cities_Countries_CountryId",
            "Cities",
            "CountryId",
            "Countries",
            principalColumn: "Id",
            onDelete: ReferentialAction.Restrict);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            "FK_AspNetUsers_Cities_CityId",
            "AspNetUsers");

        migrationBuilder.DropForeignKey(
            "FK_Cities_Countries_CountryId",
            "Cities");

        migrationBuilder.DropIndex(
            "IX_AspNetUsers_CityId",
            "AspNetUsers");

        migrationBuilder.DropPrimaryKey(
            "PK_Cities",
            "Cities");

        migrationBuilder.DropColumn(
            "Address",
            "AspNetUsers");

        migrationBuilder.DropColumn(
            "CityId",
            "AspNetUsers");

        migrationBuilder.DropColumn(
            "CountryId",
            "AspNetUsers");

        migrationBuilder.RenameTable(
            "Cities",
            newName: "City");

        migrationBuilder.RenameIndex(
            "IX_Cities_CountryId",
            table: "City",
            newName: "IX_City_CountryId");

        migrationBuilder.AlterColumn<string>(
            "LastName",
            "AspNetUsers",
            "nvarchar(max)",
            nullable: true,
            oldClrType: typeof(string),
            oldType: "nvarchar(50)",
            oldMaxLength: 50,
            oldNullable: true);

        migrationBuilder.AlterColumn<string>(
            "FirstName",
            "AspNetUsers",
            "nvarchar(max)",
            nullable: true,
            oldClrType: typeof(string),
            oldType: "nvarchar(50)",
            oldMaxLength: 50,
            oldNullable: true);

        migrationBuilder.AddPrimaryKey(
            "PK_City",
            "City",
            "Id");

        migrationBuilder.AddForeignKey(
            "FK_City_Countries_CountryId",
            "City",
            "CountryId",
            "Countries",
            principalColumn: "Id",
            onDelete: ReferentialAction.Restrict);
    }
}