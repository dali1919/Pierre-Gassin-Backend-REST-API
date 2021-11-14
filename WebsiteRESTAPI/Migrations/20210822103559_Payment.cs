using Microsoft.EntityFrameworkCore.Migrations;

namespace WebsiteRESTAPI.Migrations
{
    public partial class Payment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImagesIds",
                table: "Transaction",
                newName: "ImageSeizes");

            migrationBuilder.AddColumn<string>(
                name: "ImageId",
                table: "Transaction",
                type: "varchar(500)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageId",
                table: "Transaction");

            migrationBuilder.RenameColumn(
                name: "ImageSeizes",
                table: "Transaction",
                newName: "ImagesIds");
        }
    }
}
