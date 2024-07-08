using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodShop.Migrations
{
    /// <inheritdoc />
    public partial class FoodShopNET105Identity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_foods_admins_AdminCode",
                table: "foods");

            migrationBuilder.DropForeignKey(
                name: "FK_order_customer_CustomerEmail",
                table: "order");

            migrationBuilder.AlterColumn<string>(
                name: "State",
                table: "order",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<DateTime>(
                name: "OrderDate",
                table: "order",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<string>(
                name: "CustomerEmail",
                table: "order",
                type: "Varchar(200)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "Varchar(200)");

            migrationBuilder.AlterColumn<int>(
                name: "CInforId",
                table: "order",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "Sold",
                table: "foods",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "PreviousPrice",
                table: "foods",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "AdminCode",
                table: "foods",
                type: "Char(5)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "Char(5)");

            migrationBuilder.AlterColumn<bool>(
                name: "IsOnl",
                table: "admins",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "admins",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddForeignKey(
                name: "FK_foods_admins_AdminCode",
                table: "foods",
                column: "AdminCode",
                principalTable: "admins",
                principalColumn: "AdminCode");

            migrationBuilder.AddForeignKey(
                name: "FK_order_customer_CustomerEmail",
                table: "order",
                column: "CustomerEmail",
                principalTable: "customer",
                principalColumn: "Email");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_foods_admins_AdminCode",
                table: "foods");

            migrationBuilder.DropForeignKey(
                name: "FK_order_customer_CustomerEmail",
                table: "order");

            migrationBuilder.AlterColumn<string>(
                name: "State",
                table: "order",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "OrderDate",
                table: "order",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CustomerEmail",
                table: "order",
                type: "Varchar(200)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "Varchar(200)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CInforId",
                table: "order",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Sold",
                table: "foods",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "PreviousPrice",
                table: "foods",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AdminCode",
                table: "foods",
                type: "Char(5)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "Char(5)",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsOnl",
                table: "admins",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "admins",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_foods_admins_AdminCode",
                table: "foods",
                column: "AdminCode",
                principalTable: "admins",
                principalColumn: "AdminCode",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_order_customer_CustomerEmail",
                table: "order",
                column: "CustomerEmail",
                principalTable: "customer",
                principalColumn: "Email",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
