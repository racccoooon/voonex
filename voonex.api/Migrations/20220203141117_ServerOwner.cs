using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace voonex.api.Migrations
{
    public partial class ServerOwner : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ServerOwnerId",
                table: "Server",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Server_ServerOwnerId",
                table: "Server",
                column: "ServerOwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Server_User_ServerOwnerId",
                table: "Server",
                column: "ServerOwnerId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Server_User_ServerOwnerId",
                table: "Server");

            migrationBuilder.DropIndex(
                name: "IX_Server_ServerOwnerId",
                table: "Server");

            migrationBuilder.DropColumn(
                name: "ServerOwnerId",
                table: "Server");
        }
    }
}
