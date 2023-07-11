using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TMX.TaskService.WebApi.Migrations
{
    public partial class AddNotificationSentFieldToUserTasksTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "NotificationSent",
                table: "UserTasks",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NotificationSent",
                table: "UserTasks");
        }
    }
}
