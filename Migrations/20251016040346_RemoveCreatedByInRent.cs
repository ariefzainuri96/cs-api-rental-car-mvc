using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace cs_api_rental_car_mvc.Migrations
{
    public partial class RemoveCreatedByInRent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "created_by",
                table: "Rents");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "deleted_at",
                table: "users",
                nullable: true,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "deleted_at",
                table: "Rents",
                nullable: true,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset");

            migrationBuilder.AlterColumn<DateTime>(
                name: "actual_end_date",
                table: "Rents",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "deleted_at",
                table: "cars",
                nullable: true,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset");

            migrationBuilder.CreateIndex(
                name: "IX_Rents_car_id",
                table: "Rents",
                column: "car_id");

            migrationBuilder.CreateIndex(
                name: "IX_Rents_user_id",
                table: "Rents",
                column: "user_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Rents_cars_car_id",
                table: "Rents",
                column: "car_id",
                principalTable: "cars",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Rents_users_user_id",
                table: "Rents",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rents_cars_car_id",
                table: "Rents");

            migrationBuilder.DropForeignKey(
                name: "FK_Rents_users_user_id",
                table: "Rents");

            migrationBuilder.DropIndex(
                name: "IX_Rents_car_id",
                table: "Rents");

            migrationBuilder.DropIndex(
                name: "IX_Rents_user_id",
                table: "Rents");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "deleted_at",
                table: "users",
                type: "datetimeoffset",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "deleted_at",
                table: "Rents",
                type: "datetimeoffset",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "actual_end_date",
                table: "Rents",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "created_by",
                table: "Rents",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "deleted_at",
                table: "cars",
                type: "datetimeoffset",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldNullable: true);
        }
    }
}
