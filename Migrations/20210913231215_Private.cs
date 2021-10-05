using Microsoft.EntityFrameworkCore.Migrations;

namespace Lets2Chat.Migrations
{
    public partial class Private : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TargetId",
                table: "Message",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TargetName",
                table: "Message",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "targetUserId",
                table: "Message",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Message_targetUserId",
                table: "Message",
                column: "targetUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Message_AspNetUsers_targetUserId",
                table: "Message",
                column: "targetUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Message_AspNetUsers_targetUserId",
                table: "Message");

            migrationBuilder.DropIndex(
                name: "IX_Message_targetUserId",
                table: "Message");

            migrationBuilder.DropColumn(
                name: "TargetId",
                table: "Message");

            migrationBuilder.DropColumn(
                name: "TargetName",
                table: "Message");

            migrationBuilder.DropColumn(
                name: "targetUserId",
                table: "Message");
        }
    }
}
