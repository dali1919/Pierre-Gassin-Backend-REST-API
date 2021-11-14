using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebsiteRESTAPI.Migrations
{
    public partial class first : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AuthTokens",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Token = table.Column<string>(type: "varchar(255)", nullable: true),
                    UserCode = table.Column<string>(type: "varchar(255)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthTokens", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Blog",
                columns: table => new
                {
                    BlogId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Cover = table.Column<string>(type: "varchar(100)", nullable: true),
                    Title = table.Column<string>(type: "varchar(100)", nullable: true),
                    Slug = table.Column<string>(type: "varchar(100)", nullable: true),
                    Description = table.Column<string>(type: "varchar(100)", nullable: true),
                    date = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Blog", x => x.BlogId);
                });

            migrationBuilder.CreateTable(
                name: "Coeffs",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Print = table.Column<double>(nullable: false),
                    Web = table.Column<double>(nullable: false),
                    Print_web = table.Column<double>(nullable: false),
                    National = table.Column<double>(nullable: false),
                    Europe = table.Column<double>(nullable: false),
                    Mondial = table.Column<double>(nullable: false),
                    Double = table.Column<double>(nullable: false),
                    Couverture = table.Column<double>(nullable: false),
                    Pleine = table.Column<double>(nullable: false),
                    Demi = table.Column<double>(nullable: false),
                    Quart = table.Column<double>(nullable: false),
                    n1000 = table.Column<double>(nullable: false),
                    n10000 = table.Column<double>(nullable: false),
                    n100000 = table.Column<double>(nullable: false),
                    ns100000 = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Coeffs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Image",
                columns: table => new
                {
                    ImageId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Tagged = table.Column<string>(type: "varchar(200)", nullable: true),
                    Main = table.Column<string>(type: "varchar(200)", nullable: true),
                    Taille1 = table.Column<string>(type: "varchar(200)", nullable: true),
                    Taille2 = table.Column<string>(type: "varchar(200)", nullable: true),
                    Taille3 = table.Column<string>(type: "varchar(200)", nullable: true),
                    Title = table.Column<string>(type: "varchar(100)", nullable: true),
                    ReportageId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Image", x => x.ImageId);
                });

            migrationBuilder.CreateTable(
                name: "ImagesVendues",
                columns: table => new
                {
                    TransactionId = table.Column<int>(nullable: false),
                    ImageId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "Reportage",
                columns: table => new
                {
                    ReportageId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "varchar(255)", nullable: true),
                    Description = table.Column<string>(type: "varchar(255)", nullable: true),
                    Likes = table.Column<long>(type: "bigint", nullable: false),
                    Slug = table.Column<string>(type: "varchar(100)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reportage", x => x.ReportageId);
                });

            migrationBuilder.CreateTable(
                name: "Transaction",
                columns: table => new
                {
                    TransactionId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "varchar(50)", nullable: true),
                    ImagesIds = table.Column<string>(type: "varchar(500)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transaction", x => x.TransactionId);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Email = table.Column<string>(type: "varchar(50)", nullable: false),
                    Password = table.Column<string>(type: "varchar(255)", nullable: false),
                    UserName = table.Column<string>(type: "varchar(255)", nullable: false),
                    FirstName = table.Column<string>(type: "varchar(50)", nullable: false),
                    LastName = table.Column<string>(type: "varchar(50)", nullable: false),
                    Company = table.Column<string>(type: "varchar(50)", nullable: true),
                    Country = table.Column<string>(type: "varchar(50)", nullable: false),
                    Tva = table.Column<string>(type: "varchar(50)", nullable: true),
                    Adress = table.Column<string>(type: "varchar(50)", nullable: true),
                    ZipCode = table.Column<int>(type: "int", nullable: false),
                    Phone = table.Column<int>(type: "int", nullable: false),
                    Appartement = table.Column<string>(type: "varchar(50)", nullable: true),
                    Region = table.Column<string>(type: "varchar(50)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Email);
                });

            migrationBuilder.CreateTable(
                name: "RefreshToken",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Token = table.Column<string>(nullable: true),
                    Expires = table.Column<DateTime>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    CreatedByIp = table.Column<string>(nullable: true),
                    Revoked = table.Column<DateTime>(nullable: true),
                    RevokedByIp = table.Column<string>(nullable: true),
                    ReplacedByToken = table.Column<string>(nullable: true),
                    UserEmail = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshToken", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RefreshToken_User_UserEmail",
                        column: x => x.UserEmail,
                        principalTable: "User",
                        principalColumn: "Email",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RefreshToken_UserEmail",
                table: "RefreshToken",
                column: "UserEmail");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuthTokens");

            migrationBuilder.DropTable(
                name: "Blog");

            migrationBuilder.DropTable(
                name: "Coeffs");

            migrationBuilder.DropTable(
                name: "Image");

            migrationBuilder.DropTable(
                name: "ImagesVendues");

            migrationBuilder.DropTable(
                name: "RefreshToken");

            migrationBuilder.DropTable(
                name: "Reportage");

            migrationBuilder.DropTable(
                name: "Transaction");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
